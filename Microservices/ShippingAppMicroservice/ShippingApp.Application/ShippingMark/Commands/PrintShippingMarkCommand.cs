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
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ShippingApp.Application.ShippingMark.Commands
{
    public class PrintShippingMarkCommand : IRequest<ShippingMarkPrintingModel>
    {
        public PrintShippingMarkRequest PrintShippingMarkRequest { get; set; }
    }

    public class PrintShippingMarkCommandHandler : IRequestHandler<PrintShippingMarkCommand, ShippingMarkPrintingModel>
    {
        private readonly IShippingAppDbContext _context;
        private readonly IMapper _mapper;

        public PrintShippingMarkCommandHandler(IShippingAppDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ShippingMarkPrintingModel> Handle(PrintShippingMarkCommand request, CancellationToken cancellationToken)
        {
            var shippingMarkShippings = await _context.ShippingMarkPrintings
                .Include(x => x.Product)
                .Where(x => x.ShippingMarkId == request.PrintShippingMarkRequest.ShippingMarkId
                         && x.ProductId == request.PrintShippingMarkRequest.ProductId)
                .Where(x => x.Status.Equals(nameof(ReceivedMarkStatus.New)))
                .OrderBy(x => x.Sequence)
                .ToListAsync();

            if (shippingMarkShippings == null || !shippingMarkShippings.Any())
            {
                return null;
            }

            Entities.ShippingMarkPrinting printItem = null;

            for (int i = 0; i < shippingMarkShippings.Count; i++)
            {
                var itemPrint = shippingMarkShippings[i];

                if (itemPrint.PrintCount != 0)
                {
                    continue;
                }

                itemPrint.PrintCount += 1;
                itemPrint.Status = nameof(ShippingMarkStatus.Storage);
                itemPrint.PrintingBy = request.PrintShippingMarkRequest.PrintedBy;
                itemPrint.PrintingDate = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                printItem = itemPrint;
                break;
            }

            return _mapper.Map<ShippingMarkPrintingModel>(printItem);
        }
    }
}
