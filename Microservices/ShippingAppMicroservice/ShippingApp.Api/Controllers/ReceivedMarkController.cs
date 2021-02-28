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
        public async Task<ActionResult<Result>> UpdateReceivedMarkAsync(int id, [FromBody]ReceivedMarkModel model)
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

        [HttpPut("Print/{receivedMarkPrintingId}")]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Result>> PrintReceivedMarkAsync(int receivedMarkPrintingId)
        {
            var result = await Mediator.Send(new PrintReceivedMarkCommand { ReceivedMarkPrintingId = receivedMarkPrintingId });
            return Ok(result);
        }
    }
}
