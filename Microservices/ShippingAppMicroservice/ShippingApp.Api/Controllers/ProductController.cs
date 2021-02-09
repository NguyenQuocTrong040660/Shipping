using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShippingApp.Application.Product.Commands;
using ShippingApp.Application.Product.Queries;
using ShippingApp.Domain.Models;

namespace ShippingApp.Api.Controllers
{
    public class ProductController : BaseController
    {
        readonly ILogger<ProductController> _logger;
        public ProductController(IMediator mediator, ILogger<ProductController> logger) : base(mediator)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(int), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProductModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<bool>> AddProducts(ProductModel product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(product);
            }

            var result = await Mediator.Send(new CreateProductCommand { Product = product });
            return Ok(result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<ProductModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<ProductModel>>> GetProducts()
        {
            return await Mediator.Send(new GetAllProductQuery());
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProductModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ProductModel>> GetProductByIdAsync(Guid id)
        {
            var result = await Mediator.Send(new GetProductByIdQuery { Id = id });
            return Ok(result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(int), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProductModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<int>> UpdateProductAsync(Guid id, [FromBody] ProductModel productDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(productDTO);
            }

            var result = await Mediator.Send(new UpdateProductCommand { Id = id, Product = productDTO });
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(int), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<int>> DeleteProductAysnc(Guid id)
        {
            var result = await Mediator.Send(new DeleteProductCommand { Id = id });
            return Ok(result);
        }
    }
}
