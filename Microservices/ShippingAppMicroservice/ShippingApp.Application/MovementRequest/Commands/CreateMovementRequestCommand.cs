using AutoMapper;
using MediatR;
using ShippingApp.Application.Interfaces;
using ShippingApp.Domain.Models;
using System;
using System.Threading;
using System.Threading.Tasks;
using Entities = ShippingApp.Domain.Entities;
using ShippingApp.Application.Common.Results;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ShippingApp.Domain.Enumerations;

namespace ShippingApp.Application.MovementRequest.Commands
{
    public class CreateMovementRequestCommand : IRequest<Result>
    {
        public MovementRequestModel MovementRequest { get; set; }
    }

    public class CreateMovementRequestCommandHandler : IRequestHandler<CreateMovementRequestCommand, Result>
    {
        private readonly IMapper _mapper;
        private readonly IShippingAppRepository<Entities.MovementRequest> _shippingAppRepository;
        private readonly IShippingAppDbContext _context;

        public CreateMovementRequestCommandHandler(IMapper mapper, IShippingAppRepository<Entities.MovementRequest> shippingAppRepository, IShippingAppDbContext context)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _shippingAppRepository = shippingAppRepository ?? throw new ArgumentNullException(nameof(shippingAppRepository));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Result> Handle(CreateMovementRequestCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await ValidateMovementRequestAsync(request.MovementRequest);

            if (!validationResult.Succeeded)
            {
                return validationResult;
            }

            var entity = _mapper.Map<Entities.MovementRequest>(request.MovementRequest);

            var result = await _shippingAppRepository.AddAsync(entity);

            if (result.Succeeded)
            {
                await UpdateWorkOrderAsync(entity);
            }

            return result;
        }

        private async Task UpdateWorkOrderAsync(Entities.MovementRequest entity)
        {
            foreach (var item in entity.MovementRequestDetails)
            {
                var workOrder = await _context.WorkOrders
                    .Include(x => x.MovementRequestDetails)
                    .FirstOrDefaultAsync(i => i.Id == item.WorkOrderId);

                if (GetRemainQuantityWorkOrder(workOrder) == 0)
                {
                    workOrder.Status = nameof(WorkOrderStatus.Close);
                }
                else
                {
                    workOrder.Status = nameof(WorkOrderStatus.Start);
                }
            }

            await _context.SaveChangesAsync();
        }

        private async Task<Result> ValidateMovementRequestAsync(MovementRequestModel model)
        {
            foreach (var item in model.MovementRequestDetails)
            {
                var workOrder = await _context.WorkOrders
                    .Include(x => x.MovementRequestDetails)
                    .FirstOrDefaultAsync(i => i.Id == item.WorkOrderId);

                var remainQuantityOfWorkOrder = GetRemainQuantityWorkOrder(workOrder);

                if (item.Quantity > remainQuantityOfWorkOrder)
                {
                    return Result.Failure("Quantity can not be greater than Remain Quantity of Work Order");
                }
            }

            return Result.Success();
        }

        private int GetRemainQuantityWorkOrder(Entities.WorkOrder workOrder)
        {
            if (workOrder.MovementRequestDetails == null || !workOrder.MovementRequestDetails.Any())
            {
                return workOrder.Quantity;
            }

            return workOrder.Quantity - workOrder.MovementRequestDetails.Sum(x => x.Quantity);
        }
    }
}
