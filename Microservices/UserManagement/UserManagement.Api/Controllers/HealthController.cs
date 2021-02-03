using Microsoft.AspNetCore.Mvc;

namespace UserManagement.Api.Controllers
{
    public class HealthController : ApiController
    {
        [HttpGet]
        public ActionResult Health()
        {
            return Ok();
        }
    }
}
