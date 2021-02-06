using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ShippingApp.Application.Common.Exceptions;
using ShippingApp.Domain.Interfaces;
using ShippingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Entities = ShippingApp.Domain.Entities;
using Models = ShippingApp.Domain.Models;
using DTO = ShippingApp.Domain.DTO;

namespace ShippingApp.Application.Commands
{
    public class UpdateShippingPlanCommand : IRequest<int>
    {
        public DTO.ShippingPlan shippingPlan { get; set; }
    }

    public class UpdateShippingPlanCommandHandler : IRequestHandler<UpdateShippingPlanCommand, int>
    {
        private readonly IShippingAppRepository _shippingAppRepository;
        private readonly IMapper _mapper;

        public UpdateShippingPlanCommandHandler(IShippingAppRepository shippingAppRepository, IMapper mapper)
        {
            _shippingAppRepository = shippingAppRepository;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<int> Handle(UpdateShippingPlanCommand request, CancellationToken cancellationToken)
        {
            var shippingPlan = _mapper.Map<Models.ShippingPlan>(request.shippingPlan);

            return await _shippingAppRepository.UpdateShippingPlan(shippingPlan);
        }
    }


}
