using AutoMapper;
using MediatR;
using ShippingApp.Application.Interfaces;
using ShippingApp.Domain.Models;
using System;
using System.Threading;
using System.Threading.Tasks;
using ShippingApp.Application.Common.Results;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Entities = ShippingApp.Domain.Entities;

namespace ShippingApp.Application.ShippingPlan.Commands
{
    public class UpdateShippingPlanCommand : IRequest<Result>
    {
        public int Id { get; set; }
        public ShippingPlanModel ShippingPlan { get; set; }
    }

    public class UpdateShippingPlanCommandHandler : IRequestHandler<UpdateShippingPlanCommand, Result>
    {
        private readonly IShippingAppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IShippingAppRepository<Entities.ShippingPlan> _shippingAppRepository;
       
        public UpdateShippingPlanCommandHandler(IShippingAppDbContext context, IMapper mapper, IShippingAppRepository<Entities.ShippingPlan> shippingAppRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _shippingAppRepository = shippingAppRepository ?? throw new ArgumentNullException(nameof(shippingAppRepository));
        }

        public async Task<Result> Handle(UpdateShippingPlanCommand request, CancellationToken cancellationToken)
        {
            var shippingPlan = _mapper.Map<Entities.ShippingPlan>(request.ShippingPlan);
            await _shippingAppRepository.Update(request.Id, shippingPlan);
            return Result.Success();
        }
    }
}
