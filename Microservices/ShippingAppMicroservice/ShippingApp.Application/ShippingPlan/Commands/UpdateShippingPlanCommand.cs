using AutoMapper;
using MediatR;
using ShippingApp.Application.Interfaces;
using ShippingApp.Domain.Models;
using System;
using System.Threading;
using System.Threading.Tasks;
using ShippingApp.Application.Common.Results;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Entities = ShippingApp.Domain.Entities;

namespace ShippingApp.Application.ShippingPlan.Commands
{
    public class UpdateShippingPlanCommand : IRequest<Result>
    {
        public int Id { get; set; }
        public ShippingPlanModel ShippingPlan { get; set; }
    }

    public class UpdateShippingPlanCommandHandler : IRequestHandler<UpdateShippingPlanCommand, Result>
    {
        private readonly IShippingAppDbContext _context;
        private readonly IMapper _mapper;

        public UpdateShippingPlanCommandHandler(IShippingAppDbContext context, IMapper mapper)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Result> Handle(UpdateShippingPlanCommand request, CancellationToken cancellationToken)
        {
            var shippingPlan = await _context.ShippingPlans
                .Include(x => x.ShippingPlanDetails)
                .Where(x => x.Id == request.ShippingPlan.Id)
                .FirstOrDefaultAsync();

            if (shippingPlan == null)
            {
                throw new ArgumentNullException(nameof(shippingPlan));
            }

            foreach (var item in shippingPlan.ShippingPlanDetails)
            {
                var shippingPlanDetail = request.ShippingPlan.ShippingPlanDetails.FirstOrDefault(i => i.ProductId == item.ProductId);

                if (shippingPlanDetail == null)
                {
                    _context.ShippingPlanDetails.Remove(item);
                }
                else
                {
                    item.Quantity = shippingPlanDetail.Quantity;
                    item.Price = shippingPlanDetail.Price;
                    item.Amount = shippingPlanDetail.Amount;
                    item.ShippingMode = shippingPlanDetail.ShippingMode;
                }
            }

            foreach (var item in request.ShippingPlan.ShippingPlanDetails)
            {
                var shippingPlanDetail = shippingPlan.ShippingPlanDetails.FirstOrDefault(i => i.ProductId == item.ProductId);

                if (shippingPlanDetail == null)
                {
                    var shippingPlanDetailEntity = _mapper.Map<Entities.ShippingPlanDetail>(item);
                    shippingPlanDetailEntity.ShippingPlanId = shippingPlan.Id;
                    _context.ShippingPlanDetails.Add(shippingPlanDetailEntity);
                }
            }

            shippingPlan.CustomerName = request.ShippingPlan.CustomerName;
            shippingPlan.SemlineNumber = request.ShippingPlan.SemlineNumber;
            shippingPlan.PurchaseOrder = request.ShippingPlan.PurchaseOrder;
            shippingPlan.ShippingDate = request.ShippingPlan.ShippingDate;
            shippingPlan.SalesID = request.ShippingPlan.SalesID;
            shippingPlan.Notes = request.ShippingPlan.Notes;

            await _context.SaveChangesAsync();

            return Result.Success();
        }
    }
}
