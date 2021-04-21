using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShippingApp.Application.Common.Results;
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

        [HttpPost("BulkInsert")]
        [ProducesResponseType(typeof(List<ProductModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<ProductModel>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(List<ProductModel>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<ProductModel>>> AddProducts(List<ProductModel> products)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(products);
            }

            var invalidProducts = new List<ProductModel>();

            foreach (var product in products)
            {
                var result = await Mediator.Send(new CreateProductCommand { Product = product });

                if (!result.Succeeded)
                {
                    invalidProducts.Add(product);
                }
            }
           
            return Ok(invalidProducts);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProductModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Result>> AddProducts(ProductModel product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(product);
            }

            return Ok(await Mediator.Send(new CreateProductCommand { Product = product }));
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<ProductModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<ProductModel>>> GetProducts()
        {
            return Ok(await Mediator.Send(new GetAllProductQuery { }));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProductModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ProductModel>> GetProductByIdAsync(int id)
        {
            return Ok(await Mediator.Send(new GetProductByIdQuery { Id = id }));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProductModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Result>> UpdateProductAsync(int id, [FromBody] ProductModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(model);
            }

            return Ok(await Mediator.Send(new UpdateProductCommand { Id = id, Product = model }));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Result>> DeleteProductAysnc(int id)
        {
            return Ok(await Mediator.Send(new DeleteProductCommand { Id = id }));
        }

        [HttpPost("VerifyProduct")]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<string>>> VerifyProductAsync([FromBody] List<string> productNumbers)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(productNumbers);
            }

            var existProduct = new List<string>();

            foreach (var item in productNumbers)
            {
                var product = await Mediator.Send(new GetProductByProductNumberQuery { ProductNumber = item });

                if (product == null)
                {
                    continue;
                }

                existProduct.Add(item);
            }

            return Ok(existProduct);
        }

        [HttpPost("VerifyImportProduct")]
        [ProducesResponseType(typeof(List<ImportResult>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<ImportResult>>> VerifyImportProductAsync([FromBody] List<ProductModel> products)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(products);
            }

            var importResults = new List<ImportResult>();
            var validator = new ProductValidator();

            foreach (var item in products)
            {
                var product = await Mediator.Send(new GetProductByProductNumberQuery { ProductNumber = item.ProductNumber });

                if (product == null)
                {
                    var validateResult = validator.Validate(item);

                    if (!validateResult.IsValid)
                    {
                        importResults.Add(ImportResult.Failure(validateResult.Errors.Select(x => x.ErrorMessage).ToList(), item.ProductNumber, item));
                    }
                    else
                    {
                        importResults.Add(ImportResult.Success(item.ProductNumber, item));
                    }
                }
                else
                {
                    importResults.Add(ImportResult.Failure(new List<string> { "Product has been issued" }, item.ProductNumber, item));
                }
            }

            return Ok(importResults);
        }
    }
}
