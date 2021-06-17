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
using Microsoft.EntityFrameworkCore;

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
            var receivedMarkPrinting = await _context.ReceivedMarkPrintings
                .FindAsync(request.UnstuffReceivedMark.ReceivedMarkPrintingId);

            if (receivedMarkPrinting == null)
            {
                throw new ArgumentNullException(nameof(receivedMarkPrinting));
            }

            if (receivedMarkPrinting.Status.Equals(nameof(ReceivedMarkStatus.Unstuff)))
            {
                return Result.Failure("Received mark has already been unstuffed. Please select another received mark");
            }

            if (receivedMarkPrinting.Quantity <= request.UnstuffReceivedMark.UnstuffQuantity)
            {
                return Result.Failure("Not enough quantity of Received Mark to unstuff");
            }

            var lastReceivedMark = _context.ReceivedMarkPrintings.Where(x => x.ReceivedMarkId == receivedMarkPrinting.ReceivedMarkId
                                                     && x.ProductId == receivedMarkPrinting.ProductId)
                                                                 .OrderByDescending(x => x.Sequence)
                                                                 .FirstOrDefault();

            if (lastReceivedMark == null)
            {
                throw new ArgumentNullException(nameof(lastReceivedMark));
            }

            receivedMarkPrinting.Status = nameof(ReceivedMarkStatus.Unstuff);
            receivedMarkPrinting.ParentId = request.UnstuffReceivedMark.ReceivedMarkPrintingId;

            var firstReceivedMark = new Entities.ReceivedMarkPrinting
            {
                Quantity = request.UnstuffReceivedMark.UnstuffQuantity,
                ReceivedMarkId = receivedMarkPrinting.ReceivedMarkId,
                ProductId = receivedMarkPrinting.ProductId,
                Sequence = receivedMarkPrinting.Sequence,
                Status = nameof(ReceivedMarkStatus.New),
                MovementRequestId = receivedMarkPrinting.MovementRequestId
            };

            var secondReceivedMark = new Entities.ReceivedMarkPrinting
            {
                Quantity = receivedMarkPrinting.Quantity - request.UnstuffReceivedMark.UnstuffQuantity,
                ReceivedMarkId = receivedMarkPrinting.ReceivedMarkId,
                ProductId = receivedMarkPrinting.ProductId,
                Sequence = lastReceivedMark.Sequence + 1,
                Status = nameof(ReceivedMarkStatus.New),
                MovementRequestId = receivedMarkPrinting.MovementRequestId
            };

            await _context.ReceivedMarkPrintings.AddAsync(firstReceivedMark);
            await _context.ReceivedMarkPrintings.AddAsync(secondReceivedMark);

            return await  _context.SaveChangesAsync(cancellationToken) > 0 
                ? Result.Success() 
                : Result.Failure("Failed to unstuff received mark");
        }
    }
}
