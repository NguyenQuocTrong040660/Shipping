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

namespace ShippingApp.Application.ShippingMark.Queries
{
    public class GetShippingMarkByIdQuery : IRequest<ShippingMarkModel>
    {
        public int Id { get; set; }
    }
    public class GetShippingMarkByIdQueryHandler : IRequestHandler<GetShippingMarkByIdQuery, ShippingMarkModel>
    {
        private readonly IMapper _mapper;
        private readonly IShippingAppRepository<Entities.ShippingMark> _shippingAppRepository;

        public GetShippingMarkByIdQueryHandler(IMapper mapper, IShippingAppRepository<Entities.ShippingMark> shippingAppRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _shippingAppRepository = shippingAppRepository ?? throw new ArgumentNullException(nameof(shippingAppRepository));
        }

        public async Task<ShippingMarkModel> Handle(GetShippingMarkByIdQuery request, CancellationToken cancellationToken)
        {
            var shippingMarks = await _shippingAppRepository.GetDbSet().ToListAsync();

            var entity = shippingMarks.FirstOrDefault(x => x.Id == request.Id);
            return _mapper.Map<ShippingMarkModel>(entity);
        }
    }
}
