using MediatR;
using ShippingApp.Application.Common.Results;
using ShippingApp.Application.Interfaces;
using ShippingApp.Domain.Models;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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

        public UpdateShippingRequestCommandHandler(IShippingAppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
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
                }
            }

            shippingRequest.Notes = request.ShippingRequest.Notes;

            return await _context.SaveChangesAsync() > 0
                ? Result.Success()
                : Result.Failure($"Failed to update shipping request");
        }
    }
}
