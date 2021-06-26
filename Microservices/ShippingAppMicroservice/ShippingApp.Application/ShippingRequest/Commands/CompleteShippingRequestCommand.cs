using MediatR;
using ShippingApp.Application.Common.Results;
using ShippingApp.Application.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.Extensions.Logging;
using ShippingApp.Domain.Enumerations;
using System.Linq;

namespace ShippingApp.Application.ShippingRequest.Commands
{
    public class CompleteShippingRequestCommand : IRequest<Result>
    {
        public int ShippingMarkId { get; set; }
    }

    public class CompleteShippingRequestCommandHandler : IRequestHandler<CompleteShippingRequestCommand, Result>
    {
        private readonly IShippingAppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<CompleteShippingRequestCommandHandler> _logger;

        public CompleteShippingRequestCommandHandler(IShippingAppDbContext context, IMapper mapper, ILogger<CompleteShippingRequestCommandHandler> logger)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Result> Handle(CompleteShippingRequestCommand request, CancellationToken cancellationToken)
        {
            var shippingMark = await _context.ShippingMarks.Include(x => x.ShippingMarkShippings).FirstOrDefaultAsync(x => x.Id == request.ShippingMarkId);

            if (shippingMark == null || !shippingMark.ShippingMarkShippings.Any())
            {
                return Result.Failure("Failed to complete Shipping request. Please try again later");
            }

            var shippingMarkShipping = shippingMark.ShippingMarkShippings.FirstOrDefault();

            var shippingRequest = await _context.ShippingRequests.FirstOrDefaultAsync(x => x.Id == shippingMarkShipping.ShippingRequestId);

            shippingRequest.Status = nameof(ShippingRequestStatus.Completed);

            if (await _context.SaveChangesAsync() == 0)
            {
                _logger.LogError($"Failed to complete Shipping Request");
            }

            return Result.Success();
        }
    }
}
