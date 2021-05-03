using ShippingApp.Application.Interfaces;
using System;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Entities = ShippingApp.Domain.Entities;
using ShippingApp.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace ShippingApp.Application.MovementRequest.Queries
{
    public class GetMovementRequestByIdWithoutWorkOderQuery : IRequest<MovementRequestModel>
    {
        public int Id { get; set; }
    }

    public class GetMovementRequestByIdWithoutWorkOderQueryHandler : IRequestHandler<GetMovementRequestByIdWithoutWorkOderQuery, MovementRequestModel>
    {
        private readonly IMapper _mapper;
        private readonly IShippingAppRepository<Entities.MovementRequest> _shippingAppRepository;

        public GetMovementRequestByIdWithoutWorkOderQueryHandler(IMapper mapper, IShippingAppRepository<Entities.MovementRequest> shippingAppRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _shippingAppRepository = shippingAppRepository ?? throw new ArgumentNullException(nameof(shippingAppRepository));
        }

        public async Task<MovementRequestModel> Handle(GetMovementRequestByIdWithoutWorkOderQuery request, CancellationToken cancellationToken)
        {
            var entity = await _shippingAppRepository
               .GetDbSet()
               .Include(i => i.MovementRequestDetails)
               .ThenInclude(i => i.Product)
               .FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

            return _mapper.Map<MovementRequestModel>(entity);
        }
    }
}
