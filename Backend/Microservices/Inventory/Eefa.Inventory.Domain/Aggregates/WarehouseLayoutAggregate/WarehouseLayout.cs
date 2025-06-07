using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;
using Eefa.Common;
using Eefa.Common.Data;
using Eefa.Common.Domain;
using Eefa.Inventory.Domain.Aggregates.WarehouseAggregate;
using Eefa.Inventory.Domain.Enum;

namespace Eefa.Inventory.Domain
{

    /// <summary>
    /// موقعیت های انبار
    /// </summary>
    /// 
    [Table("WarehouseLayouts", Schema = "inventory")]
    public partial class WarehouseLayout : DomainBaseEntity, IAggregateRoot, IHierarchical
    {

        public int? WarehouseId { get; set; }
        /// <description>
        /// کد والد
        ///</description>

        public int? ParentId { get; set; }
        /// <description>
        /// کد سطح
        ///</description>

        public string LevelCode { get; set; } = default!;
        /// <description>
        /// عنوان
        ///</description>

        public string Title { get; set; } = default!;
        /// <description>
        /// ظرفیت
        ///</description>

        public int Capacity { get; set; } = default!;
        /// <description>
        /// شماره شروع
        ///</description>

        public int StartIndex { get; set; } = default!;
        /// <description>
        /// شماره پایان
        ///</description>

        public int? EndIndex { get; set; }
        /// <description>
        /// نوع واحد کالا
        ///</description>

        public int? UnitBaseTypeId { get; set; }
        /// <description>
        /// ترتیب نمایش
        ///</description>

        public int OrderIndex { get; set; } = default!;
        /// <description>
        /// زیر مجموعه بصورت سریال است
        ///</description>

        public bool IsChildSequncial { get; set; } = default!;
        /// <description>
        /// نقش صاحب سند
        ///</description>

        public int? EntryMode { get; set; }

       
        public WarehouseLayoutStatus? Status { get; set; }

        public bool LastLevel { get; set; } = default!;
        public bool? IsDefault { get; set; } = default!;
        

        public virtual WarehouseLayout? Parent { get; set; }
        public virtual BaseValue? UnitBaseType { get; set; }
        //public virtual Warehouse? Warehouse { get; set; }
        //public virtual ICollection<DocumentItem> DocumentItems { get; set; } = default!;
        public virtual ICollection<WarehouseLayout> InverseParent { get; set; } = default!;
       
        public virtual ICollection<WarehouseLayoutProperty> WarehouseLayoutProperties { get; set; } = default!;
        public virtual ICollection<WarehouseLayoutCategories> WarehouseLayoutCategories { get; set; } = default!;
        //public virtual ICollection<WarehouseLayoutQuantity> WarehouseLayoutQuantities { get; set; } = default!;
        public virtual ICollection<WarehouseCountFormHead> WarehouseCountFormHeads { get; set; } = default!;
        public List<AuditMapRule> Audit()
        {
            return new List<AuditMapRule>();
        }

       
        public static async Task<int> UpdateQuantityAsync(WarehouseLayoutQuantity model, IRepository<WarehouseLayoutQuantity> _Repository)
        {
            try
            {
                _Repository.Update(model);
                return await _Repository.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                return 0;
            }
            

        }
        
        //-------------------------------------History------------------------------------------
        public static async Task<int> AddHistoryAsync(WarehouseHistory model, IRepository<WarehouseHistory> _Repository)
        {

             _Repository.Insert(model);
            return await _Repository.SaveChangesAsync();

        }
        
        
       
    }

 

}
