using AutoMapper;
using MediatR;
using ShippingApp.Application.Common.Results;
using ShippingApp.Application.Interfaces;
using ShippingApp.Domain.Models;
using System;
using Entities = ShippingApp.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ShippingApp.Application.WorkOrder.Commands
{
    public class UpdateWorkOrderCommand : IRequest<Result>
    {
        public int Id { get; set; }
        public WorkOrderModel WorkOrder { get; set; }
    }

    public class UpdateWorkOrderCommandHandler : IRequestHandler<UpdateWorkOrderCommand, Result>
    {
        private readonly IMapper _mapper;
        private readonly IShippingAppRepository<Entities.WorkOrder> _shippingAppRepository;
        private readonly IShippingAppDbContext _context;

        public UpdateWorkOrderCommandHandler(IMapper mapper, IShippingAppRepository<Entities.WorkOrder> shippingAppRepository, IShippingAppDbContext context)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _shippingAppRepository = shippingAppRepository ?? throw new ArgumentNullException(nameof(shippingAppRepository));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Result> Handle(UpdateWorkOrderCommand request, CancellationToken cancellationToken)
        {
            var workOrder = await _context.WorkOrders
                .Include(x => x.WorkOrderDetails)
                .Where(x => x.Id == request.WorkOrder.Id)
                .FirstOrDefaultAsync();

            foreach (var item in workOrder.WorkOrderDetails)
            {
                var woDetail = request.WorkOrder.WorkOrderDetails.FirstOrDefault(i => i.ProductId == item.ProductId);

                if (woDetail == null)
                {
                    _context.WorkOrderDetails.Remove(item);
                }
                else
                {
                    item.Quantity = woDetail.Quantity;
                }
            }

            workOrder.Notes = request.WorkOrder.Notes;

            return await _context.SaveChangesAsync() > 0
                ? Result.Success()
                : Result.Failure($"Failed to update work order");
        }
    }
}
