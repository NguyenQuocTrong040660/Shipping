using AutoMapper;
using MediatR;
using ShippingApp.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ShippingApp.Application.Commands
{
    public class DeleteShippingPlanCommand : IRequest<int>
    {
     public Guid Id { get; set; }
    }

    public class DeleteShippingPlanCommandHandler : IRequestHandler<DeleteShippingPlanCommand, int>
    {
        private readonly IShippingAppRepository _shippingAppRepository;
        private readonly IMapper _mapper;

        public DeleteShippingPlanCommandHandler(IShippingAppRepository Repository, IMapper mapper)
        {
            _shippingAppRepository = Repository;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<int> Handle(DeleteShippingPlanCommand request, CancellationToken cancellationToken)
        {
            return await _shippingAppRepository.DeleteShippingPlanByID(request.Id);
        }
    }
}
