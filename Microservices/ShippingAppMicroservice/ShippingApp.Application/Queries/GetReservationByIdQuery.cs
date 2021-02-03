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
    public class GetReservationByIdQuery : IRequest<Reservation>
    {
        public Guid ReservationId;
    }

    public class GetReservationByIdQueryHandler : IRequestHandler<GetReservationByIdQuery, Reservation>
    {
        private readonly IReservationRepository _repository;
        private readonly IMapper _mapper;

        public GetReservationByIdQueryHandler(IReservationRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Reservation> Handle(GetReservationByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _repository.GetReservationByID(request.ReservationId);
            return await Task.FromResult(_mapper.Map<Reservation>(result));
        }
    }
}
