using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShippingApp.Application.Commands;
using ShippingApp.Application.Queries;
using ShippingApp.Domain.Interfaces;
using ShippingApp.Domain.Models;

namespace ShippingApp.Api.Controllers
{
    public class BrandController : BaseController
    {
        public BrandController(IMediator mediator) : base(mediator)
        {

        }

        [HttpGet]
        [Route("GetAllBrand/{companyIndex}")]
        public async Task<ActionResult<List<Brand>>> GetAllBrand(int companyIndex)
        {
            return await _mediator.Send(new GetBrandQuery() { companyIndex = companyIndex});
        }

        [HttpPost]
        [Route("AddBrand")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(int), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Brand), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<int>> InsertBrand(Brand model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(model);
            }

            var result = await _mediator.Send(new CreateBrandCommand() { Model = model });

            if (result == 0)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPut("UpdateBrand/{brandCode}")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(int), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Brand), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<int>> PutBrand(Guid brandCode, Brand entity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(entity);
            }

            var result = await _mediator.Send(new UpdateBrandCommand() { Id = brandCode, Entity = entity });

            if (result == 0)
            {
                return NotFound();
            }

            return Ok(result);
        }


        [HttpDelete("DeleteBrand/{brandCode}")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(int), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<int>> DeleteBrand(Guid brandCode)
        {
            var result = await _mediator.Send(new DeleteBrandCommand() { Id = brandCode });

            if (result == 0)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet]
        [Route("GetBrandById/{id}")]
        [ProducesResponseType(typeof(Brand), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Brand), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Brand>> GetBrandById(Guid id)
        {
            var result = await _mediator.Send(new GetBrandByIdQuery() { BrandCode = id });

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }
    }
}
