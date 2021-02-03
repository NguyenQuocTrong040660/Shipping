using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ShippingApp.Application.Commands;
using ShippingApp.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using ShippingApp.Domain.DTO;
using Microsoft.Extensions.Logging;

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
        [Route("SendEmail")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<bool>> SendEmail([FromBody] EmailItemDTO emailInformationDto)
        {
            _logger.LogInformation("Start Send", DateTime.Now.ToString());
            var result = await _mediator.Send(new SendMailCommand { EmailInfoDto = emailInformationDto });
            _logger.LogInformation("End Send", DateTime.Now.ToString());
            return Ok(result);
        }
    }
}