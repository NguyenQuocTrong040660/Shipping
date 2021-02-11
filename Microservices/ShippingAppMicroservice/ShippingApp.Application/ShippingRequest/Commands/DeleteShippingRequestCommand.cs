using MediatR;
using ShippingApp.Application.Common.Results;
using ShippingApp.Application.Interfaces;
using System;
using Entities = ShippingApp.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace ShippingApp.Application.ShippingRequest.Commands
{
    public class DeleteShippingRequestCommand : IRequest<Result>
    {
        public int Id { get; set; }
    }

    public class DeleteShippingRequestCommandHandler : IRequestHandler<DeleteShippingRequestCommand, Result>
    {
        private readonly IShippingAppRepository<Entities.ShippingRequest> _shippingAppRepository;

        public DeleteShippingRequestCommandHandler(IShippingAppRepository<Entities.ShippingRequest> shippingAppRepository)
        {
            _shippingAppRepository = shippingAppRepository ?? throw new ArgumentNullException(nameof(shippingAppRepository));
        }

        public async Task<Result> Handle(DeleteShippingRequestCommand request, CancellationToken cancellationToken)
        {
            return await _shippingAppRepository.DeleteAsync(request.Id);
        }
    }
}
