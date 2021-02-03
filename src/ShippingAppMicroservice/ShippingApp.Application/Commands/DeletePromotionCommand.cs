using AutoMapper;
using MediatR;
using ShippingApp.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ShippingApp.Application.Commands
{
    public class DeletePromotionCommand : IRequest<int>
    {
     public Guid Id { get; set; }
    }

    public class DeletePromotionCommandHandler : IRequestHandler<DeletePromotionCommand, int>
    {
        private readonly IPromotionRepository _Repository;
        private readonly IMapper _mapper;

        public DeletePromotionCommandHandler(IPromotionRepository Repository, IMapper mapper)
        {
            _Repository = Repository;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<int> Handle(DeletePromotionCommand request, CancellationToken cancellationToken)
        {
            return await _Repository.DeletePromotion(request.Id);
        }
    }
}
