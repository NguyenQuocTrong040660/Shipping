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
        public string Key { get; set; }
        public ConfigModel Config { get; set; }
    }

    public class UpdateConfigCommandHandler : IRequestHandler<UpdateConfigCommand, Result>
    {
        private readonly IMapper _mapper;
        private readonly IShippingAppDbContext _context;

        public UpdateConfigCommandHandler(IMapper mapper, IShippingAppDbContext context)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Result> Handle(UpdateConfigCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Configs.FindAsync(request.Key);

            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            entity.Value = request.Config.Value;
            entity.Descriptions = request.Config.Descriptions;

            return await _context.SaveChangesAsync() > 0 ? Result.Success() : Result.Failure("Failed to update config");
        }
    }
}
