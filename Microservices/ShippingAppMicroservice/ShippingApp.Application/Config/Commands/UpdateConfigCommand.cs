using AutoMapper;
using MediatR;
using ShippingApp.Application.Common.Results;
using ShippingApp.Application.Interfaces;
using ShippingApp.Domain.Models;
using System;
using Entities = ShippingApp.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace ShippingApp.Application.Config.Commands
{
    public class UpdateConfigCommand : IRequest<Result>
    {
        public int Id { get; set; }
        public ConfigModel Config { get; set; }
    }

    public class UpdateConfigCommandHandler : IRequestHandler<UpdateConfigCommand, Result>
    {
        private readonly IMapper _mapper;
        private readonly IShippingAppRepository<Entities.Config> _shippingAppRepository;

        public UpdateConfigCommandHandler(IMapper mapper, IShippingAppRepository<Entities.Config> shippingAppRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _shippingAppRepository = shippingAppRepository ?? throw new ArgumentNullException(nameof(shippingAppRepository));
        }

        public async Task<Result> Handle(UpdateConfigCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Entities.Config>(request.Config);
            return await _shippingAppRepository.Update(request.Id, entity);
        }
    }
}
