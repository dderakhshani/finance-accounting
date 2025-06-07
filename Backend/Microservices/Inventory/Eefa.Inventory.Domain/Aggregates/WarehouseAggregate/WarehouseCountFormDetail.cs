using Eefa.Common.Domain;
using Eefa.Inventory.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eefa.Inventory.Domain.Aggregates.WarehouseAggregate
{
    [Table("WarehouseCountFormDetail", Schema = "inventory")]
    public class WarehouseCountFormDetail : DomainBaseEntity, IAggregateRoot
    {
        public int WarehouseCountFormHeadId { get; set; }
        public int WarehouseLayoutQuantitiesId { get; set; }
        public double? CountedQuantity { get; set; }
        public double? SystemQuantity { get; set; }
        public string Description { get; set; }
        public WarehouseLayoutStatus LastWarehouseLayoutStatus { get; set; }

    }
}
