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
    public class GetReservationQuery : IRequest<List<Reservation>>
    {
    }
    public class GetReservationQueryHandler : IRequestHandler<GetReservationQuery, List<Reservation>>
    {
        private readonly IReservationRepository _reservationRepository;
        public GetReservationQueryHandler(IReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;
        }

        public async Task<List<Reservation>> Handle(GetReservationQuery request, CancellationToken cancellationToken)
        {
            List<Reservation> reservations = _reservationRepository.GetAllReservation();
            return  await Task.FromResult(reservations);
        }

    }

    
}
