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
    public class DeleteMemberShipCommand : IRequest<int>
    {
     public Guid Id { get; set; }
    }

    public class DeleteMemberShipCommandHandler : IRequestHandler<DeleteMemberShipCommand, int>
    {
        private readonly IMemberShipRepository _Repository;
        private readonly IMapper _mapper;

        public DeleteMemberShipCommandHandler(IMemberShipRepository Repository, IMapper mapper)
        {
            _Repository = Repository;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<int> Handle(DeleteMemberShipCommand request, CancellationToken cancellationToken)
        {
            return await _Repository.DeleteMemberShip(request.Id);
        }
    }
}
