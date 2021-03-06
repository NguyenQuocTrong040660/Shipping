using AutoMapper;
using MediatR;
using ShippingApp.Application.Interfaces;
using ShippingApp.Domain.Models;
using System;
using System.Threading;
using System.Threading.Tasks;
using Entities = ShippingApp.Domain.Entities;
using ShippingApp.Application.Common.Results;
using ShippingApp.Domain.Enumerations;
using ShippingApp.Application.Config.Queries;
using ShippingApp.Application.Product.Queries;
using System.Collections.Generic;
using System.Linq;

namespace ShippingApp.Application.ShippingRequest.Commands
{
    public class CreateShippingRequestCommand : IRequest<(Result, ShippingRequestResponse)>
    {
        public ShippingRequestModel ShippingRequest { get; set; }
    }

    public class CreateShippingRequestCommandHandler : IRequestHandler<CreateShippingRequestCommand, (Result, ShippingRequestResponse)>
    {
        private readonly IMapper _mapper;
        private readonly IShippingAppRepository<Entities.ShippingRequest> _shippingAppRepository;
        private readonly IMediator _mediator;
        private readonly IShippingAppDbContext _context;

        public CreateShippingRequestCommandHandler(IMapper mapper,
            IMediator mediator,
            IShippingAppDbContext context,
            IShippingAppRepository<Entities.ShippingRequest> shippingAppRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _shippingAppRepository = shippingAppRepository ?? throw new ArgumentNullException(nameof(shippingAppRepository));
        }

        public async Task<(Result, ShippingRequestResponse)> Handle(CreateShippingRequestCommand request, CancellationToken cancellationToken)
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

            if ((request.ShippingRequest.ShippingDate - DateTime.Now).TotalDays <= numberDays)
            {
                return (Result.Failure($"Shipping Date should be larger than submit date {numberDays} days"), null);
            }

            var entity = _mapper.Map<Entities.ShippingRequest>(request.ShippingRequest);

            entity.ShippingPlans = _context.ShippingPlans.ToList().Where(x => request.ShippingRequest.ShippingPlans.Any(s => s.Id == x.Id)).ToList();

            var shippingRequestLogistics = new List<Entities.ShippingRequestLogistic>();

            foreach (var item in entity.ShippingPlans)
            {
                shippingRequestLogistics.Add(new Entities.ShippingRequestLogistic
                {
                    ProductId = item.ProductId
                });
            }

            entity.ShippingRequestLogistics = shippingRequestLogistics;

            var result = await _shippingAppRepository.AddAsync(entity);

            var response = await BuildShippingResponse(entity);

            return (result, response);
        }

        private async Task<ShippingRequestResponse> BuildShippingResponse(Entities.ShippingRequest entity)
        {
            var configLogisticDeptEmail = await _mediator.Send(new GetConfigByKeyQuery
            {
                Key = ConfigKey.LogisticDeptEmail
            });

            var configShippingDeptEmail = await _mediator.Send(new GetConfigByKeyQuery
            {
                Key = ConfigKey.ShippingDeptEmail
            });

            if (configLogisticDeptEmail == null || configShippingDeptEmail == null)
            {
                return null;
            }

            var shippingRequestModel = _mapper.Map<ShippingRequestModel>(entity);

            foreach (var item in shippingRequestModel.ShippingPlans)
            {
                item.Product = await _mediator.Send(new GetProductByIdQuery { Id = item.ProductId });
            }

            return new ShippingRequestResponse
            {
                LogisticDeptEmail = configLogisticDeptEmail.Value,
                ShippingDeptEmail = configShippingDeptEmail.Value,
                ShippingRequest = shippingRequestModel
            };
        }
    }
}
