using ShippingApp.Application.Interfaces;
using System;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ShippingApp.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace ShippingApp.Application.ReceivedMark.Queries
{
    public class GetReceivedMarkByIdQuery : IRequest<ReceivedMarkModel>
    {
        public int Id { get; set; }
    }
    public class GetReceiveMarkByIdQueryHandler : IRequestHandler<GetReceivedMarkByIdQuery, ReceivedMarkModel>
    {
        private readonly IShippingAppDbContext _context;
        private readonly IMapper _mapper;

        public GetReceiveMarkByIdQueryHandler(IShippingAppDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ReceivedMarkModel> Handle(GetReceivedMarkByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.ReceivedMarks
                 .AsNoTracking()
                 .Include(x => x.ReceivedMarkMovements)
                 .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            foreach (var item in entity.ReceivedMarkMovements)
            {
                item.Product = await _context.Products
                    .FindAsync(item.ProductId);

                item.MovementRequest = await _context.MovementRequests
                    .FindAsync(item.MovementRequestId);
            }

            return _mapper.Map<ReceivedMarkModel>(entity);
        }
    }
}
