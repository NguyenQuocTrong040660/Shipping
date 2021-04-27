using MediatR;
using ShippingApp.Application.Common.Results;
using ShippingApp.Application.Interfaces;
using System;
using Entities = ShippingApp.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ShippingApp.Application.ShippingRequest.Commands
{
    public class DeleteShippingRequestCommand : IRequest<Result>
    {
        public int Id { get; set; }
    }

    public class DeleteShippingRequestCommandHandler : IRequestHandler<DeleteShippingRequestCommand, Result>
    {
        private readonly IShippingAppRepository<Entities.ShippingRequest> _shippingAppRepository;
        private readonly IShippingAppDbContext _context;

        public DeleteShippingRequestCommandHandler(IShippingAppDbContext context, IShippingAppRepository<Entities.ShippingRequest> shippingAppRepository)
        {
            _shippingAppRepository = shippingAppRepository ?? throw new ArgumentNullException(nameof(shippingAppRepository));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Result> Handle(DeleteShippingRequestCommand request, CancellationToken cancellationToken)
        {
            var shippingPlans = await _context.ShippingPlans.Where(x => x.ShippingRequestId == request.Id).ToListAsync();

            shippingPlans.ForEach(i =>
            {
                i.ShippingRequestId = null;
            });

            await _context.SaveChangesAsync();

            return await _shippingAppRepository.DeleteAsync(request.Id);
        }
    }
}
