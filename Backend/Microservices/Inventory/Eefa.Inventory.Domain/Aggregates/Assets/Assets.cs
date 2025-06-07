using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Eefa.Common.Data;
using Eefa.Common.Domain;

namespace Eefa.Inventory.Domain
{

    [Table("Assets", Schema = "inventory")]
    public partial class Assets : DomainBaseEntity, IAggregateRoot
    {
        public int WarehouseId { get; set; } = default!;
        public int CommodityId { get; set; } = default!;
        public int MeasureId { get; set; } = default!;
        public int AssetGroupId { get; set; } = default!;
        public int? UnitId { get; set; } = default!;
        public DateTime DocumentDate { get; set; } = default!;
        public string CommoditySerial { get; set; } = default!;
        public string AssetSerial { get; set; } = default!;
        public int? DepreciationTypeBaseId { get; set; } = default!;
        public double Price { get; set; } = default!;
        public double? DepreciatedPrice { get; set; } = default!;
        public bool IsActive { get; set; } = default!;
        public int? DocumentHeadId { get; set; } = default!;
        public int? DocumentItemId { get; set; } = default!;
        public string Description { get; set; } = default!;
       
    }
   

}
