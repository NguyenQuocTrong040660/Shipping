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

            var groupByShippingPlan = shippingPlanImports.GroupBy(x => x.ShippingPlanId)
                                                  .Select(g => new
                                                  {
                                                      ShippingPlanId = g.Key,
                                                      ShippingPlans = g.ToList()
                                                  });

            foreach (var group in groupByShippingPlan)
            {
                var item = shippingPlanImports.FirstOrDefault(i => i.ShippingPlanId == group.ShippingPlanId);

                int Quantity = int.Parse(item.QuantityOrder);
                float Price = float.Parse(item.SalesPrice);

                var productDatabase = await Mediator.Send(new GetProductByProductNumberQuery
                {
                    ProductNumber = item.ProductNumber
                });

                if (productDatabase == null || Quantity * Price < 0 || group.ShippingPlans.Count > 1)
                {
                    _logger.LogError("Failed to import data with shippingPlanId {0}", group.ShippingPlanId);
                    invalidDatas.AddRange(group.ShippingPlans);
                    continue;
                }

                var shippingPlanDatabase = await Mediator.Send(new GetShippingPlanByRefIdQuery
                {
                    ProductNumber = item.ProductNumber,
                    SalelineNumber = item.SalelineNumber,
                    SalesOrder = item.SalesOrder
                });

                if (shippingPlanDatabase != null)
                {
                    invalidDatas.AddRange(group.ShippingPlans);
                    continue;
                }

                var shippingPlan = new ShippingPlanModel
                {
                    Notes = item?.Notes ?? string.Empty,
                    CustomerName = item?.CustomerName ?? string.Empty,
                    SalesOrder = item.SalesOrder,
                    ShippingDate = DateTime.Parse(item.ShippingDate),
                    PurchaseOrder = item.PurchaseOrder,
                    SalelineNumber = item.SalelineNumber,
                    BillTo = item.BillTo,
                    BillToAddress = item.BillToAddress,
                    ShipTo = item.ShipTo,
                    ShipToAddress = item.ShipToAddress,
                    ProductLine = item.ProductLine,
                    AccountNumber = item.AccountNumber,
                    ProductId = productDatabase.Id,
                    Quantity = Quantity,
                    Price = Price,
                    Amount = Quantity * Price,
                    ShippingMode = item.ShippingMode
                };
                
                var result = await Mediator.Send(new CreateNewShippingPLanCommand 
                { 
                    ShippingPlan = shippingPlan 
                });

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
        public async Task<ActionResult<Result>> AddShippingPlanAsync([FromBody] ShippingPlanModel shippingPlan)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(shippingPlan);
            }

            return Ok(await Mediator.Send(new CreateNewShippingPLanCommand() { ShippingPlan = shippingPlan }));
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<ShippingPlanModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<ShippingPlanModel>>> GetAllShippingPlanAsync()
        {
            return Ok(await Mediator.Send(new GetAllShippingPlanQuery()));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ShippingPlanModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ShippingPlanModel>> GetShippingPlanByIdAsync(int id)
        {
            return Ok(await Mediator.Send(new GetShippingPlanByIdQuery { Id = id }));
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

            return Ok(await Mediator.Send(new UpdateShippingPlanCommand { Id = id, ShippingPlan = shippingPlan }));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Result>> DeletedShippingPlanAsync(int id)
        {
            return Ok(await Mediator.Send(new DeleteShippingPlanCommand { Id = id }));
        }

        [HttpPost("VerifyImportShippingPlan")]
        [ProducesResponseType(typeof(List<ImportResult>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<ImportResult>>> VerifyImportShippingPlanAsync([FromBody] List<ShippingPlanImportModel> shippingPlans)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(shippingPlans);
            }

            var importResults = new List<ImportResult>();
            var validator = new ShippingPlanImportModelValidator();

            var shippingPlanDb = await Mediator.Send(new GetAllShippingPlanQuery { });

            foreach (var item in shippingPlans)
            {
                string key = $"{item.SalesOrder}-{item.SalelineNumber}-{item.ProductNumber}";

                if (shippingPlanDb.Any(x => x.RefId.Equals(key)))
                {
                    importResults.Add(ImportResult.Failure(new List<string> { "Shipping Plan has been existed" }, key, item));
                    continue;
                }

                var product = await Mediator.Send(new GetProductByProductNumberQuery { ProductNumber = item.ProductNumber });

                if (product == null)
                {
                    importResults.Add(ImportResult.Failure(new List<string> { "Product has not been existed" }, key, item));
                    continue;
                }

                var validateResult = validator.Validate(item);

                if (!validateResult.IsValid)
                {
                    importResults.Add(ImportResult.Failure(validateResult.Errors.Select(x => x.ErrorMessage).ToList(), key, item));
                }
                else
                {
                    importResults.Add(ImportResult.Success(key, item));
                }
            }

            return Ok(importResults);
        }
    }
}
