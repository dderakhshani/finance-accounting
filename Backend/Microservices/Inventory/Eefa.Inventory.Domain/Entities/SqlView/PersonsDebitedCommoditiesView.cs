using System;

namespace Eefa.Inventory.Domain
{

    public partial class PersonsDebitedCommoditiesView
    {
        [System.ComponentModel.DataAnnotations.Key]
        public int Id { get; set; }
        public Nullable<int> PersonId { get; set; }
        public Nullable<int> WarehouseId { get; set; }
        public Nullable<int> CommodityId { get; set; }
        public Nullable<int> MeasureId { get; set; }
        public string CommoditySerial { get; set; }
        public Nullable<int> AssetId { get; set; }
        public int DebitTypeId { get; set; }
        public System.DateTime DocumentDate { get; set; }
        public Nullable<System.DateTime> ExpierDate { get; set; }
        public double Quantity { get; set; }
        public Nullable<int> UnitId { get; set; }
        public bool IsActive { get; set; }
        public Nullable<int> DocumentItemId { get; set; }
        public int OwnerRoleId { get; set; }
        public int CreatedById { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public Nullable<int> ModifiedById { get; set; }
        public System.DateTime ModifiedAt { get; set; }
        public bool IsDeleted { get; set; }
        public string MeasureTitle { get; set; }
        public string SearchTerm { get; set; }
        public string UnitsTitle { get; set; }
        public string DepreciationTitle { get; set; }
        public string AssetGroupTitle { get; set; }
        public string CategoryLevelCode { get; set; }
        public string CategoryTitle { get; set; }
        public string WarehousesTitle { get; set; }
        public string AssetSerial { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NationalNumber { get; set; }
        public string FullName { get; set; }
        public int DocumentNo { get; set; }

        

        public string CommodityTitle { get; set; }
        public string DebitReferenceTitle { get; set; }
        public string CommodityCode { get; set; }
        public bool? IsHaveWast { get; set; } = default!;
        public bool? IsAsset { get; set; } = default!;
        public bool? IsConsumable { get; set; } = default!;
        public Nullable<int> AccountReferenceId { get; set; }
        public string DocumentItemsDescription { get; set; } = default!;
        public string DocumentDescription { get; set; } = default!;

        public string TotalDescription { get; set; } = default!;



    }


}
