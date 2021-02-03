using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ShippingApp.Application.Common.Exceptions;
using ShippingApp.Domain.Interfaces;
using ShippingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Entities = ShippingApp.Domain.Entities;

namespace ShippingApp.Application.Commands
{
    public class UpdateMemberShipCommand : IRequest<int>
    {
        public MemberShip Entity { get; set; }
        public Guid Id { get; set; }
    }
    public class UpdateMemberShipCommanddHandler : IRequestHandler<UpdateMemberShipCommand, int>
    {
        private readonly IMemberShipRepository _Repository;
        private readonly IMapper _mapper;

        public UpdateMemberShipCommanddHandler(IMemberShipRepository Repository, IMapper mapper)
        {
            _Repository = Repository;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<int> Handle(UpdateMemberShipCommand request, CancellationToken cancellationToken)
        {
            var entity = await _Repository.GetMemberShipByID(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(entity), request.Id);
            }

            return await _Repository.UpdateMemberShip(request.Entity);
        }
    }


}
