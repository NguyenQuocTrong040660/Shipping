using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Commands = ShippingApp.Application.Commands;
using Queries = ShippingApp.Application.Queries;
using DTO = ShippingApp.Domain.DTO;

namespace ShippingApp.Api.Controllers
{
    public class ShippingPlanController : BaseController
    {
        private readonly IWebHostEnvironment _environment;
        readonly ILogger<ShippingPlanController> _logger;
        public ShippingPlanController(IMediator mediator, IWebHostEnvironment environment, ILogger<ShippingPlanController> logger) : base(mediator)
        {
            _environment = environment;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        [Route("AddShippingPlan")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(int), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(DTO.ShippingPlan), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<bool>> AddShippingPlan(DTO.ShippingPlan shippingPlan)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(shippingPlan);
            }
            var result = await _mediator.Send(new Commands.CreateNewShippingPLanCommand() { shippingPlan = shippingPlan });

            if (result == 0)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet]
        [Route("GetAllShippingPlan")]
        public async Task<ActionResult<List<DTO.ShippingPlan>>> GetAllShippingPlan()
        {
            return await _mediator.Send(new Queries.GetAllShippingPlanQuery());
        }

        [HttpGet]
        [Route("GetShippingPlanByID")]
        [ProducesResponseType(typeof(DTO.ShippingPlan), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(DTO.ShippingPlan), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DTO.ShippingPlan>> GetShippingPlanByID(Guid id)
        {
            var result = await _mediator.Send(new Queries.GetShippingPlanByIDQuery() { Id = id });

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPut("UpdateShippingPlan")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(int), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(DTO.ShippingPlan), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<int>> UpdateShippingPlan(DTO.ShippingPlan shippingPlan)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(shippingPlan);
            }

            var result = await _mediator.Send(new Commands.UpdateShippingPlanCommand() { shippingPlan = shippingPlan });

            if (result == 0)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpDelete]
        [Route("DeletedShippingPlan")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(int), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<int>> DeletedShippingPlan(Guid id)
        {
            var result = await _mediator.Send(new Commands.DeleteShippingPlanCommand() { Id = id });

            if (result == 0)
            {
                return NotFound();
            }

            return Ok(true);
        }
    }
}
