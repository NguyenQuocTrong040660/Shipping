using ShippingApp.Application.Interfaces;
using System;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Entities = ShippingApp.Domain.Entities;
using ShippingApp.Domain.Models;

namespace ShippingApp.Application.Config.Queries
{
    public class GetConfigByKeyQuery : IRequest<ConfigModel>
    {
        public string Key { get; set; }
    }

    public class GetConfigByKeyQueryHandler : IRequestHandler<GetConfigByKeyQuery, ConfigModel>
    {
        private readonly IMapper _mapper;
        private readonly IShippingAppDbContext _context;

        public GetConfigByKeyQueryHandler(IMapper mapper, IShippingAppDbContext context)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<ConfigModel> Handle(GetConfigByKeyQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.Configs.FindAsync(request.Key);
            return _mapper.Map<ConfigModel>(entity);
        }
    }
}
