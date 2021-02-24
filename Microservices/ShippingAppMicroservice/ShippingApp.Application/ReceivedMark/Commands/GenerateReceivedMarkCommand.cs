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

namespace ShippingApp.Application.ReceivedMark.Commands
{
    public class GenerateReceivedMarkCommand : IRequest<List<ReceivedMarkModel>>
    {
        public int MovementRequestId { get; set; }
    }

    public class GenerateReceivedMarkCommandHandler : IRequestHandler<GenerateReceivedMarkCommand, List<ReceivedMarkModel>>
    {
        private readonly IMapper _mapper;
        private readonly IShippingAppRepository<Entities.ReceivedMark> _shippingAppRepository;
        private readonly IShippingAppDbContext _context;

        public GenerateReceivedMarkCommandHandler(IMapper mapper,
            IShippingAppDbContext context,
            IShippingAppRepository<Entities.ReceivedMark> shippingAppRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _shippingAppRepository = shippingAppRepository ?? throw new ArgumentNullException(nameof(shippingAppRepository));
        }

        public async Task<List<ReceivedMarkModel>> Handle(GenerateReceivedMarkCommand request, CancellationToken cancellationToken)
        {
            var movementRequest = await _context.MovementRequests
                .Include(x => x.MovementRequestDetails)
                .ThenInclude(x => x.Product)
                .Include(x => x.ReceivedMarks)
                .FirstOrDefaultAsync(i => i.Id == request.MovementRequestId);

            if (movementRequest == null)
            {
                return new List<ReceivedMarkModel>();
            }

            if (movementRequest.ReceivedMarks.Any())
            {
                return new List<ReceivedMarkModel>();
            }

            var groupByProduct = movementRequest.MovementRequestDetails
                .GroupBy(i => i.Product)
                .Select(x => new
                {
                    Product = x.Key,
                    ReceivedQty = x.ToList().Sum(i => i.Quantity),
                }).ToList();

            var receivedMark = new List<Entities.ReceivedMark>();

            foreach (var group in groupByProduct)
            {
                int remainQty = group.ReceivedQty;
                var product = group.Product;
                int sequence = 1;

                while(remainQty >= product.QtyPerPackage)
                {
                    receivedMark.Add(new Entities.ReceivedMark
                    {
                        MovementRequestId = request.MovementRequestId,
                        Notes = string.Empty,
                        ProductId = product.Id,
                        Quantity = product.QtyPerPackage,
                        Sequence = sequence,
                        Status = nameof(ReceiveMarkStatus.Storage),
                    });

                    remainQty -= product.QtyPerPackage;
                    sequence++;
                }
            }

            _context.ReceivedMarks.AddRange(receivedMark);
            ;

            return _context.SaveChanges() > 0 ? _mapper.Map<List<ReceivedMarkModel>>(receivedMark) : new List<ReceivedMarkModel>();
        }
    }
}
