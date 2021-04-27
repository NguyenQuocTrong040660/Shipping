using AutoMapper;
using MediatR;
using ShippingApp.Application.Interfaces;
using ShippingApp.Domain.Models;
using System;
using System.Threading;
using System.Threading.Tasks;
using Entities = ShippingApp.Domain.Entities;
using ShippingApp.Application.Common.Results;
using ShippingApp.Application.Config.Queries;
using ShippingApp.Domain.Enumerations;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ShippingApp.Application.ShippingPlan.Commands
{
    public class CreateNewShippingPLanCommand : IRequest<Result>
    {
        public ShippingPlanModel ShippingPlan { get; set; }
    }

    public class CreateNewShippingPLanCommandHandler : IRequestHandler<CreateNewShippingPLanCommand, Result>
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IShippingAppRepository<Entities.ShippingPlan> _shippingAppRepository;
        private readonly IShippingAppDbContext _context;

        public CreateNewShippingPLanCommandHandler(IMapper mapper, 
            IMediator mediator,
            IShippingAppDbContext context,
            IShippingAppRepository<Entities.ShippingPlan> shippingAppRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _shippingAppRepository = shippingAppRepository ?? throw new ArgumentNullException(nameof(shippingAppRepository));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Result> Handle(CreateNewShippingPLanCommand request, CancellationToken cancellationToken)
        {
            var config = await _mediator.Send(new GetConfigByKeyQuery
            {
                Key = ConfigKey.MinShippingDay
            });

            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            if (!int.TryParse(config.Value, out int numberDays))
            {
                throw new Exception("Failed to try parse value from config table");
            }


            if ((request.ShippingPlan.ShippingDate - DateTime.Now).TotalDays <= numberDays)
            {
                return Result.Failure($"Shipping Date should be larger than submit date {numberDays} days");
            }

            try
            {
                var shippingPlans = await _context.ShippingPlans
                .Include(x => x.Product)
                .ToListAsync();

                var shippingPlanDb = shippingPlans
                                        .Where(x => x.SalesOrder == request.ShippingPlan.SalesOrder)
                                        .Where(x => x.SalelineNumber == request.ShippingPlan.SalelineNumber)
                                        .Where(x => x.Product.ProductNumber == request.ShippingPlan.Product.ProductNumber)
                                        .FirstOrDefault();

                if (shippingPlanDb != null)
                {
                    return Result.Failure($"Shipping Plan ({shippingPlanDb.SalesOrder}-{shippingPlanDb.SalelineNumber}-{shippingPlanDb.Product.ProductNumber}) already existed");
                }
            }
            catch (Exception ex)
            {
                return Result.Failure($"Failed to create Shipping Plan");
            }

            var shippingPlan = _mapper.Map<Entities.ShippingPlan>(request.ShippingPlan);
            return await _shippingAppRepository.AddAsync(shippingPlan);
        }
    }
}
