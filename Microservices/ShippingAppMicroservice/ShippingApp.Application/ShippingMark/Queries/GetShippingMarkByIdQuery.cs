using ShippingApp.Application.Interfaces;
using System;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ShippingApp.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using ShippingApp.Application.ReceivedMark.Queries;

namespace ShippingApp.Application.ShippingMark.Queries
{
    public class GetShippingMarkByIdQuery : IRequest<ShippingMarkModel>
    {
        public int Id { get; set; }
    }
    public class GetShippingMarkByIdQueryHandler : IRequestHandler<GetShippingMarkByIdQuery, ShippingMarkModel>
    {
        private readonly IShippingAppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public GetShippingMarkByIdQueryHandler(IShippingAppDbContext context, IMapper mapper, IMediator mediator)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<ShippingMarkModel> Handle(GetShippingMarkByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.ShippingMarks
                .AsNoTracking()
                .Include(x => x.ShippingMarkShippings)
                .ThenInclude(x => x.Product)
                .FirstOrDefaultAsync(x => x.Id == request.Id);

            entity.ReceivedMarkPrintings = await _context.ReceivedMarkPrintings
                                                .AsNoTracking()
                                                .Where(x => x.ShippingMarkId == request.Id)
                                                .ToListAsync();

            var model = _mapper.Map<ShippingMarkModel>(entity);

            foreach (var item in model.ShippingMarkShippings)
            {
                item.Product.ReceivedMarkPrintings = await _mediator.Send(new GetReceivedMarkPrintingsByProductIdQuery
                {
                    ProductId = item.ProductId
                });

                item.ShippingRequest = _mapper.Map<ShippingRequestModel>(
                    await _context.ShippingRequests.FindAsync(item.ShippingRequestId));
            }

            return model;
        }
    }
}
