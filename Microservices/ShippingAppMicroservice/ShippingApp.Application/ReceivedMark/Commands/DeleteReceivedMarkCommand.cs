using MediatR;
using ShippingApp.Application.Common.Results;
using ShippingApp.Application.Interfaces;
using System;
using Entities = ShippingApp.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace ShippingApp.Application.ReceivedMark.Commands
{
    public class DeleteReceivedMarkCommand : IRequest<Result>
    {
        public int Id { get; set; }
    }

    public class DeleteReceiveMarkCommandHandler : IRequestHandler<DeleteReceivedMarkCommand, Result>
    {
        private readonly IShippingAppRepository<Entities.ReceivedMark> _shippingAppRepository;

        public DeleteReceiveMarkCommandHandler(IShippingAppRepository<Entities.ReceivedMark> shippingAppRepository)
        {
            _shippingAppRepository = shippingAppRepository ?? throw new ArgumentNullException(nameof(shippingAppRepository));
        }

        public async Task<Result> Handle(DeleteReceivedMarkCommand request, CancellationToken cancellationToken)
        {
            return await _shippingAppRepository.DeleteAsync(request.Id);
        }
    }
}
