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

            var result = await Mediator.Send(new CreateReceivedMarkCommand { ReceivedMark = model });
            return Ok(result);
        }
     
        [HttpGet]
        [ProducesResponseType(typeof(List<ReceivedMarkModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<ReceivedMarkModel>>> GetReceivedMarks()
        {
            return await Mediator.Send(new GetReceivedMarksQuery { });
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ReceivedMarkModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ReceivedMarkModel>> GetReceivedMarkByIdAsync(int id)
        {
            var result = await Mediator.Send(new GetReceivedMarkByIdQuery { Id = id });
            return Ok(result);
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

            var result = await Mediator.Send(new UpdateReceivedMarkCommand { Id = id, ReceivedMark = model });
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Result>> DeleteReceivedMarkAysnc(int id)
        {
            var result = await Mediator.Send(new DeleteReceivedMarkCommand { Id = id });
            return Ok(result);
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

            var result = await Mediator.Send(new UnstuffReceivedMarkCommand { UnstuffReceivedMark = model });
            return Ok(result);
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

            var result = await Mediator.Send(new GenerateReceivedMarkMovementsByMovementRequestsQuery 
            { 
                MovementRequests = movementRequests 
            });

            return Ok(result);
        }

        [HttpGet("ReceivedMarkSummaries/{receivedMarkId}")]
        [ProducesResponseType(typeof(List<ReceivedMarkSummaryModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<ReceivedMarkSummaryModel>>> GetReceivedMarkSummariesAsync(int receivedMarkId)
        {
            var result = await Mediator.Send(new GetReceivedMarkSummariesByIdQuery { ReceivedMarkId = receivedMarkId });
            return Ok(result);
        }

        [HttpGet("ReceivedMarkPrintings/{receivedMarkId}/{productId}")]
        [ProducesResponseType(typeof(List<ReceivedMarkPrintingModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<ReceivedMarkPrintingModel>>> GetReceivedMarkPrintingsAsync(
            int receivedMarkId, int productId)
        {
            var result = await Mediator.Send(new GetReceivedMarkPrintingsByIdQuery 
            { 
                ReceivedMarkId = receivedMarkId, 
                ProductId = productId 
            });

            return Ok(result);
        }

        [HttpGet("ReceivedMarkPrintings/Product/{productId}")]
        [ProducesResponseType(typeof(List<ReceivedMarkPrintingModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<ReceivedMarkPrintingModel>>> GetReceivedMarkPrintingsByProductAsync(int productId)
        {
            var result = await Mediator.Send(new GetReceivedMarkPrintingsByProductIdQuery
            {
                ProductId = productId
            });

            return Ok(result);
        }
    }
}
