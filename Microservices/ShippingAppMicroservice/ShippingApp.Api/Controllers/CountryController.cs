using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShippingApp.Application.Common.Results;
using ShippingApp.Application.Country.Commands;
using ShippingApp.Application.Country.Queries;
using ShippingApp.Domain.Models;

namespace ShippingApp.Api.Controllers
{
    public class CountryController : BaseController
    {
        public CountryController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<CountryModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<CountryModel>>> GetCoutriesAsync()
        {
            return await Mediator.Send(new GetCountryQuery());
        }
        
        [HttpPost]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(CountryModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Result>> AddCountryAsync([FromBody] CountryModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(model);
            }

            var result = await Mediator.Send(new CreateCountryCommand() { Country = model });
            return Ok(result);
        }

        [HttpPut("{countryCode}")]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(CountryModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Result>> UpdateCountryAsync(string countryCode, CountryModel entity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(entity);
            }

            var result = await Mediator.Send(new UpdateCountryCommand() { CountryCode = countryCode, Entity = entity });
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CountryModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CountryModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<CountryModel>> GetCountryByIdAsync(string countryCode)
        {
            var result = await Mediator.Send(new GetCountryByIdQuery() { CountryCode = countryCode });
            return Ok(result);
        }

        [HttpDelete("{countryCode}")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(int), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<int>> DeleteCountryAsync(string countryCode)
        {
            var result = await Mediator.Send(new DeleteCountryCommand() { CountryCode = countryCode });
            return Ok(result);
        }
    }
}
