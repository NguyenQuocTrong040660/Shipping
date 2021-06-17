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
using System.Collections.Generic;

namespace ShippingApp.Application.ReceivedMark.Commands
{
    public class MergeReceivedMarkCommand : IRequest<Result>
    {
        public List<ReceivedMarkPrintingModel> ReceivedMarkPrintings { get; set; }
    }

    public class MergeReceivedMarkCommandHandler : IRequestHandler<MergeReceivedMarkCommand, Result>
    {
        private readonly IShippingAppDbContext _context;

        public MergeReceivedMarkCommandHandler(IShippingAppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Result> Handle(MergeReceivedMarkCommand request, CancellationToken cancellationToken)
        {
            var quantityMerged = request.ReceivedMarkPrintings.Sum(x => x.Quantity);

            var firstReceivedMarkPrinting = request.ReceivedMarkPrintings.FirstOrDefault();

            var product = await _context.Products.FindAsync(firstReceivedMarkPrinting.ProductId);

            if (quantityMerged > product.QtyPerPackage)
            {
                return Result.Failure("Quantity of merged packages could not be greater than Quantity of Package");
            }

            var query = await _context.ReceivedMarkPrintings
                                                .Where(x => x.ProductId == firstReceivedMarkPrinting.ProductId)
                                                .Where(x => x.ReceivedMarkId == firstReceivedMarkPrinting.ReceivedMarkId)
                                                .Where(x => x.MovementRequestId == firstReceivedMarkPrinting.MovementRequestId)
                                                .Where(x => !x.Status.Equals(nameof(ReceivedMarkStatus.Unstuff)))
                                                .ToListAsync(cancellationToken);

            var receivedMarkPrintingsDelete = query
                                                .Where(x => request.ReceivedMarkPrintings.Any(i => i.Id == x.Id))
                                                .ToList();

            var receivedMarkPrintingsRemain =  query
                                                .Where(x => !request.ReceivedMarkPrintings.Any(i => i.Id == x.Id))
                                                .ToList();

            _context.ReceivedMarkPrintings.RemoveRange(receivedMarkPrintingsDelete);

            int sequence = 1;
            foreach (var item in receivedMarkPrintingsRemain)
            {
                item.Sequence = sequence;
                sequence++;
            }

            await _context.ReceivedMarkPrintings.AddAsync(new Entities.ReceivedMarkPrinting
            {
                Quantity = quantityMerged,
                ReceivedMarkId = firstReceivedMarkPrinting.ReceivedMarkId,
                ProductId = firstReceivedMarkPrinting.ProductId,
                Sequence = sequence,
                Status = nameof(ReceivedMarkStatus.New),
                MovementRequestId = firstReceivedMarkPrinting.MovementRequestId
            });

            return await  _context.SaveChangesAsync(cancellationToken) > 0 
                ? Result.Success() 
                : Result.Failure("Failed to merge received marks");
        }
    }
}
