using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShippingApp.Application.Common.Results;
using ShippingApp.Application.Product.Queries;
using ShippingApp.Application.ShippingPlan.Commands;
using ShippingApp.Application.ShippingPlan.Queries;
using ShippingApp.Domain.Models;

namespace ShippingApp.Api.Controllers
{
    public class ShippingPlanController : BaseController
    {
        readonly ILogger<ShippingPlanController> _logger;

        public ShippingPlanController(IMediator mediator, ILogger<ShippingPlanController> logger) : base(mediator)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost("BulkInsert")]
        [ProducesResponseType(typeof(List<ShippingPlanImportModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<ShippingPlanImportModel>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<ShippingPlanImportModel>>> BulkInsertShippingPlanAsync(
            [FromBody] List<ShippingPlanImportModel> shippingPlanImports)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(shippingPlanImports);
            }

            var invalidDatas = new List<ShippingPlanImportModel>();

            var groupShippingPlans = shippingPlanImports.GroupBy(x => x.ShippingPlanId)
                                                  .Select(g => new
                                                  {
                                                      ShippingPlanId = g.Key,
                                                      ShippingPlans = g.ToList()
                                                  });

            foreach (var group in groupShippingPlans)
            {
                var item = shippingPlanImports.FirstOrDefault(i => i.ShippingPlanId == group.ShippingPlanId);

                var shippingPlan = new ShippingPlanModel
                {
                    Notes = item?.Notes ?? string.Empty,
                    CustomerName = item?.CustomerName ?? string.Empty,
                    SalesID = item.SalesID,
                    ShippingDate = item.ShippingDate,
                    PurchaseOrder = item.PurchaseOrder,
                    RefId = item.ShippingPlanId,
                    SemlineNumber = item.SemlineNumber,
                };

                var shippingPlanDetails = new List<ShippingPlanDetailModel>();

                var groupByProduct = group.ShippingPlans
                                                  .GroupBy(x => x.ProductNumber)
                                                  .Select(g => new
                                                  {
                                                      ProductNumber = g.Key,
                                                      Quantity = g.Sum(x => x.QuantityOrder),
                                                      SalesPrice = g.FirstOrDefault(x => x.ProductNumber == g.Key).SalesPrice,
                                                      ShippingMode = g.FirstOrDefault(x => x.ProductNumber == g.Key).ShippingMode,
                                                      ShippingPlans = g.ToList()
                                                  });

                foreach (var product in groupByProduct)
                {
                    var productDatabase = await Mediator.Send(new GetProductByProductNumberQuery 
                    { 
                        ProductNumber = product.ProductNumber 
                    });

                    if (productDatabase == null)
                    {
                        invalidDatas.AddRange(product.ShippingPlans);
                        continue;
                    }

                    if (product.Quantity * product.SalesPrice < 0)
                    {
                        invalidDatas.AddRange(product.ShippingPlans);
                        continue;
                    }

                    shippingPlanDetails.Add(new ShippingPlanDetailModel
                    {
                        ProductId = productDatabase.Id,
                        Quantity = product.Quantity,
                        ShippingPlanId = shippingPlan.Id,
                        Price = product.SalesPrice,
                        Amount = product.Quantity * product.SalesPrice,
                        ShippingMode = product.ShippingMode
                    });
                }

                shippingPlan.ShippingPlanDetails = shippingPlanDetails;

                var result = await Mediator.Send(new CreateNewShippingPLanCommand { ShippingPlan = shippingPlan });

                if (!result.Succeeded)
                {
                    invalidDatas.AddRange(group.ShippingPlans);
                }
            }

            return Ok(invalidDatas.Distinct());
        }


        [HttpPost]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ShippingPlanModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<bool>> AddShippingPlanAsync([FromBody] ShippingPlanModel shippingPlan)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(shippingPlan);
            }

            var result = await Mediator.Send(new CreateNewShippingPLanCommand() { ShippingPlan = shippingPlan });
            return Ok(result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<ShippingPlanModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<ShippingPlanModel>>> GetAllShippingPlanAsync()
        {
            return await Mediator.Send(new GetAllShippingPlanQuery());
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ShippingPlanModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ShippingPlanModel>> GetShippingPlanByIdAsync(int id)
        {
            var result = await Mediator.Send(new GetShippingPlanByIDQuery { Id = id });
            return Ok(result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ShippingPlanModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Result>> UpdateShippingPlanAsync(int id, [FromBody] ShippingPlanModel shippingPlan)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(shippingPlan);
            }

            var result = await Mediator.Send(new UpdateShippingPlanCommand { Id = id, ShippingPlan = shippingPlan });
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<int>> DeletedShippingPlanAsync(int id)
        {
            var result = await Mediator.Send(new DeleteShippingPlanCommand { Id = id });
            return Ok(result);
        }
    }
}
