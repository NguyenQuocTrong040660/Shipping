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
using System.Collections.Generic;
using ShippingApp.Application.ShippingMark.Commands;

namespace ShippingApp.Application.ShippingMark.Queries
{
    public class GetShippingMarkByShippingRequestIdQuery : IRequest<List<ShippingMarkModel>>
    {
        public int ShippingRequestId { get; set; }
    }

    public class GetShippingMarkByShippingRequestIdQueryHandler : IRequestHandler<GetShippingMarkByShippingRequestIdQuery, List<ShippingMarkModel>>
    {
        private readonly IMapper _mapper;
        private readonly IShippingAppRepository<Entities.ShippingMark> _shippingAppRepository;
        private readonly IMediator _mediator;

        public GetShippingMarkByShippingRequestIdQueryHandler(IMapper mapper,
            IMediator mediator,
            IShippingAppRepository<Entities.ShippingMark> shippingAppRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _shippingAppRepository = shippingAppRepository ?? throw new ArgumentNullException(nameof(shippingAppRepository));
        }

        public async Task<List<ShippingMarkModel>> Handle(GetShippingMarkByShippingRequestIdQuery request, CancellationToken cancellationToken)
        {
            var shippingMarks = await _shippingAppRepository.GetDbSet()
                 .Include(x => x.Product)
                 .Where(x => x.ShippingRequestId == request.ShippingRequestId)
                 .ToListAsync();

            if (shippingMarks.Count == 0)
            {
                var generatedShippingMarks = await _mediator.Send(new GenerateShippingMarkCommand { ShippingRequestId = request.ShippingRequestId });
                return _mapper.Map<List<ShippingMarkModel>>(generatedShippingMarks);
            }

            return _mapper.Map<List<ShippingMarkModel>>(shippingMarks);
        }
    }
}
