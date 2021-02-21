using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShippingApp.Application.Common.Results;
using ShippingApp.Application.Interfaces;
using ShippingApp.Application.WorkOrder.Commands;
using ShippingApp.Application.WorkOrder.Queries;
using ShippingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShippingApp.Api.Controllers
{
    public class WorkOrderController : BaseController
    {
        readonly ILogger<WorkOrderController> _logger;
        readonly IShippingAppDbContext _context;

        public WorkOrderController(
            IMediator mediator, 
            ILogger<WorkOrderController> logger, 
            IShippingAppDbContext context) : base(mediator)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        [HttpPost("BulkInsert")]
        [ProducesResponseType(typeof(List<WorkOrderModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<WorkOrderImportModel>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<WorkOrderModel>>> AddWorkOrderAsync([FromBody] List<WorkOrderImportModel> workOrderImports)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(workOrderImports);
            }

            var groupWorkOrders = from w in workOrderImports
                             group w by w.WorkOrderId into g
                             select new { WorkOrderId = g.Key, WorkOrderDetails = g.ToList() };

            var invalidworkOrders = new List<WorkOrderModel>();

            foreach (var group in groupWorkOrders)
            {
                var item = workOrderImports.FirstOrDefault(i => i.WorkOrderId == group.WorkOrderId);

                var workOrder = new WorkOrderModel
                {
                    Notes = item?.Notes ?? string.Empty,
                    RefId = group.WorkOrderId,
                };

                var workOderDetails = new List<WorkOrderDetailModel>();

                foreach (var workOderDetail in group.WorkOrderDetails)
                {
                    var product = await _context.Products.FirstOrDefaultAsync(e => e.ProductNumber.Equals(workOderDetail.ProductNumber));

                    if (product == null)
                    {
                        continue;
                    }

                    workOderDetails.Add(new WorkOrderDetailModel
                    {
                        ProductId = product.Id,
                        Quantity = workOderDetail.Quantity,
                        WorkOrderId = workOrder.Id,
                    });
                }

                workOrder.WorkOrderDetails = workOderDetails;

                var result = await Mediator.Send(new CreateWorkOrderCommand { WorkOrder = workOrder });

                if (!result.Succeeded)
                {
                    invalidworkOrders.Add(workOrder);
                }
            }

            return Ok(invalidworkOrders);
        }


        [HttpPost]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(WorkOrderModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Result>> AddWorkOrderAsync(WorkOrderModel workOrder)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(workOrder);
            }

            var result = await Mediator.Send(new CreateWorkOrderCommand { WorkOrder = workOrder });
            return Ok(result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<WorkOrderModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<WorkOrderModel>>> GetWorkOrders()
        {
            return await Mediator.Send(new GetWorkOrdersQuery { });
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(WorkOrderModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<WorkOrderModel>> GetWorkOrderByIdAsync(int id)
        {
            var result = await Mediator.Send(new GetWorkOrderByIdQuery { Id = id });
            return Ok(result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(WorkOrderModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Result>> UpdateWorkOrderAsync(int id, [FromBody] WorkOrderModel workOrder)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(workOrder);
            }

            var result = await Mediator.Send(new UpdateWorkOrderCommand { Id = id, WorkOrder = workOrder });
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Result>> DeleteWorkOrderAysnc(int id)
        {
            var result = await Mediator.Send(new DeleteWorkOrderCommand { Id = id });
            return Ok(result);
        }
    }
}
