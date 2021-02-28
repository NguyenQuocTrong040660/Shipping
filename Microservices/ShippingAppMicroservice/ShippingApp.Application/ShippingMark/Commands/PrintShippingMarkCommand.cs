using AutoMapper;
using MediatR;
using ShippingApp.Application.Common.Results;
using ShippingApp.Application.Interfaces;
using ShippingApp.Domain.Models;
using System;
using Entities = ShippingApp.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;
using ShippingApp.Domain.Enumerations;

namespace ShippingApp.Application.ShippingMark.Commands
{
    public class PrintShippingMarkCommand : IRequest<Result>
    {
        public int Id { get; set; }
    }

    public class PrintShippingMarkCommandHandler : IRequestHandler<PrintShippingMarkCommand, Result>
    {
        private readonly IShippingAppDbContext _context;

        public PrintShippingMarkCommandHandler(IShippingAppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Result> Handle(PrintShippingMarkCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.ShippingMarks.FindAsync(request.Id);

            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (!entity.Status.Equals(nameof(ShippingMarkStatus.New)))
            {
                return Result.Failure("Could not print Shipping Mark");
            }

            if (entity.PrintCount != 0)
            {
                return Result.Failure("Shipping Mark has already been printed. Please contact your manager to re-print");
            }

            entity.PrintCount += 1;

            return await _context.SaveChangesAsync() > 0
                ? Result.Success()
                : Result.Failure("Failed to print Shipping Mark");
        }
    }
}
