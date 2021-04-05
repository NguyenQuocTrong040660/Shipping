using ShippingApp.Application.Interfaces;
using System;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ShippingApp.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using ShippingApp.Domain.Enumerations;

namespace ShippingApp.Application.ReceivedMark.Queries
{
    public class GetReceivedMarkMovementsFullInfoByIdQuery : IRequest<List<ReceivedMarkMovementModel>>
    {
        public int ReceivedMarkId { get; set; }
    }
    public class GetReceivedMarkMovementsFullInfoByIdQueryHandler : IRequestHandler<GetReceivedMarkMovementsFullInfoByIdQuery, List<ReceivedMarkMovementModel>>
    {
        private readonly IShippingAppDbContext _context;
        private readonly IMapper _mapper;

        public GetReceivedMarkMovementsFullInfoByIdQueryHandler(IShippingAppDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<List<ReceivedMarkMovementModel>> Handle(GetReceivedMarkMovementsFullInfoByIdQuery request, CancellationToken cancellationToken)
        {
            var receivedMarkMovements = _mapper.Map<List<ReceivedMarkMovementModel>>(await _context.ReceivedMarkMovements
                .AsNoTracking()
                .Include(x => x.Product)
                .Where(x => x.ReceivedMarkId == request.ReceivedMarkId)
                .ToListAsync());

            foreach (var item in receivedMarkMovements)
            {
                var receivedMarkPrintings = await _context.ReceivedMarkPrintings
                  .AsNoTracking()
                  .Where(x => x.ProductId == item.ProductId)
                  .Where(x => x.MovementRequestId == item.MovementRequestId)
                  .Where(x => x.ReceivedMarkId == item.ReceivedMarkId)
                  .Where(x => !x.Status.Equals(nameof(ReceivedMarkStatus.Unstuff)))
                  .ToListAsync();

                item.TotalPackage = receivedMarkPrintings.Count;
                item.TotalQuantityPrinted = receivedMarkPrintings
                    .Where(x => x.Status.Equals(nameof(ReceivedMarkStatus.Storage)))
                    .Sum(x => x.Quantity);
            }

            return receivedMarkMovements;
        }
    }
}
