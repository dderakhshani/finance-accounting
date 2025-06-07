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
    [Table("WarehouseCountFormHead", Schema = "inventory")]
    public class WarehouseCountFormHead : DomainBaseEntity, IAggregateRoot
    {

        public int? ParentId { get; set; }
        public int FormNo { get; set; }
        public DateTime FormDate { get; set; }
        public int WarehouseId { get; set; }
        public int WarehouseLayoutId { get; set; }        
        public int CounterUserId { get; set; }
        public int ConfirmerUserId { get; set; }
        public WarehouseStateForm FormState { get; set; }
        public string Description { get; set; }
        public List<WarehouseCountFormDetail> WarehouseCountFormDetails { get; set; }
        public virtual WarehouseLayout WarehouseLayout { get; set; }
    }
}
