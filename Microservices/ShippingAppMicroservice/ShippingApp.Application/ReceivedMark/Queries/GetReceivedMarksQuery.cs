using ShippingApp.Application.Interfaces;
using System;
using System.Collections.Generic;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Entities = ShippingApp.Domain.Entities;
using AutoMapper;
using ShippingApp.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ShippingApp.Application.ReceivedMark.Queries
{
    public class GetReceivedMarksQuery : IRequest<List<ReceivedMarkModel>>
    {
    }

    public class GetReceiveMarksQueryHandler : IRequestHandler<GetReceivedMarksQuery, List<ReceivedMarkModel>>
    {
        private readonly IMapper _mapper;
        private readonly IShippingAppDbContext _context;
        private readonly IShippingAppRepository<Entities.ReceivedMark> _shippingAppRepository;

        public GetReceiveMarksQueryHandler(IMapper mapper, IShippingAppDbContext context, IShippingAppRepository<Entities.ReceivedMark> shippingAppRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _shippingAppRepository = shippingAppRepository ?? throw new ArgumentNullException(nameof(shippingAppRepository));
        }

        public async Task<List<ReceivedMarkModel>> Handle(GetReceivedMarksQuery request, CancellationToken cancellationToken)
        {
            var receivedMarks = _mapper.Map<List<ReceivedMarkModel>>(await _shippingAppRepository
                .GetDbSet()
                .AsNoTracking()
                .OrderByDescending(x => x.LastModified)
                .ToListAsync());

            foreach (var item in receivedMarks)
            {
                var products = item.ReceivedMarkMovements.Select(x => x.ProductId);
                var workOrders = await _context.WorkOrders
                    .AsNoTracking()
                    .Include(x => x.WorkOrderDetails)
                    .Where(x => products.Contains(x.WorkOrderDetails.First().ProductId))
                    .ToListAsync();

                item.WorkOrdersCollection = $"[{string.Join(",", workOrders.Select(x => x.RefId))}]";
            }

            return receivedMarks;
        }
    }
}
