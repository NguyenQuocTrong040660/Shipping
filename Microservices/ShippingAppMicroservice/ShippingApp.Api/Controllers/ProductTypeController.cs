using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShippingApp.Application.Commands;
using ShippingApp.Application.Queries;
using ShippingApp.Domain.Interfaces;
using ShippingApp.Domain.Models;

namespace ShippingApp.Api.Controllers
{
    public class ProductTypeController : BaseController
    {
        public ProductTypeController(IMediator mediator) : base(mediator)
        {
            
        }

        [HttpGet]
        [Route("GetAllProductType/{companyIndex}")]
        public async Task<ActionResult<List<ProductType>>> GetAllProductType(int companyIndex)
        {
            return await _mediator.Send(new GetProductTypeQuery() { companyIndex = companyIndex });
        }

        //[HttpGet]
        //[Route("GetProductTypeById/{id}")]
        //[ProducesResponseType(typeof(ProductType), StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(ProductType), StatusCodes.Status404NotFound)]
        //public async Task<ActionResult<ProductType>> GetProductTypeById(Guid id)
        //{
        //    var result = await _mediator.Send(new GetProductTypeByNameQuery() { ProductTypeCode = id });

        //    if (result == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(result);
        //}

        [HttpPost]
        [Route("ProductType")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(int), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProductType), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<int>> InsertProductType(ProductType model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(model);
            }

            var result = await _mediator.Send(new CreateProductTypeCommand() { Model = model });

            if (result == 0)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet]
        [Route("ProductType/{productTypeName}")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(int), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProductType), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<int>> GetProductTypeByName(string productTypeName)
        {
          
            var result = await _mediator.Send(new GetProductTypeByNameQuery() { ProductTypeName = productTypeName});

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }
          
        [HttpPut]
        [Route("UpdateProductType/{productTypeCode}")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(int), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProductType), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<int>> PutProductType(Guid productTypeCode, ProductType entity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(entity);
            }

            var result = await _mediator.Send(new UpdateProductTypeCommand() { ProductTypeCode = productTypeCode, Entity = entity });

            if (result == 0)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpDelete]
        [Route("DeleteProductType/{productTypeCode}")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(int), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<int>> DeleteProductType(Guid productTypeCode)
        {
            var result = await _mediator.Send(new DeleteProductTypeCommand() { ProductTypeCode = productTypeCode });

            if (result == 0)
            {
                return NotFound();
            }

            return Ok(result);
        }


    }
}
