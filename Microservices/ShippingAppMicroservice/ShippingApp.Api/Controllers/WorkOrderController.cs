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

                var workOrder = new WorkOrderModel
                {
                    Notes = item?.Notes ?? string.Empty,
                    RefId = group.WorkOrderId,
                    CreatedDate = DateTime.Now,
                    PartRevision = item.PartRevision,
                    CustomerName = item.CustomerName,
                    ProcessRevision = item.ProcessRevision
                };

                var workOderDetails = new List<WorkOrderDetailModel>();

                var groupByProduct = group.WorkOrderDetails
                                                  .GroupBy(x => x.ProductNumber)
                                                  .Select(g => new
                                                  {
                                                      ProductNumber = g.Key,
                                                      Quantity = g.Sum(x => x.Quantity),
                                                      WorkOders = g.ToList()
                                                  });

                if (groupByProduct.Count() > 1)
                {
                    invalidworkOrders.AddRange(group.WorkOrderDetails);
                    _logger.LogError("Work Order can only have one Product");

                    continue;
                }

                foreach (var product in groupByProduct)
                {
                    var productDatabase = await Mediator.Send(new GetProductByProductNumberQuery
                    {
                        ProductNumber = product.ProductNumber
                    });

                    if (productDatabase == null)
                    {
                        _logger.LogError("Could not find any product with {0}", product.ProductNumber);
                        invalidworkOrders.AddRange(product.WorkOders);
                        continue;
                    }

                    workOderDetails.Add(new WorkOrderDetailModel
                    {
                        ProductId = productDatabase.Id,
                        Quantity = product.Quantity,
                        WorkOrderId = workOrder.Id,
                    });
                }

                workOrder.WorkOrderDetails = workOderDetails;

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

            var result = await Mediator.Send(new CreateWorkOrderCommand { WorkOrder = workOrder });
            return Ok(result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<WorkOrderModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<WorkOrderModel>>> GetWorkOrders()
        {
            return await Mediator.Send(new GetWorkOrdersQuery { });
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(WorkOrderModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<WorkOrderModel>> GetWorkOrderByIdAsync(int id)
        {
            var result = await Mediator.Send(new GetWorkOrderByIdQuery { Id = id });
            return Ok(result);
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

            var result = await Mediator.Send(new UpdateWorkOrderCommand { Id = id, WorkOrder = workOrder });
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Result>> DeleteWorkOrderAysnc(int id)
        {
            var result = await Mediator.Send(new DeleteWorkOrderCommand { Id = id });
            return Ok(result);
        }
    }
}
