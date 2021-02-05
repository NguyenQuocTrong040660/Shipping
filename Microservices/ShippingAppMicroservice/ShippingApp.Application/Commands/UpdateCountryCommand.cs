using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using ShippingApp.Domain.Models;
using System.Threading.Tasks;
using System.Threading;
using ShippingApp.Domain.Interfaces;
using AutoMapper;
using System.Net.WebSockets;
using ShippingApp.Application.Common.Exceptions;

namespace ShippingApp.Application.Commands
{
    public class UpdateCountryCommand : IRequest<int>
    {
        public Country Entity { get; set; }
        public string CountryCode { get; set; }
    }

    public class UpdateCountryCommanddHandler : IRequestHandler<UpdateCountryCommand, int>
    {
        private readonly IShippingAppRepository _shippingAppRepository;
        private readonly IMapper _mapper;

        public UpdateCountryCommanddHandler(IShippingAppRepository repository, IMapper mapper)
        {
            _shippingAppRepository = repository;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }


        public async Task<int> Handle(UpdateCountryCommand request, CancellationToken cancellationToken)
        {
            //var entity = await _shippingAppRepository.GetCountryByCode(request.CountryCode);

            //if (entity == null)
            //{
            //    throw new NotFoundException(nameof(entity), request.CountryCode);
            //}

            //return await _shippingAppRepository.UpdateCountry(request.CountryCode, _mapper.Map<Country>(request.Entity));
            return 0;
        }
    }
}
