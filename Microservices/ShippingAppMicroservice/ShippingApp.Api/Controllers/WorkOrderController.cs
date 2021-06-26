using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShippingApp.Application.Common.Results;
using ShippingApp.Application.Product.Queries;
using ShippingApp.Application.WorkOrder.Commands;
using ShippingApp.Application.WorkOrder.Queries;
using ShippingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShippingApp.Api.Controllers
{
    public class WorkOrderController : BaseController
    {
        private readonly ILogger<WorkOrderController> _logger;

        public WorkOrderController(
            IMediator mediator, 
            ILogger<WorkOrderController> logger) : base(mediator)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost("BulkInsert")]
        [ProducesResponseType(typeof(List<WorkOrderImportModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<WorkOrderImportModel>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<WorkOrderImportModel>>> AddWorkOrderAsync([FromBody] List<WorkOrderImportModel> workOrderImports)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(workOrderImports);
            }

            var invalidworkOrders = new List<WorkOrderImportModel>();

            var groupWorkOrders = workOrderImports.GroupBy(x => x.WorkOrderId)
                                                  .Select(g => new
                                                  {
                                                      WorkOrderId = g.Key,
                                                      WorkOrderDetails = g.ToList()
                                                  });

            foreach (var group in groupWorkOrders)
            {
                var item = workOrderImports.FirstOrDefault(i => i.WorkOrderId == group.WorkOrderId);

                var productDatabase = await Mediator.Send(new GetProductByProductNumberQuery
                {
                    ProductNumber = item.ProductNumber
                });

                if (productDatabase == null)
                {
                    _logger.LogError("Could not find any product with {0}", item.ProductNumber);
                    invalidworkOrders.Add(item);
                    continue;
                }

                var workOrder = new WorkOrderModel
                {
                    Notes = item?.Notes ?? string.Empty,
                    RefId = group.WorkOrderId,
                    CreatedDate = DateTime.Now,
                    PartRevision = item.PartRevision,
                    CustomerName = item.CustomerName,
                    ProcessRevision = item.ProcessRevision,
                    ProductId = productDatabase.Id,
                    Quantity = int.Parse(item.Quantity),
                };

                var groupByProduct = group.WorkOrderDetails
                                                  .GroupBy(x => x.ProductNumber)
                                                  .Select(g => new
                                                  {
                                                      ProductNumber = g.Key,
                                                      Quantity = g.Sum(x => int.Parse(x.Quantity)),
                                                      WorkOders = g.ToList()
                                                  });

                if (groupByProduct.Count() > 1)
                {
                    invalidworkOrders.AddRange(group.WorkOrderDetails);
                    _logger.LogError("Work Order can only have one Product");
                    continue;
                }

                var result = await Mediator.Send(new CreateWorkOrderCommand { WorkOrder = workOrder });

                if (!result.Succeeded)
                {
                    _logger.LogError("Unable to create work order by importing");
                    invalidworkOrders.AddRange(group.WorkOrderDetails);
                }
            }

            return Ok(invalidworkOrders.Distinct());
        }

        [HttpPost]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(WorkOrderModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Result>> AddWorkOrderAsync(WorkOrderModel workOrder)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(workOrder);
            }

            return Ok(await Mediator.Send(new CreateWorkOrderCommand { WorkOrder = workOrder }));
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<WorkOrderModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<WorkOrderModel>>> GetWorkOrders()
        {
            return Ok(await Mediator.Send(new GetWorkOrdersQuery { }));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(WorkOrderModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<WorkOrderModel>> GetWorkOrderByIdAsync(int id)
        {
            return Ok(await Mediator.Send(new GetWorkOrderByIdQuery { Id = id }));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(WorkOrderModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Result>> UpdateWorkOrderAsync(int id, [FromBody] WorkOrderModel workOrder)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(workOrder);
            }

            return Ok(await Mediator.Send(new UpdateWorkOrderCommand { Id = id, WorkOrder = workOrder }));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Result>> DeleteWorkOrderAysnc(int id)
        {
            return Ok(await Mediator.Send(new DeleteWorkOrderCommand { Id = id }));
        }

        [HttpPost("VerifyImportWorkOrder")]
        [ProducesResponseType(typeof(List<ImportResult>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<ImportResult>>> VerifyImportWorkOrderAsync([FromBody] List<WorkOrderImportModel> workOrderImports)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(workOrderImports);
            }

            var importResults = new List<ImportResult>();
            var validator = new WorkOrderImportModelValidator();
            var workOrdersDb = await Mediator.Send(new GetWorkOrdersQuery { });

            foreach (var item in workOrderImports)
            {
                if (workOrdersDb.Any(x => x.RefId.Equals(item.WorkOrderId)))
                {
                    importResults.Add(ImportResult.Failure(new List<string> { "Work Order has been existed" }, item.WorkOrderId, item));
                    continue;
                }

                var product = await Mediator.Send(new GetProductByProductNumberQuery { ProductNumber = item.ProductNumber });

                if (product == null)
                {
                    importResults.Add(ImportResult.Failure(new List<string> { "Product has not been existed" }, item.WorkOrderId, item));
                    continue;
                }

                var validateResult = validator.Validate(item);

                if (!validateResult.IsValid)
                {
                    importResults.Add(ImportResult.Failure(validateResult.Errors.Select(x => x.ErrorMessage).ToList(), item.WorkOrderId, item));
                }
                else
                {
                    importResults.Add(ImportResult.Success(item.WorkOrderId, item));
                }
            }

            return Ok(importResults);
        }
    }
}
