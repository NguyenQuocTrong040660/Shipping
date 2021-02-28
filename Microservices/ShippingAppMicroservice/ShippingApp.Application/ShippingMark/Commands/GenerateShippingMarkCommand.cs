using AutoMapper;
using MediatR;
using ShippingApp.Application.Interfaces;
using ShippingApp.Domain.Models;
using System;
using System.Threading;
using System.Threading.Tasks;
using Entities = ShippingApp.Domain.Entities;
using ShippingApp.Application.Common.Results;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using ShippingApp.Domain.Enumerations;

namespace ShippingApp.Application.ShippingMark.Commands
{
    public class GenerateShippingMarkCommand : IRequest<List<ShippingMarkModel>>
    {
        public int ShippingRequestId { get; set; }
    }

    public class GenerateShippingMarkCommandHandler : IRequestHandler<GenerateShippingMarkCommand, List<ShippingMarkModel>>
    {
        private readonly IMapper _mapper;
        private readonly IShippingAppDbContext _context;

        public GenerateShippingMarkCommandHandler(IMapper mapper,
            IShippingAppDbContext context)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<ShippingMarkModel>> Handle(GenerateShippingMarkCommand request, CancellationToken cancellationToken)
        {
            var shippingRequest = await _context.ShippingRequests
               .Include(x => x.ShippingRequestDetails)
               .ThenInclude(x => x.Product)
               .ThenInclude(x => x.ShippingMarks)
               .FirstOrDefaultAsync(i => i.Id == request.ShippingRequestId);

            if (shippingRequest == null)
            {
                throw new ArgumentNullException(nameof(shippingRequest));
            }

            if (shippingRequest.ShippingMarks != null && shippingRequest.ShippingMarks.Any())
            {
                return new List<ShippingMarkModel>();
            }

            var getReceivedMarksQuery = _context.ReceivedMarkPrintings;
                //.Where(x => x.Status.Equals(nameof(ReceiveMarkStatus.Storage)));

            var groupProducts = shippingRequest.ShippingRequestDetails
                .GroupBy(i => i.Product)
                .Select(x => new
                {
                    Product = x.Key,
                    TotalQuantity = x.ToList().Sum(i => i.Quantity),
                    ReceivedMarks = getReceivedMarksQuery.Where(i => i.ProductId == x.Key.Id)
                }).ToList();

            var shippingMarks = new List<Entities.ShippingMark>();

            foreach (var group in groupProducts)
            {
                int remainQty = group.TotalQuantity;
                var product = group.Product;
                var receivedMarks = group.ReceivedMarks;
                int sequence = 1;

                while(remainQty >= product.QtyPerPackage && receivedMarks.Any())
                {
                    var receivedMark = receivedMarks.FirstOrDefault();

                    shippingMarks.Add(new Entities.ShippingMark
                    {
                        ProductId = receivedMark.ProductId,
                        Quantity = receivedMark.Quantity,
                        Sequence = sequence,
                        ShippingRequestId = shippingRequest.Id,
                        Status = nameof(ShippingMarkStatus.New),
                        Revision = string.Empty,
                    });

                    //receivedMark.Status = nameof(ReceiveMarkStatus.Shipping);

                    remainQty -= product.QtyPerPackage;
                    sequence++;
                }
            }

            await _context.ShippingMarks.AddRangeAsync(shippingMarks);

            return await _context.SaveChangesAsync() > 0 
                ? _mapper.Map<List<ShippingMarkModel>>(shippingMarks) 
                : new List<ShippingMarkModel>();
        }
    }
}
