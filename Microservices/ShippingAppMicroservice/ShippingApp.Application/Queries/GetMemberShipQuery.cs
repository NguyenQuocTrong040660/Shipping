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
    public class GetMemberShipQuery : IRequest<List<MemberShip>>
    {
    }
    public class GetMemberShipQueryHandler : IRequestHandler<GetMemberShipQuery, List<MemberShip>>
    {
        private readonly IMemberShipRepository _Repository;
        public GetMemberShipQueryHandler(IMemberShipRepository Repository)
        {
            _Repository = Repository;
        }

        public async Task<List<MemberShip>> Handle(GetMemberShipQuery request, CancellationToken cancellationToken)
        {
            List<MemberShip> memberShips = _Repository.GetAllMemberShip();
            return  await Task.FromResult(memberShips);
        }

    }

    
}
