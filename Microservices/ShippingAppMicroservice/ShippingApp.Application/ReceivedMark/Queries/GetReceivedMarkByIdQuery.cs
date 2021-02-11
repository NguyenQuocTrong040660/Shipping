using ShippingApp.Application.Interfaces;
using System;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Entities = ShippingApp.Domain.Entities;
using ShippingApp.Domain.Models;

namespace ShippingApp.Application.ReceivedMark.Queries
{
    public class GetReceivedMarkByIdQuery : IRequest<ReceivedMarkModel>
    {
        public int Id { get; set; }
    }
    public class GetReceiveMarkByIdQueryHandler : IRequestHandler<GetReceivedMarkByIdQuery, ReceivedMarkModel>
    {
        private readonly IMapper _mapper;
        private readonly IShippingAppRepository<Entities.ReceivedMark> _shippingAppRepository;

        public GetReceiveMarkByIdQueryHandler(IMapper mapper, IShippingAppRepository<Entities.ReceivedMark> shippingAppRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _shippingAppRepository = shippingAppRepository ?? throw new ArgumentNullException(nameof(shippingAppRepository));
        }

        public async Task<ReceivedMarkModel> Handle(GetReceivedMarkByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _shippingAppRepository.GetAsync(request.Id);
            return _mapper.Map<ReceivedMarkModel>(entity);
        }
    }
}
