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
    public class DeleteCountryCommand : IRequest<int>
    {
        public string CountryCode { get; set; }
    }
    public class DeleteCountryCommandHandler : IRequestHandler<DeleteCountryCommand, int>
    {
        private readonly IShippingAppRepository _shippingAppRepository;
        private readonly IMapper _mapper;

        public DeleteCountryCommandHandler(IShippingAppRepository repository, IMapper mapper)
        {
            _shippingAppRepository = repository;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<int> Handle(DeleteCountryCommand request, CancellationToken cancellationToken)
        {
            //return await _shippingAppRepository.DeleteCountry(request.CountryCode);
            return 0;
        }
    }

}




