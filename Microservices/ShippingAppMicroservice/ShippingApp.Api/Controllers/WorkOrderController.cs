using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShippingApp.Application.Common.Results;
using ShippingApp.Application.WorkOrder.Commands;
using ShippingApp.Application.WorkOrder.Queries;
using ShippingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShippingApp.Api.Controllers
{
    public class WorkOrderController : BaseController
    {
        readonly ILogger<WorkOrderController> _logger;
        public WorkOrderController(IMediator mediator, ILogger<WorkOrderController> logger) : base(mediator)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status404NotFound)]
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
