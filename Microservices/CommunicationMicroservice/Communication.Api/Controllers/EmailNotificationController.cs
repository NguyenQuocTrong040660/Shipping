using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Communication.Application.Email.Commands;
using Communication.Application.Common.Results;
using Communication.Domain.Models;

namespace Communication.Api.Controllers
{
    public class EmailNotificationController : BaseController
    {
        readonly ILogger<EmailNotificationController> _logger;

        public EmailNotificationController(IMediator mediator, ILogger<EmailNotificationController> logger) : base(mediator)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(EmailModel), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Result>> SendEmail([FromBody] EmailModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(request);
            }

            _logger.LogInformation("Start Send", DateTime.Now.ToString());

            var result = await Mediator.Send(new SendMailCommand { Email = request });

            _logger.LogInformation("End Send", DateTime.Now.ToString());

            return Ok(result);
        }
    }
}