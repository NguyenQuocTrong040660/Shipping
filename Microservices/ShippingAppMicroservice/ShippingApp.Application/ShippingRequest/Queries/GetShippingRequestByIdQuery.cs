using ShippingApp.Application.Interfaces;
using System;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly IShippingAppDbContext _context;

        public GetShippingRequestByIdQueryHandler(IMapper mapper,
            IShippingAppDbContext context)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<ShippingRequestModel> Handle(GetShippingRequestByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.ShippingRequests
                .Include(x => x.ShippingPlans)
                .ThenInclude(x => x.Product)
                .FirstOrDefaultAsync(x => x.Id == request.Id);

            return _mapper.Map<ShippingRequestModel>(entity);
        }
    }
}
