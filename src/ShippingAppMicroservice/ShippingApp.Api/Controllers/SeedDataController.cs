using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShippingApp.Migration;

namespace ShippingApp.Api.Controllers
{
    ////Should be disabled in PROD
    public class SeedDataController : BaseController
    {
        private readonly ISeedShippingApp _seedShippingApp;
        public SeedDataController(IMediator mediator, ISeedShippingApp seedProduct) : base(mediator)
        {
            _seedShippingApp = seedProduct;
        }

        [HttpPost]
        [Route("SeedData")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<bool>> SeedData()
        {
            _seedShippingApp.SeedData();
            return Ok(true);
        }
    }
}
