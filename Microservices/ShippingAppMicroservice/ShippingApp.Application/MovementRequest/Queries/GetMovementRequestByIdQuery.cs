using ShippingApp.Application.Interfaces;
using System;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Entities = ShippingApp.Domain.Entities;
using ShippingApp.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ShippingApp.Application.MovementRequest.Queries
{
    public class GetMovementRequestByIdQuery : IRequest<MovementRequestModel>
    {
        public int Id { get; set; }
    }
    public class GetMovementRequestByIdQueryHandler : IRequestHandler<GetMovementRequestByIdQuery, MovementRequestModel>
    {
        private readonly IMapper _mapper;
        private readonly IShippingAppRepository<Entities.MovementRequest> _shippingAppRepository;
        private readonly IShippingAppDbContext _context;

        public GetMovementRequestByIdQueryHandler(IMapper mapper, 
            IShippingAppDbContext context,
            IShippingAppRepository<Entities.MovementRequest> shippingAppRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _shippingAppRepository = shippingAppRepository ?? throw new ArgumentNullException(nameof(shippingAppRepository));
        }

        public async Task<MovementRequestModel> Handle(GetMovementRequestByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _shippingAppRepository
               .GetDbSet()
               .Include(i => i.MovementRequestDetails)
               .ThenInclude(i => i.WorkOrder)
               .ThenInclude(i => i.Product)
               .FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

            var vm = _mapper.Map<MovementRequestModel>(entity);

            foreach (var item in vm.MovementRequestDetails)
            {
                item.WorkOrder.ReceviedMarkQuantity = await PopulateReceviedMarkQuantityAsync(item.WorkOrderId, cancellationToken);
            }

            return vm;
        }

        private async Task<int> PopulateReceviedMarkQuantityAsync(int wokrOrderId, CancellationToken cancellationToken)
        {
            var receivedMarkMovements = await _context.ReceivedMarkMovements
                .Where(x => x.WorkOrderId == wokrOrderId)
                .ToListAsync(cancellationToken);

            return receivedMarkMovements.Sum(x => x.Quantity);
        }
    }
}
