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

namespace ShippingApp.Application.ShippingRequest.Queries
{
    public class GetShippingRequestLogisticByShippingRequestId : IRequest<ShippingRequestLogisticModel>
    {
        public int ShippingRequestId { get; set; }
    }

    public class GetShippingRequestLogisticByShippingRequestIdHandler : IRequestHandler<GetShippingRequestLogisticByShippingRequestId, ShippingRequestLogisticModel>
    {
        private readonly IShippingAppDbContext _context;
        private readonly IMapper _mapper;

        public GetShippingRequestLogisticByShippingRequestIdHandler(IShippingAppDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ShippingRequestLogisticModel> Handle(GetShippingRequestLogisticByShippingRequestId request, CancellationToken cancellationToken)
        {
            var entity = await _context.ShippingRequestLogistics.FirstOrDefaultAsync(x => x.ShippingRequestId == request.ShippingRequestId);
            return _mapper.Map<ShippingRequestLogisticModel>(entity);
        }
    }
}
