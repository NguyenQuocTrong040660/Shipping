using MediatR;
using ShippingApp.Application.Common.Results;
using ShippingApp.Application.Interfaces;
using ShippingApp.Domain.Models;
using System;
using Entities = ShippingApp.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using ShippingApp.Domain.Enumerations;

namespace ShippingApp.Application.ReceivedMark.Commands
{
    public class UnstuffReceivedMarkCommand : IRequest<Result>
    {
        public UnstuffReceivedMarkRequest UnstuffReceivedMark { get; set; }
    }

    public class UnstuffReceivedMarkCommandHandler : IRequestHandler<UnstuffReceivedMarkCommand, Result>
    {
        private readonly IShippingAppDbContext _context;

        public UnstuffReceivedMarkCommandHandler(
            IShippingAppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Result> Handle(UnstuffReceivedMarkCommand request, CancellationToken cancellationToken)
        {
            var receivedMark = await _context.ReceivedMarks.FindAsync(request.UnstuffReceivedMark.Id);

            if (receivedMark == null)
            {
                throw new ArgumentNullException(nameof(receivedMark));
            }

            if (receivedMark.Status.Equals(nameof(ReceiveMarkStatus.Unstuff)))
            {
                return Result.Failure("Received mark has already been unstuffed. Please select another received mark");
            }

            if (receivedMark.Quantity <= request.UnstuffReceivedMark.UnstuffQuantity)
            {
                return Result.Failure("Not enough quantity of Received Mark to unstuff");
            }

            var lastReceivedMark = _context.ReceivedMarks.Where(x => x.MovementRequestId == receivedMark.MovementRequestId
                                                     && x.ProductId == receivedMark.ProductId)
                                                 .OrderByDescending(x => x.Sequence)
                                                 .FirstOrDefault();

            if (lastReceivedMark == null)
            {
                throw new ArgumentNullException(nameof(lastReceivedMark));
            }

            var firstReceivedMark = new Entities.ReceivedMark
            {
                Quantity = request.UnstuffReceivedMark.UnstuffQuantity,
                MovementRequestId = receivedMark.MovementRequestId,
                Notes = string.Empty,
                ProductId = receivedMark.ProductId,
                Sequence = lastReceivedMark.Sequence + 1,
                Status = nameof(ReceiveMarkStatus.Storage)
            };

            var secondReceivedMark = new Entities.ReceivedMark
            {
                Quantity = receivedMark.Quantity - request.UnstuffReceivedMark.UnstuffQuantity,
                MovementRequestId = receivedMark.MovementRequestId,
                Notes = string.Empty,
                ProductId = receivedMark.ProductId,
                Sequence = lastReceivedMark.Sequence + 2,
                Status = nameof(ReceiveMarkStatus.Storage)
            };

            receivedMark.Status = nameof(ReceiveMarkStatus.Unstuff);
            receivedMark.ParentId = request.UnstuffReceivedMark.Identifier;

            await _context.ReceivedMarks.AddAsync(firstReceivedMark);
            await _context.ReceivedMarks.AddAsync(secondReceivedMark);

            return await  _context.SaveChangesAsync() > 0 
                ? Result.Success() 
                : Result.Failure("Failed to unstuff received mark");
        }
    }
}
