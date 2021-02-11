using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShippingApp.Application.Common.Results;
using ShippingApp.Application.Config.Commands;
using ShippingApp.Application.Config.Queries;
using ShippingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShippingApp.Api.Controllers
{
    public class ConfigController : BaseController
    {
        readonly ILogger<ConfigController> _logger;
        public ConfigController(IMediator mediator, ILogger<ConfigController> logger) : base(mediator)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ConfigModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Result>> AddConfigAsync(ConfigModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(model);
            }

            var result = await Mediator.Send(new CreateConfigCommand { Config = model });
            return Ok(result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<ConfigModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<ConfigModel>>> GetConfigs()
        {
            return await Mediator.Send(new GetConfigsQuery { });
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ConfigModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ConfigModel>> GetConfigsByIdAsync(int id)
        {
            var result = await Mediator.Send(new GetConfigByIdQuery { Id = id });
            return Ok(result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ConfigModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Result>> UpdateConfigAsync(int id, [FromBody]ConfigModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(model);
            }

            var result = await Mediator.Send(new UpdateConfigCommand { Id = id, Config = model });
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Result>> DeleteConfigAysnc(int id)
        {
            var result = await Mediator.Send(new DeleteConfigCommand { Id = id });
            return Ok(result);
        }
    }
}
