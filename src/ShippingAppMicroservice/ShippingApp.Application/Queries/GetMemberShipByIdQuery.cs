using AutoMapper;
using MediatR;
using ShippingApp.Domain.Interfaces;
using ShippingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ShippingApp.Application.Queries
{
    public class GetMemberShipByIdQuery : IRequest<MemberShip>
    {
        public Guid Id;
    }

    public class GetMemberShipByIdQueryHandler : IRequestHandler<GetMemberShipByIdQuery, MemberShip>
    {
        private readonly IMemberShipRepository _repository;
        private readonly IMapper _mapper;

        public GetMemberShipByIdQueryHandler(IMemberShipRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<MemberShip> Handle(GetMemberShipByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _repository.GetMemberShipByID(request.Id);
            return await Task.FromResult(_mapper.Map<MemberShip>(result));
        }
    }
}
