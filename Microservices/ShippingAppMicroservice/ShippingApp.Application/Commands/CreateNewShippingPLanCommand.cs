using AutoMapper;
using MediatR;
using ShippingApp.Domain.Interfaces;
using ShippingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Models = ShippingApp.Domain.Models;
using DTO = ShippingApp.Domain.DTO;
using Entities = ShippingApp.Domain.Entities;

namespace ShippingApp.Application.Commands
{
    public class CreateNewShippingPLanCommand : IRequest<int>
    {
        public DTO.ShippingPlan shippingPlan { get; set; }
    }

    public class CreateNewShippingPLanCommandHandler : IRequestHandler<CreateNewShippingPLanCommand, int>
    {
        private readonly IShippingAppRepository _shippingAppRepository;
        private readonly IMapper _mapper;

        public CreateNewShippingPLanCommandHandler(IShippingAppRepository Repository, IMapper mapper)
        {
            _shippingAppRepository = Repository;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<int> Handle(CreateNewShippingPLanCommand request, CancellationToken cancellationToken)
        {
            var shippingPlan = _mapper.Map<Models.ShippingPlan>(request.shippingPlan);

            return await _shippingAppRepository.CreateNewShippingPlan(shippingPlan);
        }
    }
}
