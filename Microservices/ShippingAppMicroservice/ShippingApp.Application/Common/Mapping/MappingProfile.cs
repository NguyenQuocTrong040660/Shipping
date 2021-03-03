using AutoMapper;
using Entities = ShippingApp.Domain.Entities;
using Models = ShippingApp.Domain.Models;

namespace ShippingApp.Application.Common.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Entities.Country, Models.CountryModel>()
                .ReverseMap();
            CreateMap<Entities.Product, Models.ProductModel>()
               .ReverseMap();

            CreateMap<Entities.ShippingPlan, Models.ShippingPlanModel>()
                .ReverseMap();
            CreateMap<Entities.ShippingPlanDetail, Models.ShippingPlanDetailModel>()
               .ReverseMap();

            CreateMap<Entities.MovementRequest, Models.MovementRequestModel>()
               .ReverseMap();
            CreateMap<Entities.MovementRequestDetail, Models.MovementRequestDetailModel>()
               .ReverseMap();


            CreateMap<Entities.ReceivedMark, Models.ReceivedMarkModel>()
               .ReverseMap();
            CreateMap<Entities.ReceivedMarkMovement, Models.ReceivedMarkMovementModel>()
               .ReverseMap();
            CreateMap<Entities.ReceivedMarkPrinting, Models.ReceivedMarkPrintingModel>()
               .ReverseMap();
            CreateMap<Entities.ReceivedMarkSummary, Models.ReceivedMarkSummaryModel>()
               .ReverseMap();


            CreateMap<Entities.ShippingMark, Models.ShippingMarkModel>()
                .ReverseMap();
            CreateMap<Entities.ShippingMarkPrinting, Models.ShippingMarkPrintingModel>()
               .ReverseMap();
            CreateMap<Entities.ShippingMarkShipping, Models.ShippingMarkShippingModel>()
               .ReverseMap();
            CreateMap<Entities.ShippingMarkSummary, Models.ShippingMarkSummaryModel>()
               .ReverseMap();


            CreateMap<Entities.ShippingRequest, Models.ShippingRequestModel>()
                .ReverseMap();
            CreateMap<Entities.ShippingRequestDetail, Models.ShippingRequestDetailModel>()
              .ReverseMap();
            CreateMap<Entities.ShippingRequestLogistic, Models.ShippingRequestLogisticModel>()
              .ReverseMap();


            CreateMap<Entities.WorkOrder, Models.WorkOrderModel>()
               .ReverseMap();
            CreateMap<Entities.WorkOrderDetail, Models.WorkOrderDetailModel>()
              .ReverseMap();

            CreateMap<Entities.Config, Models.ConfigModel>()
              .ReverseMap();
        }
    }
}
