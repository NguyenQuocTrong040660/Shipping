using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Communication.Api.Controllers
{
    public class HealthController : BaseController
    {
        public HealthController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        public IActionResult GetHealth()
        {
            return Ok();
        }
    }
}
