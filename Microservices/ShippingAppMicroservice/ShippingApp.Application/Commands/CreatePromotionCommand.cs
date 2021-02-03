using AutoMapper;
using MediatR;
using ShippingApp.Domain.Interfaces;
using ShippingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ShippingApp.Application.Commands
{
    public class CreatePromotionCommand : IRequest<int>
    {
        public Promotion Model { get; set; }
    }
    public class CreatePromotionCommandHandler : IRequestHandler<CreatePromotionCommand, int>
    {
        private readonly IPromotionRepository _Repository;
        private readonly IMapper _mapper;

        public CreatePromotionCommandHandler(IPromotionRepository Repository, IMapper mapper)
        {
            _Repository = Repository;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<int> Handle(CreatePromotionCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Promotion>(request.Model);
            return await _Repository.CreatePromotion(entity);
        }
    }
}
