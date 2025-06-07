using Eefa.Common.Data;
using Eefa.Common.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
namespace Eefa.Inventory.Domain
{
    [Table("PersonsDebitedCommodities", Schema = "inventory")]
    public partial class PersonsDebitedCommodities : DomainBaseEntity, IAggregateRoot
    {
        public int? WarehouseId { get; set; } = default!;
        public int DocumentItemId { get; set; } = default!;
        
        public int? PersonId { get; set; } = default!;
        public int? DebitAccountReferenceId { get; set; } = default!;
        public int? DebitAccountReferenceGroupId { get; set; } = default!;
        
        public int? CommodityId { get; set; } = default!;
        public int? MeasureId { get; set; } = default!;
        public string CommoditySerial { get; set; } = default!;
        public int? AssetId { get; set; } = default!;
        public int? UnitId { get; set; } = default!;
        public int DebitTypeId { get; set; } = default!;
        public DateTime DocumentDate { get; set; } = default!;
        public DateTime ExpierDate { get; set; } = default!;
        public double Quantity { get; set; } = default!;
        public bool IsActive { get; set; } = default!;
        public string Description { get; set; } = default!;
       
    }
   

}
