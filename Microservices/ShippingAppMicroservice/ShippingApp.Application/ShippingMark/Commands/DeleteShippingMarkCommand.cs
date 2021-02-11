using MediatR;
using ShippingApp.Application.Common.Results;
using ShippingApp.Application.Interfaces;
using System;
using Entities = ShippingApp.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace ShippingApp.Application.ShippingMark.Commands
{
    public class DeleteShippingMarkCommand : IRequest<Result>
    {
        public int Id { get; set; }
    }

    public class DeleteShippingMarkCommandHandler : IRequestHandler<DeleteShippingMarkCommand, Result>
    {
        private readonly IShippingAppRepository<Entities.ShippingMark> _shippingAppRepository;

        public DeleteShippingMarkCommandHandler(IShippingAppRepository<Entities.ShippingMark> shippingAppRepository)
        {
            _shippingAppRepository = shippingAppRepository ?? throw new ArgumentNullException(nameof(shippingAppRepository));
        }

        public async Task<Result> Handle(DeleteShippingMarkCommand request, CancellationToken cancellationToken)
        {
            return await _shippingAppRepository.DeleteAsync(request.Id);
        }
    }
}
