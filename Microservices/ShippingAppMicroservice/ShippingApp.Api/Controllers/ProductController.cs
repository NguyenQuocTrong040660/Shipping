using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Commands = ShippingApp.Application.Commands;
using Queries = ShippingApp.Application.Queries;
using Models = ShippingApp.Domain.Models;
using DTO = ShippingApp.Domain.DTO;

namespace ShippingApp.Api.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IWebHostEnvironment _environment;
        readonly ILogger<ProductController> _logger;
        public ProductController(IMediator mediator, IWebHostEnvironment environment, ILogger<ProductController> logger) : base(mediator)
        {
            _environment = environment;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        [Route("AddProducts")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(int), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(DTO.ProductDTO), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<bool>> AddProducts(DTO.ProductDTO productDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(productDTO);
            }
            var result = await _mediator.Send(new Commands.CreateProductCommand() { productDTO = productDTO });

            if (result == 0)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet]
        [Route("GetAllProducts")]
        public async Task<ActionResult<List<DTO.ProductDTO>>> GetAllProducts()
        {
            return await _mediator.Send(new Queries.GetAllProductQuery());
        }

        [HttpGet]
        [Route("GetProductsbyID/{id}")]
        [ProducesResponseType(typeof(DTO.ProductDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(DTO.ProductDTO), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DTO.ProductDTO>> GetProductByID(Guid id)
        {
            var result = await _mediator.Send(new Queries.GetProductByIDQuery() { Id = id });

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPut("UpdateProduct")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(int), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(DTO.ProductDTO), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<int>> UpdateProduct(DTO.ProductDTO productDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(productDTO);
            }

            var result = await _mediator.Send(new Commands.UpdateProductCommand() { productDTO = productDTO });

            if (result == 0)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpDelete]
        [Route("DeletedProduct/{id}")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(int), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<int>> DeleteProductOverview(Guid id)
        {
            var result = await _mediator.Send(new Commands.DeleteProductCommand() { Id = id });

            if (result == 0)
            {
                return NotFound();
            }

            return Ok(true);
        }

        [HttpPost("UploadImage"), DisableRequestSizeLimit]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public IActionResult UploadImage(IFormFile file)
        {
            const string folderToUploads = "images";

            if (!ModelState.IsValid)
            {
                return BadRequest(file);
            }

            if (file == null || file.Length == 0)
            {
                return BadRequest("File can not be null or empty");
            }

            if (file.Length > 2097152)
            {
                return BadRequest("File size is too much. Please choice another image");
            }

            try
            {
                var folderName = Path.Combine(_environment.WebRootPath, folderToUploads);

                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                Random random = new Random();

                string randomString = DateTime.Now.Day.ToString()
                    + "_" + DateTime.Now.Month.ToString()
                    + "_" + DateTime.Now.Year.ToString()
                    + DateTime.Now.Second.ToString()
                    + "_" + random.Next();

                string fileName = randomString + ContentDispositionHeaderValue.Parse(file.ContentDisposition)
                                .FileName
                                .Trim('"');

                string fullPath = Path.Combine(pathToSave, fileName);
                string dbPath = Path.Combine(folderName, fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                string domain = $"{HttpContext.Request.Scheme}{Uri.SchemeDelimiter}{Request.Host.ToUriComponent()}";

                return Ok(new
                {
                    imageName = fileName,
                    imageUrl = $"{domain}/{folderToUploads}/{fileName}",
                    folderName = folderName
                });
            }
            catch (Exception ex)
            {
                return BadRequest($"Internal server error: {ex.Message}");
            }
        }
    }
}
