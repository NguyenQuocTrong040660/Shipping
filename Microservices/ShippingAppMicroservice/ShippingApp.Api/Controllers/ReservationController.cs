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
using ShippingApp.Application.Commands;
using ShippingApp.Application.Queries;
//using ShippingApp.Domain.Entities;
using ShippingApp.Domain.Models;

namespace ShippingApp.Api.Controllers
{
    public class ReservationController : BaseController
    {
        private readonly IWebHostEnvironment _environment;
        readonly ILogger<ReservationController> _logger;
        public ReservationController(IMediator mediator, IWebHostEnvironment environment, ILogger<ReservationController> logger) : base(mediator)
        {
            _environment = environment;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        [Route("AddReservation")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(int), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Reservation), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<bool>> AddReservation(Reservation model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(model);
            }
            var result = await _mediator.Send(new CreateReservationCommand() { Model = model });

            if (result == 0)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet]
        [Route("GetAllReservation")]
        public async Task<ActionResult<List<Reservation>>> GetAllReservation()
        {
            //return await _mediator.Send(new GetProductQuery());
            //var result = await _mediator.Send(new GetProductQuery() { companyIndex = companyIndex });

            return await _mediator.Send(new GetReservationQuery());
        }

        [HttpGet]
        [Route("GetReservationById/{id}")]
        [ProducesResponseType(typeof(Reservation), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Reservation), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Reservation>> GetReservationById(Guid id)
        {
            var result = await _mediator.Send(new GetReservationByIdQuery() { ReservationId = id });

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        //[HttpGet]
        //[Route("GetAllProductsHightLight")]
        //public async Task<ActionResult<List<ProductOverview>>> GetAllProductsHightLight()
        //{
        //    //return await _mediator.Send(new GetProductQuery());
        //    //var result = await _mediator.Send(new GetProductQuery() { companyIndex = companyIndex });

        //    return await _mediator.Send(new GetProductHightLightQuery());
        //}



        //[HttpGet]
        //[Route("GetProductsbyID/{id}")]
        //[ProducesResponseType(typeof(ProductOverview), StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(ProductOverview), StatusCodes.Status404NotFound)]
        //public async Task<ActionResult<ProductOverview>> GetProductByID(Guid id)
        //{
        //    var result = await _mediator.Send(new GetProductByIDQuery() { Id = id});

        //    if (result == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(result);
        //}

        //[HttpGet]
        //[Route("GetProductsbyCountries")]
        //public async Task<ActionResult<List<ProductOverview>>> GetProductsbyCountries(string CountryCode)
        //{
        //    return await _mediator.Send(new GetProductByCountriesQuery(CountryCode));
        //}

        [HttpPut]
        [Route("UpdateReservation/{id}")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(int), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Reservation), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<int>> UpdateReservation(Guid id, Reservation entity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(entity);
            }

            var result = await _mediator.Send(new UpdateReservationCommand() { Id = id, Entity = entity });

            if (result == 0)
            {
                return NotFound();
            }

            return Ok(result);
        }



        [HttpDelete]
        [Route("DeletedReservation/{id}")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(int), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<int>> DeletedReservation(Guid id)
        {
            var result = await _mediator.Send(new DeleteReservationCommand() { Id = id });

            if (result == 0)
            {
                return NotFound();
            }

            return Ok(true);
        }

        //[HttpPost]
        //[Route("CheckoutProduct")]
        //[ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(int), StatusCodes.Status404NotFound)]
        //[ProducesResponseType(typeof(ProductOverview), StatusCodes.Status400BadRequest)]
        //public async Task<ActionResult<bool>> CheckoutProduct(Order Order)
        //{
        //    //_logger.LogInformation("Start Send", DateTime.Now.ToString());
        //    //var result = await _mediator.Send(new SendMailCommand { CartDto = cartModel });
        //    //_logger.LogInformation("End Send", DateTime.Now.ToString());

        //    //var result = await _mediator.Send(new CreateCustomerCommand() { Customer = Order.Customer });

        //    //if (result == 0)
        //    //{
        //    //    return NotFound();
        //    //}

        //    var result = await _mediator.Send(new CreateOrderCommand() { Order = Order });

        //    if (result == 0)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(result);
        //}

        //[HttpPost("UploadImage"), DisableRequestSizeLimit]
        //[ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        //public IActionResult UploadImage(IFormFile file)
        //{
        //    const string folderToUploads = "images";

        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(file);
        //    }

        //    if (file == null || file.Length == 0)
        //    {
        //        return BadRequest("File can not be null or empty");
        //    }

        //    if (file.Length > 2097152)
        //    {
        //        return BadRequest("File size is too much. Please choice another image");
        //    }

        //    try
        //    {
        //        var folderName = Path.Combine(_environment.WebRootPath, folderToUploads);

        //        var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

        //        Random random = new Random();

        //        string randomString = DateTime.Now.Day.ToString()
        //            + "_" + DateTime.Now.Month.ToString()
        //            + "_" + DateTime.Now.Year.ToString()
        //            + DateTime.Now.Second.ToString()
        //            + "_" + random.Next();

        //        string fileName = randomString + ContentDispositionHeaderValue.Parse(file.ContentDisposition)
        //                        .FileName
        //                        .Trim('"');

        //        string fullPath = Path.Combine(pathToSave, fileName);
        //        string dbPath = Path.Combine(folderName, fileName);

        //        using (var stream = new FileStream(fullPath, FileMode.Create))
        //        {
        //            file.CopyTo(stream);
        //        }

        //        string domain = $"{HttpContext.Request.Scheme}{Uri.SchemeDelimiter}{Request.Host.ToUriComponent()}";

        //        return Ok(new
        //        {
        //            imageName = fileName,
        //            imageUrl = $"{domain}/{folderToUploads}/{fileName}",
        //            folderName = folderName
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest($"Internal server error: {ex.Message}");
        //    }
        //}
    }
}
