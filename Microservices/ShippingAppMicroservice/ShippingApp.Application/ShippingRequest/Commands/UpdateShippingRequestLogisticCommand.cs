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
        public int ProductId { get; set; }
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
            var entity = await _context.ShippingRequestLogistics
                .FirstOrDefaultAsync(x => x.ShippingRequestId == request.ShippingRequestId 
                && x.ProductId == request.ProductId);

            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            entity.CustomDeclarationNumber = request.ShippingRequestLogistic.CustomDeclarationNumber;
            entity.Notes = request.ShippingRequestLogistic.Notes;
            entity.ShipTo = request.ShippingRequestLogistic.ShipTo;
            entity.ShipToAddress = request.ShippingRequestLogistic.ShipToAddress;
            entity.TrackingNumber = request.ShippingRequestLogistic.TrackingNumber;
            entity.GrossWeight = request.ShippingRequestLogistic.GrossWeight;

            entity.BillTo = request.ShippingRequestLogistic.BillTo;
            entity.BillToAddress = request.ShippingRequestLogistic.BillToAddress;
            entity.ShipTo = request.ShippingRequestLogistic.ShipTo;
            entity.ShipToAddress = request.ShippingRequestLogistic.ShipToAddress;

            entity.Forwarder = request.ShippingRequestLogistic.Forwarder;
            entity.NetWeight = request.ShippingRequestLogistic.NetWeight;
            entity.Dimension = request.ShippingRequestLogistic.Dimension;

            await _context.SaveChangesAsync();
            return Result.Success();
        }
    }
}
