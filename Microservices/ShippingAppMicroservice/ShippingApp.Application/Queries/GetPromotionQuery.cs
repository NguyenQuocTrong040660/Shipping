using ShippingApp.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using ShippingApp.Domain.Models;
using System.Linq;

namespace ShippingApp.Application.Queries
{
    public class GetPromotionQuery : IRequest<List<Promotion>>
    {
    }
    public class GetPromotionQueryHandler : IRequestHandler<GetPromotionQuery, List<Promotion>>
    {
        private readonly IPromotionRepository _Repository;
        public GetPromotionQueryHandler(IPromotionRepository Repository)
        {
            _Repository = Repository;
        }

        public async Task<List<Promotion>> Handle(GetPromotionQuery request, CancellationToken cancellationToken)
        {
            List<Promotion> Promotions = _Repository.GetAllPromotion();
            return  await Task.FromResult(Promotions);
        }

    }

    
}
