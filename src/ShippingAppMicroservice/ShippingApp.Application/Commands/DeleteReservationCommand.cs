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
    public class DeleteReservationCommand : IRequest<int>
    {
     public Guid Id { get; set; }
    }

    public class DeleteReservationCommandHandler : IRequestHandler<DeleteReservationCommand, int>
    {
        private readonly IReservationRepository _Repository;
        private readonly IMapper _mapper;

        public DeleteReservationCommandHandler(IReservationRepository Repository, IMapper mapper)
        {
            _Repository = Repository;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<int> Handle(DeleteReservationCommand request, CancellationToken cancellationToken)
        {
            return await _Repository.DeleteReservation(request.Id);
        }
    }
}
