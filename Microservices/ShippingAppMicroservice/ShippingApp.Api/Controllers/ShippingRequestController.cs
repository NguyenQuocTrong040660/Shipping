﻿using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShippingApp.Application.Common.Results;
using ShippingApp.Application.ShippingRequest.Commands;
using ShippingApp.Application.ShippingRequest.Queries;
using ShippingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShippingApp.Api.Controllers
{
    public class ShippingRequestController : BaseController
    {
        readonly ILogger<ShippingRequestController> _logger;
        public ShippingRequestController(IMediator mediator, ILogger<ShippingRequestController> logger) : base(mediator)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ShippingRequestModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Result>> AddShippingRequestAsync(ShippingRequestModel shippingRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(shippingRequest);
            }

            var result = await Mediator.Send(new CreateShippingRequestCommand { ShippingRequest = shippingRequest });
            return Ok(result);
        }

        [HttpPut("{movementRequestId}")]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ShippingRequestLogisticModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Result>> AddShippingRequestLogistics(int movementRequestId, [FromBody] ShippingRequestLogisticModel shippingRequestLogistic)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(shippingRequestLogistic);
            }

            var result = await Mediator.Send(new CreateShippingRequestLogisticCommand 
            { 
                MovementRequestId = movementRequestId,
                ShippingRequestLogistic = shippingRequestLogistic
            });

            return Ok(result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<ShippingRequestModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<ShippingRequestModel>>> GetShippingRequests()
        {
            return await Mediator.Send(new GetShippingRequestsQuery { });
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ShippingRequestModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ShippingRequestModel>> GetShippingRequestByIdAsync(int id)
        {
            var result = await Mediator.Send(new GetShippingRequestByIdQuery { Id = id });
            return Ok(result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ShippingRequestModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Result>> UpdateShippingRequestAsync(int id, [FromBody] ShippingRequestModel shippingRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(shippingRequest);
            }

            var result = await Mediator.Send(new UpdateShippingRequestCommand { Id = id, ShippingRequest = shippingRequest });
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Result>> DeleteShippingRequestAysnc(int id)
        {
            var result = await Mediator.Send(new DeleteShippingRequestCommand { Id = id });
            return Ok(result);
        }
    }
}
