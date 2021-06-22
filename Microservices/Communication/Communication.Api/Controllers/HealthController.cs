using MediatR;
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
