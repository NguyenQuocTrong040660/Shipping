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

namespace ShippingApp.Application.ShippingMark.Queries
{
    public class GetShippingMarksQuery : IRequest<List<ShippingMarkModel>>
    {
    }

    public class GetShippingMarksQueryHandler : IRequestHandler<GetShippingMarksQuery, List<ShippingMarkModel>>
    {
        private readonly IMapper _mapper;
        private readonly IShippingAppRepository<Entities.ShippingMark> _shippingAppRepository;

        public GetShippingMarksQueryHandler(IMapper mapper, IShippingAppRepository<Entities.ShippingMark> shippingAppRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _shippingAppRepository = shippingAppRepository ?? throw new ArgumentNullException(nameof(shippingAppRepository));
        }

        public async Task<List<ShippingMarkModel>> Handle(GetShippingMarksQuery request, CancellationToken cancellationToken)
        {
            var shippingMarks = await _shippingAppRepository.GetDbSet().AsNoTracking().ToListAsync();
            return _mapper.Map<List<ShippingMarkModel>>(shippingMarks);
        }
    }
}
