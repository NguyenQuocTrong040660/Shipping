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
    public class CreateReservationCommand : IRequest<int>
    {
        public Reservation Model { get; set; }
    }
    public class CreateReservationCommandHandler : IRequestHandler<CreateReservationCommand, int>
    {
        private readonly IReservationRepository _Repository;
        private readonly IMapper _mapper;

        public CreateReservationCommandHandler(IReservationRepository Repository, IMapper mapper)
        {
            _Repository = Repository;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<int> Handle(CreateReservationCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Reservation>(request.Model);

            return await _Repository.CreateReservation(entity);
        }
    }
}
