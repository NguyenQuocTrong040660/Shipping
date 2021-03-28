using MediatR;
using ShippingApp.Application.Common.Results;
using ShippingApp.Application.Interfaces;
using ShippingApp.Domain.Models;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using AutoMapper;
using Entities = ShippingApp.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace ShippingApp.Application.ShippingRequest.Commands
{
    public class UpdateShippingRequestCommand : IRequest<Result>
    {
        public int Id { get; set; }
        public ShippingRequestModel ShippingRequest { get; set; }
    }

    public class UpdateShippingRequestCommandHandler : IRequestHandler<UpdateShippingRequestCommand, Result>
    {
        private readonly IShippingAppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateShippingRequestCommandHandler> _logger;

        public UpdateShippingRequestCommandHandler(IShippingAppDbContext context, IMapper mapper, ILogger<UpdateShippingRequestCommandHandler> logger)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Result> Handle(UpdateShippingRequestCommand request, CancellationToken cancellationToken)
        {
            var shippingRequest = await _context.ShippingRequests
                .Include(x => x.ShippingRequestDetails)
                .Where(x => x.Id == request.ShippingRequest.Id)
                .FirstOrDefaultAsync();

            if (shippingRequest == null)
            {
                throw new ArgumentNullException(nameof(shippingRequest));
            }

            foreach (var item in shippingRequest.ShippingRequestDetails)
            {
                var shippingRequestDetail = request.ShippingRequest.ShippingRequestDetails.FirstOrDefault(i => i.ProductId == item.ProductId);

                if (shippingRequestDetail == null)
                {
                    _context.ShippingRequestDetails.Remove(item);
                }
                else
                {
                    item.Quantity = shippingRequestDetail.Quantity;
                    item.Price = shippingRequestDetail.Price;
                    item.Amount = shippingRequestDetail.Amount;
                    item.ShippingMode = shippingRequestDetail.ShippingMode;
                }
            }

            foreach (var item in request.ShippingRequest.ShippingRequestDetails)
            {
                var shippingRequestDetail = shippingRequest.ShippingRequestDetails.FirstOrDefault(i => i.ProductId == item.ProductId);

                if (shippingRequestDetail == null)
                {
                    var shippingRequestDetailEntity = _mapper.Map<Entities.ShippingRequestDetail>(item);
                    shippingRequestDetailEntity.ShippingRequestId = shippingRequest.Id;
                    _context.ShippingRequestDetails.Add(shippingRequestDetailEntity);
                }
            }

            //shippingRequest.CustomerName = request.ShippingRequest.CustomerName;
            //shippingRequest.SemlineNumber = request.ShippingRequest.SemlineNumber;
            //shippingRequest.PurchaseOrder = request.ShippingRequest.PurchaseOrder;
            //shippingRequest.ShippingDate = request.ShippingRequest.ShippingDate;
            //shippingRequest.SalesID = request.ShippingRequest.SalesID;

            shippingRequest.Notes = request.ShippingRequest.Notes;

            if (await _context.SaveChangesAsync() == 0)
            {
                _logger.LogError($"Failed to update Shipping Request");
            }

            return Result.Success();
        }
    }
}
