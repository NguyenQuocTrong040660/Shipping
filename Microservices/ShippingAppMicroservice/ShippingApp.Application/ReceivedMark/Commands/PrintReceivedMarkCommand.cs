using AutoMapper;
using MediatR;
using ShippingApp.Application.Common.Results;
using ShippingApp.Application.Interfaces;
using ShippingApp.Domain.Models;
using System;
using System.Threading;
using System.Threading.Tasks;
using ShippingApp.Domain.Enumerations;

namespace ShippingApp.Application.ReceivedMark.Commands
{
    public class PrintReceivedMarkCommand : IRequest<Result>
    {
        public int ReceivedMarkPrintingId { get; set; }
    }

    public class PrintReceivedMarkCommandHandler : IRequestHandler<PrintReceivedMarkCommand, Result>
    {
        private readonly IShippingAppDbContext _context;

        public PrintReceivedMarkCommandHandler(IShippingAppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Result> Handle(PrintReceivedMarkCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.ReceivedMarkPrintings.FindAsync(request.ReceivedMarkPrintingId);

            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (!entity.Status.Equals(nameof(ReceivedMarkStatus.New)))
            {
                return Result.Failure("Could not print mark");
            }

            if (entity.PrintCount != 0)
            {
                return Result.Failure("This mark has been printed. Please contact your manager to re-print");
            }

            entity.PrintCount += 1;

            return await _context.SaveChangesAsync() > 0
                ? Result.Success()
                : Result.Failure("Failed to print mark");
        }
    }
}
