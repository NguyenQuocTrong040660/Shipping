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
using ShippingApp.Application.Commands;
using ShippingApp.Application.Queries;
using ShippingApp.Domain.Models;

namespace ShippingApp.Api.Controllers
{
    public class MemberShipController : BaseController
    {
        private readonly IWebHostEnvironment _environment;
        readonly ILogger<MemberShipController> _logger;
        public MemberShipController(IMediator mediator, IWebHostEnvironment environment, ILogger<MemberShipController> logger) : base(mediator)
        {
            _environment = environment;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        [Route("AddMemberShip")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(int), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(MemberShip), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<bool>> AddMemberShip(MemberShip model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(model);
            }
            var result = await _mediator.Send(new CreateMemberShipCommand() { Model = model });

            if (result == 0)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet]
        [Route("GetAllMemberShip")]
        public async Task<ActionResult<List<MemberShip>>> GetAllMemberShip()
        {
            //return await _mediator.Send(new GetProductQuery());
            //var result = await _mediator.Send(new GetProductQuery() { companyIndex = companyIndex });

            return await _mediator.Send(new GetMemberShipQuery());
        }

        [HttpGet]
        [Route("GetMemberShipById/{id}")]
        [ProducesResponseType(typeof(MemberShip), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MemberShip), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MemberShip>> GetMemberShipById(Guid id)
        {
            var result = await _mediator.Send(new GetMemberShipByIdQuery() { Id = id });

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPut]
        [Route("UpdateMemberShip/{id}")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(int), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(MemberShip), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<int>> UpdateMemberShip(Guid id, MemberShip entity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(entity);
            }

            var result = await _mediator.Send(new UpdateMemberShipCommand() { Id = id, Entity = entity });

            if (result == 0)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpDelete]
        [Route("DeletedMemberShip/{id}")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(int), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<int>> DeletedMemberShip(Guid id)
        {
            var result = await _mediator.Send(new DeleteMemberShipCommand() { Id = id });

            if (result == 0)
            {
                return NotFound();
            }

            return Ok(true);
        }
    }
}
