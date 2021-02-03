using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using ShippingApp.Domain.Models;
using System.Collections.Generic;
using ShippingApp.Application.Queries;

namespace ShippingApp.Api.Controllers
{
    public class ProductSearchController: BaseController
    { 
        public ProductSearchController(IMediator mediator) : base(mediator)
        {
            
        }

        [HttpGet]
        [Route("GetProductCountries")]
        public async Task<ActionResult<List<Country>>> GetCountryList()
        {
            return await _mediator.Send(new GetCountryQuery());
        }



        [HttpGet]
        [Route("GetProductBrand")]
        public async Task<ActionResult<List<Brand>>> GetBrandList()
        {
            return await _mediator.Send(new GetProductBrandQuery());
        }
    }
}
