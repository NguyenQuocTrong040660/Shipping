using MediatR;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles = "SystemAdministrator,ITAdministrator")]
    public class ConfigController : BaseController
    {
        readonly ILogger<ConfigController> _logger;
        public ConfigController(IMediator mediator, ILogger<ConfigController> logger) : base(mediator)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<ConfigModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<ConfigModel>>> GetConfigs()
        {
            return Ok(await Mediator.Send(new GetConfigsQuery { }));
        }

        [HttpGet("{key}")]
        [ProducesResponseType(typeof(ConfigModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ConfigModel>> GetConfigsByKeyAsync(string key)
        {
            return Ok(await Mediator.Send(new GetConfigByKeyQuery { Key = key }));
        }

        [HttpPut("{key}")]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ConfigModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Result>> UpdateConfigAsync(string key, [FromBody]ConfigModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(model);
            }

            return Ok(await Mediator.Send(new UpdateConfigCommand { Key = key, Config = model }));
        }
    }
}
