using AutoMapper;
using Eefa.Common;
using Eefa.Inventory.Domain;
using System;

namespace Eefa.Inventory.Application
{

    public partial class PersonsDebitedCommoditiesModel : IMapFrom<PersonsDebitedCommoditiesView>
    {

        public void Mapping(Profile profile)
        {
            profile.CreateMap<PersonsDebitedCommoditiesView, PersonsDebitedCommoditiesModel>();
            profile.CreateMap<PersonsDebitedCommodities, PersonsDebitedCommoditiesModel>();

        }

        public int Id { get; set; }
        public string WarehousesTitle { get; set; } = default!;
        public int WarehouseId { get; set; } = default!;
        public string AssetSerial { get; set; } = default!;
        public DateTime DocumentDate { get; set; } = default!;
        public double Price { get; set; } = default!;
        public int? DepreciationTypeBaseId { get; set; } = default!;
        public int AssetGroupId { get; set; } = default!;
        public double DepreciatedPrice { get; set; } = default!;
        public int? UnitId { get; set; } = default!;
        public int? MeasureId { get; set; } = default!;
        public bool IsActive { get; set; } = default!;
        public int CommodityId { get; set; } = default!;
        public string CommoditySerial { get; set; } = default!;
        public string CategoryLevelCode { get; set; } = default!;
        public string CategoryTitle { get; set; } = default!;
        public string MeasureTitle { get; set; } = default!;
        public string SearchTerm { get; set; } = default!;
        public string UnitsTitle { get; set; } = default!;
        public string DepreciationTitle { get; set; } = default!;
        public string AssetGroupTitle { get; set; } = default!;
        public int DocumentItemId { get; set; } = default!;
        public int PersonId { get; set; } = default!;
        public int? AssetId { get; set; } = default!;
        public int DebitTypeId { get; set; } = default!;
        public DateTime ExpierDate { get; set; } = default!;
        public double Quantity { get; set; } = default!;
        public string FirstName { get; set; } = default!;

        public string LastName { get; set; } = default!;


        public string FullName { get; set; }
        public string NationalNumber { get; set; } = default!;
        public string CommodityTitle { get; set; }
        public string CommodityCode { get; set; }

        public bool? IsHaveWast { get; set; } = default!;
        public bool? IsAsset { get; set; } = default!;
        public bool? IsConsumable { get; set; } = default!;
        public string DebitReferenceTitle { get; set; }
        public int DocumentNo { get; set; }

        public string DocumentItemsDescription { get; set; } = default!;
        public string DocumentDescription { get; set; } = default!;


    }


}
