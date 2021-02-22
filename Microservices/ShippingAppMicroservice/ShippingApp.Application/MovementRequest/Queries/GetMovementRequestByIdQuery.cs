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
        public int Id;
    }
    public class GetMovementRequestByIdQueryHandler : IRequestHandler<GetMovementRequestByIdQuery, MovementRequestModel>
    {
        private readonly IMapper _mapper;
        private readonly IShippingAppRepository<Entities.MovementRequest> _shippingAppRepository;

        public GetMovementRequestByIdQueryHandler(IMapper mapper, IShippingAppRepository<Entities.MovementRequest> shippingAppRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _shippingAppRepository = shippingAppRepository ?? throw new ArgumentNullException(nameof(shippingAppRepository));
        }

        public async Task<MovementRequestModel> Handle(GetMovementRequestByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _shippingAppRepository
               .GetDbSet()
               .Include(i => i.MovementRequestDetails)
               .ThenInclude(i => i.WorkOrder)
               .ThenInclude(i => i.WorkOrderDetails)
               .ThenInclude(i => i.Product)
               .OrderByDescending(i => i.LastModified)
               .FirstOrDefaultAsync(i => i.Id == request.Id);

            return _mapper.Map<MovementRequestModel>(entity);
        }
    }
}
