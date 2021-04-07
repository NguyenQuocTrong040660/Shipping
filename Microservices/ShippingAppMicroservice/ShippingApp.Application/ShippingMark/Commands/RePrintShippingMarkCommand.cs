using AutoMapper;
using MediatR;
using ShippingApp.Application.Interfaces;
using ShippingApp.Domain.Models;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ShippingApp.Application.ShippingMark.Commands
{
    public class RePrintShippingMarkCommand : IRequest<ShippingMarkPrintingModel>
    {
        public RePrintShippingMarkRequest RePrintShippingMarkRequest { get; set; }
    }

    public class RePrintShippingMarkCommandHandler : IRequestHandler<RePrintShippingMarkCommand, ShippingMarkPrintingModel>
    {
        private readonly IShippingAppDbContext _context;
        private readonly IMapper _mapper;

        public RePrintShippingMarkCommandHandler(IShippingAppDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ShippingMarkPrintingModel> Handle(RePrintShippingMarkCommand request, CancellationToken cancellationToken)
        {
            var shippingMarkPrinting = await _context.ShippingMarkPrintings
                .Include(x => x.Product)
                .Include(x=> x.ShippingMark)
                .FirstOrDefaultAsync(x => x.Id == request.RePrintShippingMarkRequest.ShippingMarkPrintingId);

            if (shippingMarkPrinting == null)
            {
                return null;
            }

            shippingMarkPrinting.PrintCount += 1;
            shippingMarkPrinting.RePrintingBy = request.RePrintShippingMarkRequest.RePrintedBy;
            shippingMarkPrinting.RePrintingDate = DateTime.UtcNow;

            if (await _context.SaveChangesAsync() > 0)
            {
                var result = _mapper.Map<ShippingMarkPrintingModel>(shippingMarkPrinting);

                result.ShippingMarkShipping = _mapper.Map<ShippingMarkShippingModel>(await _context.ShippingMarkShippings
                    .Include(x => x.ShippingRequest)
                    .FirstOrDefaultAsync(x =>
                    x.ShippingMarkId == shippingMarkPrinting.ShippingMarkId &&
                    x.ProductId == shippingMarkPrinting.ProductId));

                return result;
            }

            return null;
        }
    }
}

