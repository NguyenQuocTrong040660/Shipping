using AutoMapper;
using MediatR;
using ShippingApp.Application.Interfaces;
using ShippingApp.Domain.Models;
using System;
using System.Threading;
using System.Threading.Tasks;
using Entities = ShippingApp.Domain.Entities;
using ShippingApp.Application.Common.Results;

namespace ShippingApp.Application.Config.Commands
{
    public class CreateConfigCommand : IRequest<Result>
    {
        public ConfigModel Config { get; set; }
    }

    public class CreateConfigCommandHandler : IRequestHandler<CreateConfigCommand, Result>
    {
        private readonly IMapper _mapper;
        private readonly IShippingAppRepository<Entities.Config> _shippingAppRepository;

        public CreateConfigCommandHandler(IMapper mapper, IShippingAppRepository<Entities.Config> shippingAppRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _shippingAppRepository = shippingAppRepository ?? throw new ArgumentNullException(nameof(shippingAppRepository));
        }

        public async Task<Result> Handle(CreateConfigCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Entities.Config>(request.Config);
            return await _shippingAppRepository.AddAsync(entity);
        }
    }
}
