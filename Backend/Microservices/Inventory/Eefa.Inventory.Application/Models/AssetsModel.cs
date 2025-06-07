using System;
using System.Collections.Generic;
using AutoMapper;
using Eefa.Common;
using Eefa.Inventory.Domain;

namespace Eefa.Inventory.Application
{

    public class AssetsModel : IMapFrom<Assets>
    {
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Assets, AssetsModel>();
            profile.CreateMap<AssetsNotAssignedView, AssetsModel>();
            profile.CreateMap<AssetsView, AssetsModel>();

        }
        public int Id { get; set; } = default!;
        public int WarehouseId { get; set; } = default!;
        public string WarehouseTitle { get; set; } = default!;
        public int CommodityId { get; set; } = default!;
        public string CommodityTitle { get; set; } = default!;
        public string CommodityCode { get; set; } = default!;
        public int MeasureId { get; set; } = default!;
        public string MeasureTitle { get; set; } = default!;
        public int AssetGroupId { get; set; } = default!;
        public string AssetGroupTitle { get; set; } = default!;
        public int? UnitId { get; set; } = default!;
        public string UnitTitle { get; set; } = default!;
        public DateTime DocumentDate { get; set; } = default!;
        public string CommoditySerial { get; set; } = default!;
        public string AssetSerial { get; set; } = default!;
        public int? DepreciationTypeBaseId { get; set; } = default!;
        public string DepreciationTitle { get; set; } = default!;
        public double Price { get; set; } = default!;
        public double? DepreciatedPrice { get; set; } = default!;
        public bool? IsActive { get; set; } = default!;
        public int? DocumentHeadId { get; set; } = default!;
        public int? DocumentItemId { get; set; } = default!;

        public int? DocumentNo { get; set; } = default!;
        public string WarehousesTitle { get; set; } = default!;
        public string CategoryLevelCode { get; set; } = default!;
        public string CategoryTitle { get; set; } = default!;
        public string SearchTerm { get; set; } = default!;
        public string UnitsTitle { get; set; } = default!;
        public bool? IsHaveWast { get; set; } = default!;
        public bool? IsAsset { get; set; } = default!;
        public bool? IsConsumable { get; set; } = default!;
        public string TotalDescription { get; set; } = default!;
        public List<AssetsSerialModel> AssetsSerials { get; set; } = default!;



    }
    public class AssetsSerialModel
    {
        public int Id { get; set; } = default!;
        public int CommodityId { get; set; } = default!;
        public string Serial { get; set; } = default!;
        public string Title { get; set; } = default!;
        public string CommoditySerial { get; set; } = default!;
        public string Description { get; set; } = default!;


    }
    public class AttachmentAssetsRequest
    {
        public int AssetsId { get; set; } = default!;
        public int AttachmentId { get; set; } = default!;
        public int PersonsDebitedCommoditiesId { get; set; } = default!;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<AttachmentAssetsRequest, Domain.AssetAttachments>()
                .IgnoreAllNonExisting();
        }
    }

    public class AssetsserialDuplicate
    {
        public int state { get; set; }
        public string requestId { get; set; }
        public int menueId { get; set; }
        public AssetsSerialModel[] AssetsSerial { get; set; }
    }

    //public class Assetsserial
    //{
    //    public int commodityId { get; set; }
    //    public string serial { get; set; }
    //    public int? Id { get; set; }
    //}

}
