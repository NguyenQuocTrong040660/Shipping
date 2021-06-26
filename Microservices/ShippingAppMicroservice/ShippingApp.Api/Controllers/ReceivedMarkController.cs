using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShippingApp.Application.Common.Results;
using ShippingApp.Application.ReceivedMark.Commands;
using ShippingApp.Application.ReceivedMark.Queries;
using ShippingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShippingApp.Api.Controllers
{
    public class ReceivedMarkController : BaseController
    {
        readonly ILogger<ReceivedMarkController> _logger;
        public ReceivedMarkController(IMediator mediator, ILogger<ReceivedMarkController> logger) : base(mediator)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ReceivedMarkModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Result>> AddReceivedMarkAsync(ReceivedMarkModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(model);
            }

            return Ok(await Mediator.Send(new CreateReceivedMarkCommand { ReceivedMark = model }));
        }
     
        [HttpGet]
        [ProducesResponseType(typeof(List<ReceivedMarkModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<ReceivedMarkModel>>> GetReceivedMarks()
        {
            return Ok(await Mediator.Send(new GetReceivedMarksQuery { }));
        }

        [HttpGet("WorkOrder")]
        [ProducesResponseType(typeof(List<WorkOrderModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<WorkOrderModel>>> GetReceivedMarkGroupByWorkOrders()
        {
            return Ok(await Mediator.Send(new GetReceivedMarksGroupByWorkOrdersQuery { }));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ReceivedMarkModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ReceivedMarkModel>> GetReceivedMarkByIdAsync(int id)
        {
            return Ok(await Mediator.Send(new GetReceivedMarkByIdQuery { Id = id }));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ReceivedMarkModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Result>> UpdateReceivedMarkAsync(int id, [FromBody] ReceivedMarkModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(model);
            }

            return Ok(await Mediator.Send(new UpdateReceivedMarkCommand { Id = id, ReceivedMark = model }));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Result>> DeleteReceivedMarkAysnc(int id)
        {
            return Ok(await Mediator.Send(new DeleteReceivedMarkCommand { Id = id }));
        }

        [HttpPost("Unstuff")]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(UnstuffReceivedMarkRequest), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Result>> UnstuffReceivedMark([FromBody] UnstuffReceivedMarkRequest model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(model);
            }

            if (model.UnstuffQuantity <= 0)
            {
                return Ok(Result.Failure("Unstuff Quantity could not less than 0"));
            }

            return Ok(await Mediator.Send(new UnstuffReceivedMarkCommand { UnstuffReceivedMark = model }));
        }

        [HttpPost("Merge")]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<ReceivedMarkPrintingModel>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Result>> MergeReceivedMarkAsync([FromBody] List<ReceivedMarkPrintingModel> receivedMarkPrintings)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(receivedMarkPrintings);
            }

            return Ok(await Mediator.Send(new MergeReceivedMarkCommand 
            {
                ReceivedMarkPrintings = receivedMarkPrintings
            }));
        }

        [HttpPost("Print")]
        [ProducesResponseType(typeof(ReceivedMarkPrintingModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ReceivedMarkPrintingModel), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(PrintReceivedMarkRequest), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ReceivedMarkPrintingModel>> PrintReceivedMarkAsync([FromBody] PrintReceivedMarkRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(request);
            }

            var result = await Mediator.Send(new PrintReceivedMarkCommand
            {
                PrintReceivedMarkRequest = request
            });

            if (result == null)
            {
                return NoContent();
            }
            
            return Ok(result);
        }

        [HttpPut("Print/{receivedMarkPrintingId}")]
        [ProducesResponseType(typeof(ReceivedMarkPrintingModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ReceivedMarkPrintingModel), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(PrintReceivedMarkRequest), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ReceivedMarkPrintingModel>> PrintReceivedMarkWithPrintingIdAsync(int receivedMarkPrintingId, 
            [FromBody] PrintReceivedMarkRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(request);
            }

            var result = await Mediator.Send(new PrintReceivedMarkCommand
            {
                PrintReceivedMarkRequest = request,
                ReceivedMarkPrintingId = receivedMarkPrintingId
            });

            if (result == null)
            {
                return NoContent();
            }

            return Ok(result);
        }

        [HttpPost("RePrint")]
        [ProducesResponseType(typeof(ReceivedMarkPrintingModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ReceivedMarkPrintingModel), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(RePrintReceivedMarkRequest), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ReceivedMarkPrintingModel>> RePrintReceivedMarkAsync([FromBody] RePrintReceivedMarkRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(request);
            }

            var result = await Mediator.Send(new RePrintReceivedMarkCommand
            {
                RePrintReceivedMarkRequest = request
            });

            if (result == null)
            {
                return NoContent();
            }

            return Ok(result);
        }

        [HttpPost("Generate/ReceivedMarkMovements")]
        [ProducesResponseType(typeof(List<ReceivedMarkMovementModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<ReceivedMarkMovementModel>>> GenerateReceivedMarkMovements(
            [FromBody] List<MovementRequestModel> movementRequests)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(movementRequests);
            }

            return Ok(await Mediator.Send(new GenerateReceivedMarkMovementsByMovementRequestsQuery
            {
                MovementRequests = movementRequests
            }));
        }

        [HttpGet("ReceivedMarkMovements/{receivedMarkId}")]
        [ProducesResponseType(typeof(List<ReceivedMarkMovementModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<ReceivedMarkMovementModel>>> GetReceivedMarkMovementsFullInfoAsync(int receivedMarkId)
        {
            return Ok(await Mediator.Send(new GetReceivedMarkMovementsFullInfoByIdQuery { ReceivedMarkId = receivedMarkId }));
        }

        [HttpGet("ReceivedMarkMovements/WorkOrder/{workOrderId}")]
        [ProducesResponseType(typeof(List<ReceivedMarkMovementModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<ReceivedMarkMovementModel>>> GetReceivedMarkMovementsFullInfoByWorkOrderAsync(int workOrderId)
        {
            return Ok(await Mediator.Send(new GetReceivedMarkMovementsFullInfoByWorkOrderIdQuery { WorkOrderId = workOrderId }));
        }

        [HttpGet("ReceivedMarkPrintings")]
        [ProducesResponseType(typeof(List<ReceivedMarkPrintingModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<ReceivedMarkPrintingModel>>> GetReceivedMarkPrintingsAsync([FromQuery] GetReceivedMarkPrintingsByIdQuery query)
        {
            return await Mediator.Send(query);
        }
    }
}
