using MediatR;
using ShippingApp.Application.Common.Results;
using ShippingApp.Application.Interfaces;
using ShippingApp.Domain.Models;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ShippingApp.Application.MovementRequest.Commands
{
    public class UpdateMovementRequestCommand : IRequest<Result>
    {
        public int Id { get; set; }
        public MovementRequestModel MovementRequest { get; set; }
    }

    public class UpdateMovementRequestCommandHandler : IRequestHandler<UpdateMovementRequestCommand, Result>
    {
        private readonly IShippingAppDbContext _context;

        public UpdateMovementRequestCommandHandler(IShippingAppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Result> Handle(UpdateMovementRequestCommand request, CancellationToken cancellationToken)
        {
            var movementRequest = await _context.MovementRequests
                .Include(x => x.MovementRequestDetails)
                .Where(x => x.Id == request.MovementRequest.Id)
                .FirstOrDefaultAsync();

            foreach (var item in movementRequest.MovementRequestDetails)
            {
                var movementRequestDetail = request.MovementRequest.MovementRequestDetails
                    .FirstOrDefault(i => i.ProductId == item.ProductId && i.WorkOrderId == item.WorkOrderId);

                if (movementRequestDetail == null)
                {
                    _context.MovementRequestDetails.Remove(item);
                }
                else
                {
                    item.Quantity = movementRequestDetail.Quantity;
                }
            }

            movementRequest.Notes = request.MovementRequest.Notes;

            return await _context.SaveChangesAsync() > 0
                ? Result.Success()
                : Result.Failure($"Failed to update movement request");
        }
    }
}
