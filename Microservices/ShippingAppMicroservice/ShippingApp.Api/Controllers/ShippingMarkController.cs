using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShippingApp.Application.Common.Results;
using ShippingApp.Application.ShippingMark.Commands;
using ShippingApp.Application.ShippingMark.Queries;
using ShippingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShippingApp.Api.Controllers
{
    public class ShippingMarkController : BaseController
    {
        readonly ILogger<ShippingMarkController> _logger;
        public ShippingMarkController(IMediator mediator, ILogger<ShippingMarkController> logger) : base(mediator)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ShippingMarkModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Result>> AddShippingMarkAsync(ShippingMarkModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(model);
            }

            var result = await Mediator.Send(new CreateShippingMarkCommand { ShippingMark = model });
            return Ok(result);
        }

        [HttpPut("Generate/{shippingRequestId}")]
        [ProducesResponseType(typeof(List<ShippingMarkModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<ShippingMarkModel>>> GenerateShippingMarkAsync(int shippingRequestId)
        {
            var result = await Mediator.Send(new GenerateShippingMarkCommand { ShippingRequestId = shippingRequestId });
            return Ok(result);
        }

        [HttpGet("ShippingRequest/{shippingRequestId}")]
        [ProducesResponseType(typeof(List<ReceivedMarkModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<ShippingMarkModel>>> GetShippingMarksByShippingRequestId(int shippingRequestId)
        {
            return await Mediator.Send(new GetShippingMarkByShippingRequestIdQuery { ShippingRequestId = shippingRequestId });
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<ShippingMarkModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<ShippingMarkModel>>> GetShippingMarks()
        {
            return await Mediator.Send(new GetShippingMarksQuery { });
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ShippingMarkModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ShippingMarkModel>> GetShippingMarkByIdAsync(int id)
        {
            var result = await Mediator.Send(new GetShippingMarkByIdQuery { Id = id });
            return Ok(result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ShippingMarkModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Result>> UpdateShippingRequestAsync(int id, [FromBody]ShippingMarkModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(model);
            }

            var result = await Mediator.Send(new UpdateShippingMarkCommand { Id = id, ShippingMark = model });
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Result>> DeleteShippingMarkAysnc(int id)
        {
            var result = await Mediator.Send(new DeleteShippingMarkCommand { Id = id });
            return Ok(result);
        }

        [HttpPut("Print/{id}")]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Result>> PrintShippingMarkAsync(int id)
        {
            var result = await Mediator.Send(new PrintShippingMarkCommand { Id = id });
            return Ok(result);
        }
    }
}
