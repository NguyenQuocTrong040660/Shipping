using AutoMapper;
using MediatR;
using ShippingApp.Application.Interfaces;
using ShippingApp.Domain.Models;
using System;
using System.Threading;
using System.Threading.Tasks;
using Entities = ShippingApp.Domain.Entities;
using ShippingApp.Application.Common.Results;
using Microsoft.EntityFrameworkCore;

namespace ShippingApp.Application.ShippingRequest.Commands
{
    public class UpdateShippingRequestLogisticCommand : IRequest<Result>
    {
        public int ShippingRequestId { get; set; }
        public ShippingRequestLogisticModel ShippingRequestLogistic { get; set; }
    }

    public class UpdateShippingRequestLogisticCommandHandler : IRequestHandler<UpdateShippingRequestLogisticCommand, Result>
    {
        private readonly IMapper _mapper;
        private readonly IShippingAppDbContext _context;

        public UpdateShippingRequestLogisticCommandHandler(IMapper mapper,
            IShippingAppDbContext context)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Result> Handle(UpdateShippingRequestLogisticCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.ShippingRequestLogistics.FirstOrDefaultAsync(x => x.ShippingRequestId == request.ShippingRequestId);

            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            entity.BillToCustomer = request.ShippingRequestLogistic.BillToCustomer;
            entity.CustomDeclarationNumber = request.ShippingRequestLogistic.CustomDeclarationNumber;
            entity.Notes = request.ShippingRequestLogistic.Notes;
            entity.ReceiverCustomer = request.ShippingRequestLogistic.ReceiverCustomer;
            entity.ReceiverAddress = request.ShippingRequestLogistic.ReceiverAddress;
            entity.TrackingNumber = request.ShippingRequestLogistic.TrackingNumber;
            entity.GrossWeight = request.ShippingRequestLogistic.GrossWeight;

            return await _context.SaveChangesAsync() > 0
                ? Result.Success()
                : Result.Failure($"Failed to update shipping request logistic");
        }
    }
}
