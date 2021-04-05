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

            return Ok(await Mediator.Send(new CreateShippingMarkCommand { ShippingMark = model }));
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<ShippingMarkModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<ShippingMarkModel>>> GetShippingMarks()
        {
            return Ok(await Mediator.Send(new GetShippingMarksQuery { }));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ShippingMarkModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ShippingMarkModel>> GetShippingMarkByIdAsync(int id)
        {
            return Ok(await Mediator.Send(new GetShippingMarkByIdQuery
            {
                Id = id
            }));
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

            return Ok(await Mediator.Send(new UpdateShippingMarkCommand
            {
                Id = id,
                ShippingMark = model
            }));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Result>> DeleteShippingMarkAysnc(int id)
        {
            return Ok(await Mediator.Send(new DeleteShippingMarkCommand
            {
                Id = id
            }));
        }

        [HttpPost("Print")]
        [ProducesResponseType(typeof(ShippingMarkPrintingModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ShippingMarkPrintingModel), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(PrintShippingMarkRequest), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ShippingMarkPrintingModel>> PrintShippingMarkAsync([FromBody] PrintShippingMarkRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(request);
            }

            var result = await Mediator.Send(new PrintShippingMarkCommand
            {
                PrintShippingMarkRequest = request
            });

            if (result == null)
            {
                return NoContent();
            }

            return Ok(result);
        }

        [HttpPost("RePrint")]
        [ProducesResponseType(typeof(ShippingMarkPrintingModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ShippingMarkPrintingModel), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(RePrintShippingMarkRequest), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ShippingMarkPrintingModel>> RePrintShippingMarkAsync([FromBody] RePrintShippingMarkRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(request);
            }

            var result = await Mediator.Send(new RePrintShippingMarkCommand
            {
                RePrintShippingMarkRequest = request
            });

            if (result == null)
            {
                return NoContent();
            }

            return Ok(result);
        }

        [HttpPost("Generate/ShippingMarkShippings")]
        [ProducesResponseType(typeof(List<ShippingMarkShippingModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ShippingRequestModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<ShippingMarkShippingModel>>> GenerateShippingMarkShippings([FromBody] ShippingRequestModel shippingRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(shippingRequest);
            }

            return Ok(await Mediator.Send(new GenerateShippingMarkShippingByShippingRequestQuery
            {
                ShippingRequest = shippingRequest
            }));
        }

        [HttpGet("ShippingMarkSummaries/{shippingMarkId}")]
        [ProducesResponseType(typeof(List<ShippingMarkSummaryModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<ShippingMarkSummaryModel>>> GetShippingMarkSummariesAsync(int shippingMarkId)
        {
            return Ok(await Mediator.Send(new GetShippingMarkSummariesByIdQuery
            {
                ShippingMarkId = shippingMarkId
            }));
        }

        [HttpGet("ShippingMarkPrintings/{shippingMarkId}/{productId}")]
        [ProducesResponseType(typeof(List<ShippingMarkPrintingModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<ShippingMarkPrintingModel>>> GetShippingMarkPrintingsAsync(
            int shippingMarkId, int productId)
        {
            return Ok(await Mediator.Send(new GetShippingPrintingsByIdQuery
            {
                ShippingMarkId = shippingMarkId,
                ProductId = productId
            }));
        }
    }
}
