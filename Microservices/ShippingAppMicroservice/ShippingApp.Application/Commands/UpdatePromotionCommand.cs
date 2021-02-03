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
    public class UpdatePromotionCommand : IRequest<int>
    {
        public Promotion Entity { get; set; }
        public Guid Id { get; set; }
    }
    public class UpdatePromotionCommanddHandler : IRequestHandler<UpdatePromotionCommand, int>
    {
        private readonly IPromotionRepository _Repository;
        private readonly IMapper _mapper;

        public UpdatePromotionCommanddHandler(IPromotionRepository Repository, IMapper mapper)
        {
            _Repository = Repository;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<int> Handle(UpdatePromotionCommand request, CancellationToken cancellationToken)
        {
            var entity = await _Repository.GetPromotionByID(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(entity), request.Id);
            }

            return await _Repository.UpdatePromotion(request.Entity);
        }
    }


}
