using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ShippingApp.Application.Queries;
using ShippingApp.Domain.Models;

namespace ShippingApp.Api.Controllers
{
	public class CategoryController : BaseController
	{
		public CategoryController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        [Route("categories")]
        public async Task<ActionResult<List<ProductType>>> categories()
        {
            return await _mediator.Send(new GetCategoryQuery());
        }
    }
}

