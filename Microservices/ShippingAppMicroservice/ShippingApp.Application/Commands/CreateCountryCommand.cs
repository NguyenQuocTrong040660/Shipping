using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using ShippingApp.Domain.Interfaces;
using ShippingApp.Domain.Models;

namespace ShippingApp.Application.Commands
{
    public class CreateCountryCommand : IRequest<int>
    {
        public Country Model { get; set; }
    }
    public class CreateCountryCommandHandler : IRequestHandler<CreateCountryCommand, int>
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;
        public CreateCountryCommandHandler(IProductRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<int> Handle(CreateCountryCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Country>(request.Model);

            return await _repository.CreateCountry(entity);
        }
    }
}