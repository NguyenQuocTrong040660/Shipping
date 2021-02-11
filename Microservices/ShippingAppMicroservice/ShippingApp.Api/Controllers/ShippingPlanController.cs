using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShippingApp.Application.Common.Results;
using ShippingApp.Application.ShippingPlan.Commands;
using ShippingApp.Application.ShippingPlan.Queries;
using ShippingApp.Domain.Models;

namespace ShippingApp.Api.Controllers
{
    public class ShippingPlanController : BaseController
    {
        readonly ILogger<ShippingPlanController> _logger;
        public ShippingPlanController(IMediator mediator, ILogger<ShippingPlanController> logger) : base(mediator)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ShippingPlanModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<bool>> AddShippingPlanAsync([FromBody] ShippingPlanModel shippingPlan)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(shippingPlan);
            }

            var result = await Mediator.Send(new CreateNewShippingPLanCommand() { ShippingPlan = shippingPlan });
            return Ok(result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<ShippingPlanModel>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<ShippingPlanModel>>> GetAllShippingPlanAsync()
        {
            return await Mediator.Send(new GetAllShippingPlanQuery());
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ShippingPlanModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ShippingPlanModel>> GetShippingPlanByIdAsync(int id)
        {
            var result = await Mediator.Send(new GetShippingPlanByIDQuery { Id = id });
            return Ok(result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ShippingPlanModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Result>> UpdateShippingPlanAsync(int id, [FromBody] ShippingPlanModel shippingPlan)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(shippingPlan);
            }

            var result = await Mediator.Send(new UpdateShippingPlanCommand { Id = id, ShippingPlan = shippingPlan });
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<int>> DeletedShippingPlanAsync(int id)
        {
            var result = await Mediator.Send(new DeleteShippingPlanCommand { Id = id });
            return Ok(result);
        }
    }
}
