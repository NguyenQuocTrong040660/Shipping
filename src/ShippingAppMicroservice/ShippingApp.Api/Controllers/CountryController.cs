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
    public class CountryController : BaseController
    {
        public CountryController(IMediator mediator) : base(mediator)
        {

        }

        [HttpGet]
        [Route("GetAllCountry")]
        public async Task<ActionResult<List<Country>>> GetAllCountry()
        {
            return await _mediator.Send(new GetCountryQuery());
        }

        [HttpPost]
        [Route("AddCountry")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(int), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Country), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<int>> InsertCountry(Country model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(model);
            }

            var result = await _mediator.Send(new CreateCountryCommand() { Model = model });

            if (result == 0)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPut("UpdateCountry/{countryCode}")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(int), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Country), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<int>> PutCountry(string countryCode, Country entity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(entity);
            }

            var result = await _mediator.Send(new UpdateCountryCommand() { CountryCode = countryCode, Entity = entity });

            if (result == 0)
            {
                return NotFound();
            }

            return Ok(result);
        }


        [HttpGet]
        [Route("GetCountryById/{id}")]
        [ProducesResponseType(typeof(Country), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Country), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Country>> GetCountryById(string id)
        {
            var result = await _mediator.Send(new GetCountryByIdQuery() { CountryCode = id });

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }


        [HttpDelete("DeleteCountry/{countryCode}")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(int), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<int>> DeleteCountry(string countryCode)
        {
            var result = await _mediator.Send(new DeleteCountryCommand() { CountryCode = countryCode });

            if (result == 0)
            {
                return NotFound();
            }

            return Ok(result);
        }
    }
}
