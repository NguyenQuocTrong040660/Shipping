using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShippingApp.Domain.DTO;
using Microsoft.Extensions.Logging;
using ShippingApp.Application.Email.Commands;

namespace ShippingApp.Api.Controllers
{
    public class EmailNotificationController : BaseController
    {
        readonly ILogger<EmailNotificationController> _logger;

        public EmailNotificationController(IMediator mediator, ILogger<EmailNotificationController> logger) : base(mediator)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> SendEmail([FromBody] EmailItemDTO emailInformationDto)
        {
            _logger.LogInformation("Start Send", DateTime.Now.ToString());

            var result = await Mediator.Send(new SendMailCommand { EmailInfoDto = emailInformationDto });

            _logger.LogInformation("End Send", DateTime.Now.ToString());

            return Ok(result);
        }
    }
}