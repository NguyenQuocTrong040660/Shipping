using MediatR;
using ShippingApp.Application.Common.Results;
using ShippingApp.Application.Interfaces;
using System;
using Entities = ShippingApp.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace ShippingApp.Application.MovementRequest.Commands
{
    public class DeleteMovementRequestCommand : IRequest<Result>
    {
        public int Id { get; set; }
    }

    public class DeleteMovementRequestCommandHandler : IRequestHandler<DeleteMovementRequestCommand, Result>
    {
        private readonly IShippingAppRepository<Entities.MovementRequest> _shippingAppRepository;
        private readonly IShippingAppDbContext _context;

        public DeleteMovementRequestCommandHandler(IShippingAppRepository<Entities.MovementRequest> shippingAppRepository,
           IShippingAppDbContext context)
        {
            _shippingAppRepository = shippingAppRepository ?? throw new ArgumentNullException(nameof(shippingAppRepository));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Result> Handle(DeleteMovementRequestCommand request, CancellationToken cancellationToken)
        {
            return await _shippingAppRepository.DeleteAsync(request.Id);
        }
    }
}
