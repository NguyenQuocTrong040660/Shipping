using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShippingApp.Application.Common.Results;
using ShippingApp.Application.MovementRequest.Commands;
using ShippingApp.Application.MovementRequest.Queries;
using ShippingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShippingApp.Api.Controllers
{
    public class MovementRequestController : BaseController
    {
        readonly ILogger<MovementRequestController> _logger;
        public MovementRequestController(IMediator mediator, ILogger<MovementRequestController> logger) : base(mediator)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(MovementRequestModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Result>> AddMovementRequest(MovementRequestModel movementRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(movementRequest);
            }

            var result = await Mediator.Send(new CreateMovementRequestCommand { MovementRequest = movementRequest });
            return Ok(result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<MovementRequestModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<MovementRequestModel>>> GetMovementRequests()
        {
            return await Mediator.Send(new GetMovementRequestsQuery { });
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(MovementRequestModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<MovementRequestModel>> GetMovementRequestByIdAsync(int id)
        {
            var result = await Mediator.Send(new GetMovementRequestByIdQuery { Id = id });
            return Ok(result);
        }

        [HttpGet("{id}/WithoutWorkOrder")]
        [ProducesResponseType(typeof(MovementRequestModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<MovementRequestModel>> GetMovementRequestByIdWithoutWOAsync(int id)
        {
            var result = await Mediator.Send(new GetMovementRequestByIdWithoutWorkOderQuery { Id = id });
            return Ok(result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MovementRequestModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Result>> UpdateMovementRequestAsync(int id, [FromBody] MovementRequestModel movementRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(movementRequest);
            }

            var result = await Mediator.Send(new UpdateMovementRequestCommand { Id = id, MovementRequest = movementRequest });
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Result>> DeleteMovementRequestAysnc(int id)
        {
            var result = await Mediator.Send(new DeleteMovementRequestCommand { Id = id });
            return Ok(result);
        }

        [HttpPost("Generate/MovementRequests")]
        [ProducesResponseType(typeof(List<MovementRequestModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<MovementRequestModel>>> GenerateMovementRequests([FromBody] List<WorkOrderModel> workOrders)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(workOrders);
            }

            var result = await Mediator.Send(new GenerateMovementRequestDetailsByWorkOdersQuery { WorkOrderModels = workOrders });
            return Ok(result);
        }
    }
}
