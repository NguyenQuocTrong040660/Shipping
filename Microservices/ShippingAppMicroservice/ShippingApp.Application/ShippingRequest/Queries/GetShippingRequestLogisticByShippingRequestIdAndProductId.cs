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
    public class GetShippingRequestLogisticByShippingRequestIdAndProductId : IRequest<ShippingRequestLogisticModel>
    {
        public int ShippingRequestId { get; set; }
        public int ProductId { get; set; }
    }

    public class GetShippingRequestLogisticByShippingRequestIdAndProductIdHandler : IRequestHandler<GetShippingRequestLogisticByShippingRequestIdAndProductId, ShippingRequestLogisticModel>
    {
        private readonly IShippingAppDbContext _context;
        private readonly IMapper _mapper;

        public GetShippingRequestLogisticByShippingRequestIdAndProductIdHandler(IShippingAppDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ShippingRequestLogisticModel> Handle(GetShippingRequestLogisticByShippingRequestIdAndProductId request, CancellationToken cancellationToken)
        {
            var entity = await _context.ShippingRequestLogistics
                .FirstOrDefaultAsync(x => x.ShippingRequestId == request.ShippingRequestId && x.ProductId == request.ProductId);
            return _mapper.Map<ShippingRequestLogisticModel>(entity);
        }
    }
}
