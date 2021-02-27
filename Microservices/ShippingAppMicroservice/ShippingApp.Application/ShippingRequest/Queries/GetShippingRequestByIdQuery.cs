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

namespace ShippingApp.Application.ShippingRequest.Queries
{
    public class GetShippingRequestByIdQuery : IRequest<ShippingRequestModel>
    {
        public int Id { get; set; }
    }
    public class GetShippingRequestByIdQueryHandler : IRequestHandler<GetShippingRequestByIdQuery, ShippingRequestModel>
    {
        private readonly IMapper _mapper;
        private readonly IShippingAppRepository<Entities.ShippingRequest> _shippingAppRepository;

        public GetShippingRequestByIdQueryHandler(IMapper mapper, IShippingAppRepository<Entities.ShippingRequest> shippingAppRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _shippingAppRepository = shippingAppRepository ?? throw new ArgumentNullException(nameof(shippingAppRepository));
        }

        public async Task<ShippingRequestModel> Handle(GetShippingRequestByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _shippingAppRepository.GetDbSet()
                .Include(x => x.ShippingRequestDetails)
                .ThenInclude(x => x.Product)
                .FirstOrDefaultAsync(x => x.Id == request.Id);

            return _mapper.Map<ShippingRequestModel>(entity);
        }
    }
}
