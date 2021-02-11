using MediatR;
using ShippingApp.Application.Common.Results;
using ShippingApp.Application.Interfaces;
using System;
using Entities = ShippingApp.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace ShippingApp.Application.Config.Commands
{
    public class DeleteConfigCommand : IRequest<Result>
    {
        public int Id { get; set; }
    }

    public class DeleteConfigCommandHandler : IRequestHandler<DeleteConfigCommand, Result>
    {
        private readonly IShippingAppRepository<Entities.Config> _shippingAppRepository;

        public DeleteConfigCommandHandler(IShippingAppRepository<Entities.Config> shippingAppRepository)
        {
            _shippingAppRepository = shippingAppRepository ?? throw new ArgumentNullException(nameof(shippingAppRepository));
        }

        public async Task<Result> Handle(DeleteConfigCommand request, CancellationToken cancellationToken)
        {
            return await _shippingAppRepository.DeleteAsync(request.Id);
        }
    }
}
