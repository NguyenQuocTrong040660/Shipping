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
                .FirstOrDefaultAsync(x => x.Id == request.Id);

            entity.ShippingRequestDetails = await _context.ShippingRequestDetails
                .AsNoTracking()
                .Include(x => x.Product)
                .Where(i => i.ShippingRequestId == entity.Id)
                .ToListAsync();

            return _mapper.Map<ShippingRequestModel>(entity);
        }
    }
}
