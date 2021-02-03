using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ShippingApp.Application.Common.Exceptions;
//using ShippingApp.Domain.Entities;
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
    public class UpdateReservationCommand : IRequest<int>
    {
        public Reservation Entity { get; set; }
        public Guid Id { get; set; }
    }
    public class UpdateReservationCommandHandler : IRequestHandler<UpdateReservationCommand, int>
    {
        private readonly IReservationRepository _Repository;
        private readonly IMapper _mapper;

        public UpdateReservationCommandHandler(IReservationRepository Repository, IMapper mapper)
        {
            _Repository = Repository;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<int> Handle(UpdateReservationCommand request, CancellationToken cancellationToken)
        {
            var entity = await _Repository.GetReservationByID(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(entity), request.Id);
            }

            return await _Repository.UpdateReservation(request.Entity);
        }
    }


}
