using System;
using System.Collections.Generic;
using AutoMapper;
using Eefa.Common;
using Eefa.Inventory.Domain;

namespace Eefa.Inventory.Application
{
    //محل و تعداد قرار گرفتن کالا در هر موقعیت انبار
    public class WarehouseLayoutsCommoditiesModel : IMapFrom<WarehouseLayoutsCommoditiesView>
    {
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.WarehouseHistoriesDocumentView, WarehouseLayoutsCommoditiesModel>()
                .ForMember(dest => dest.WarehouseTitle, opt => opt.MapFrom(src => src.WarehousesTitle)); 
            profile.CreateMap<Domain.WarehouseLayoutsCommoditiesView, WarehouseLayoutsCommoditiesModel>();
           
        }
        public int Id { get; set; }
        public int? DocumentItemId { get; set; }
        public int? WarehouseId { get; set; }
        public int? WarehouseLayoutId { get; set; }
        public int? WarehousesParentId { get; set; }
        public int? CommodityId { get; set; }
        public double? Quantity { get; set; }
        public int? WarehouseLayoutParentId { get; set; }
        public int? WarehouseLayoutCapacity { get; set; }
        public string WarehouseTitle { get; set; }
        public string WarehousesLevelCode { get; set; }
        public string WarehouseLayoutTitle { get; set; }
        public string WarehouseLayoutLevelCode { get; set; }
        public string CommodityCompactCode { get; set; }
        public string CommodityTadbirCode { get; set; }
        public string CommodityTitle { get; set; }
        public string CommodityCode { get; set; }
        public int Mode { get; set; } = default!;
        public string ModeTitle { get; set; } = default!;
        public double? TotalQuantity { get; set; }
        public int? DocumentNo { get; set; }
        public DateTime? DocumentDate { get; set; }
        public int? DocumentHeadId { get; set; }
        public string RequestNo { get; set; }
        public bool AllowInput { get; set; }
        public bool AllowOutput { get; set; }
        public DateTime? CreatedDate { get; set; } = default!;
        public string CreatedTime { get; set; } = default!;
        public int MeasureId { get; set; }
        public string MeasureTitle { get; set; }
        public double? QuantityInput { get; set; }
        public double? QuantityOutput { get; set; }
        public string TrasctionType { get; set; } = default!;
        public int? CodeVoucherGroupId { get; set; } = default!;
       

    }
    public class StocksCommoditiesModel
    {
      public  List<WarehouseLayoutsCommoditiesModel> WarehouseLayoutsCommoditiesModel { get; set; }
      public double? FirstQuantity { get; set; }
      public double? ModifyQuantity { get; set; }
    }
}
