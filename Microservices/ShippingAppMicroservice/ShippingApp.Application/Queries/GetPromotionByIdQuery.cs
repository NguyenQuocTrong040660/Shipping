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
    public class GetPromotionByIdQuery : IRequest<Promotion>
    {
        public Guid Id;
    }

    public class GetPromotionByIdQueryHandler : IRequestHandler<GetPromotionByIdQuery, Promotion>
    {
        private readonly IPromotionRepository _repository;
        private readonly IMapper _mapper;

        public GetPromotionByIdQueryHandler(IPromotionRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Promotion> Handle(GetPromotionByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _repository.GetPromotionByID(request.Id);
            return await Task.FromResult(_mapper.Map<Promotion>(result));
        }
    }
}
