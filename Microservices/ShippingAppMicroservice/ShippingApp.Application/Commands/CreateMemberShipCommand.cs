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
    public class CreateMemberShipCommand : IRequest<int>
    {
        public MemberShip Model { get; set; }
    }
    public class CreateMemberShipCommandHandler : IRequestHandler<CreateMemberShipCommand, int>
    {
        private readonly IMemberShipRepository _Repository;
        private readonly IMapper _mapper;

        public CreateMemberShipCommandHandler(IMemberShipRepository Repository, IMapper mapper)
        {
            _Repository = Repository;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<int> Handle(CreateMemberShipCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<MemberShip>(request.Model);
            return await _Repository.CreateMemberShip(entity);
        }
    }
}
