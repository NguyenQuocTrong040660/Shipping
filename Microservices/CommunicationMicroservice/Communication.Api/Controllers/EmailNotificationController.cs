using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Communication.Application.Common.Results;
using Communication.Application.Email.Commands;
using Communication.Domain.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Communication.Api.Controllers
{
    public class EmailNotificationController : BaseController
    {
        readonly ILogger<EmailNotificationController> _logger;
        private readonly IWebHostEnvironment _environment;

        public EmailNotificationController(IMediator mediator,
            IWebHostEnvironment environment,
            ILogger<EmailNotificationController> logger) : base(mediator)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
        }

        [HttpPut("testing/{to}")]
        public async Task<ActionResult<Result>> TestSendEmail(string to, [FromBody] string content)
        {
            if (_environment.IsProduction())
            {
                return NoContent();
            }

            return Ok(await Mediator.Send(new SendEmailForTestingCommand
            {
                To = to,
                Content = content
            }));
        }

        [HttpPost("users")]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<CreateUserResult>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Result>> SendEmailRegistrationUsers([FromBody] List<CreateUserResult> users)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(users);
            }

            return Ok(await Mediator.Send(new SendRegistrationEmailCommand
            {
                Users = users
            }));
        }

        [HttpPost("forgot-password")]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<ResetPasswordResult>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Result>> SendEmailForgotPasswordAsync(
            [FromBody] List<ResetPasswordResult> resetPasswordResults)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(resetPasswordResults);
            }

            return Ok(await Mediator.Send(new SendForgetPasswordEmailCommand
            {
                ResetPasswordResults = resetPasswordResults
            }));
        }

        [HttpPost("shipping-request")]
        public async Task<ActionResult<Result>> SendEmailShippingRequestAsync()
        {
            return Ok();
        }
    }
}