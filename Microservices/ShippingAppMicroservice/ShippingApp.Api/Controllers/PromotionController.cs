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
    public class PromotionController : BaseController
    {
        private readonly IWebHostEnvironment _environment;
        readonly ILogger<PromotionController> _logger;
        public PromotionController(IMediator mediator, IWebHostEnvironment environment, ILogger<PromotionController> logger) : base(mediator)
        {
            _environment = environment;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        [Route("AddPromotion")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(int), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Promotion), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<bool>> AddPromotion(Promotion model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(model);
            }
            var result = await _mediator.Send(new CreatePromotionCommand() { Model = model });

            if (result == 0)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet]
        [Route("GetAllPromotion")]
        public async Task<ActionResult<List<Promotion>>> GetAllPromotion()
        {
            //return await _mediator.Send(new GetProductQuery());
            //var result = await _mediator.Send(new GetProductQuery() { companyIndex = companyIndex });

            return await _mediator.Send(new GetPromotionQuery());
        }

        [HttpGet]
        [Route("GetPromotionById/{id}")]
        [ProducesResponseType(typeof(Promotion), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Promotion), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Promotion>> GetPromotionById(Guid id)
        {
            var result = await _mediator.Send(new GetPromotionByIdQuery() { Id = id });

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPut]
        [Route("UpdatePromotion/{id}")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(int), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Promotion), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<int>> UpdatePromotion(Guid id, Promotion entity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(entity);
            }

            var result = await _mediator.Send(new UpdatePromotionCommand() { Id = id, Entity = entity });

            if (result == 0)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpDelete]
        [Route("DeletedPromotion/{id}")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(int), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<int>> DeletedPromotion(Guid id)
        {
            var result = await _mediator.Send(new DeletePromotionCommand() { Id = id });

            if (result == 0)
            {
                return NotFound();
            }

            return Ok(true);
        }
    }
}
