using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Files.Api.Controllers
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
