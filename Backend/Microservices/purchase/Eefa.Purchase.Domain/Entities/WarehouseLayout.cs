using System.Collections.Generic;
using System.Threading.Tasks;
using Eefa.Common.Data;
using Eefa.Common.Domain;
using Eefa.Purchase.Domain.Aggregates.InvoiceAggregate;

namespace Eefa.Purchase.Domain.Entities
{

    /// <summary>
    /// موقعیت های انبار
    /// </summary>
    public partial class WarehouseLayout : DomainBaseEntity
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
        public int? Status { get; set; }

        public bool LastLevel { get; set; } = default!;

        public virtual WarehouseLayout? Parent { get; set; }
        public virtual BaseValue? UnitBaseType { get; set; }
        public virtual Warehouse? Warehouse { get; set; }
        public virtual ICollection<DocumentItem> DocumentItems { get; set; } = default!;
        public virtual ICollection<WarehouseLayout> InverseParent { get; set; } = default!;
        public virtual ICollection<WarehouseHistory> WarehouseHistories { get; set; } = default!;
        public virtual ICollection<WarehouseLayoutProperty> WarehouseLayoutProperties { get; set; } = default!;
        public virtual ICollection<WarehouseLayoutCategories> WarehouseLayoutCategories { get; set; } = default!;
        public virtual ICollection<WarehouseLayoutQuantity> WarehouseLayoutQuantities { get; set; } = default!;

        public static async Task<int> AddQuantityAsync(WarehouseLayoutQuantity model, IRepository<WarehouseLayoutQuantity> _Repository)
        {

            _Repository.Insert(model);
            return await _Repository.SaveChangesAsync();


        }

        public static async Task<int> UpdateQuantityAsync(WarehouseLayoutQuantity model, IRepository<WarehouseLayoutQuantity> _Repository)
        {

             _Repository.Update(model);
            return await _Repository.SaveChangesAsync();

        }
        public static void AddQuantity(WarehouseLayoutQuantity model, IRepository<WarehouseLayoutQuantity> _Repository)
        {

            _Repository.Insert(model);
            


        }
        public static void UpdateQuantity(WarehouseLayoutQuantity model, IRepository<WarehouseLayoutQuantity> _Repository)
        {

            _Repository.Update(model);
            

        }
        //-------------------------------------History------------------------------------------
        public static async Task<int> AddHistoryAsync(WarehouseHistory model, IRepository<WarehouseHistory> _Repository)
        {

             _Repository.Insert(model);
            return await _Repository.SaveChangesAsync();

        }
        public static void AddHistory(WarehouseHistory model, IRepository<WarehouseHistory> _Repository)
        {

            _Repository.Insert(model);
            

        }
        //-------------------------------------Stock----------------------------------------------
        public static async Task<int> UpdateStockAsync(Stock model, IRepository<Stock> _Repository)
        {

            _Repository.Update(model);
            return await _Repository.SaveChangesAsync();

        }
       
        public static async Task<int> AddStockAsync(Stock model, IRepository<Stock> _Repository)
        {

            _Repository.Insert(model);
            return await _Repository.SaveChangesAsync();

        }
        //---------------------------------------WarehouseLayout-------------------------------------------------
        public static async Task<int> AddWarehouseLayoutAsync(WarehouseLayout model, IRepository<WarehouseLayout> _Repository)
        {

            _Repository.Insert(model);
            return await _Repository.SaveChangesAsync();

        }
        public static async Task<int> UpdateWarehouseLayoutAsync(WarehouseLayout model, IRepository<WarehouseLayout> _Repository)
        {

            _Repository.Update(model);
            return await _Repository.SaveChangesAsync();

        }
        public static void AddWarehouseLayout(WarehouseLayout model, IRepository<WarehouseLayout> _Repository)
        {

            _Repository.Insert(model);
           

        }
        public static void UpdateWarehouseLayout(WarehouseLayout model, IRepository<WarehouseLayout> _Repository)
        {

            _Repository.Update(model);
           

        }
        public static void DeleteWarehouseLayout(WarehouseLayout model, IRepository<WarehouseLayout> _Repository)
        {

            _Repository.Delete(model);


        }
        //---------------------------WarehouseLayoutCategories-------------------------------------------------------
        public static async Task<int> AddWarehouseLayoutCategoriesAsync(WarehouseLayoutCategories model, IRepository<WarehouseLayoutCategories> _Repository)
        {

            _Repository.Insert(model);
            return await _Repository.SaveChangesAsync();

        }
        public static async Task<int> UpdateWarehouseLayoutCategoriesAsync(WarehouseLayoutCategories model, IRepository<WarehouseLayoutCategories> _Repository)
        {

            _Repository.Update(model);
            return await _Repository.SaveChangesAsync();

        }
        public static void AddWarehouseLayoutCategories(WarehouseLayoutCategories model, IRepository<WarehouseLayoutCategories> _Repository)
        {

            _Repository.Insert(model);


        }
        public static void UpdateWarehouseLayoutCategories(WarehouseLayoutCategories model, IRepository<WarehouseLayoutCategories> _Repository)
        {

            _Repository.Update(model);


        }
        public static void DeleteWarehouseLayoutCategories(WarehouseLayoutCategories model, IRepository<WarehouseLayoutCategories> _Repository)
        {

            _Repository.Delete(model);


        }

        //---------------------------WarehouseLayoutCategories-------------------------------------------------------
        public static async Task<int> AddWarehouseLayoutPropertyAsync(WarehouseLayoutProperty model, IRepository<WarehouseLayoutProperty> _Repository)
        {

            _Repository.Insert(model);
            return await _Repository.SaveChangesAsync();

        }
        public static async Task<int> UpdateWarehouseLayoutPropertyAsync(WarehouseLayoutProperty model, IRepository<WarehouseLayoutProperty> _Repository)
        {

            _Repository.Update(model);
            return await _Repository.SaveChangesAsync();

        }
        public static void AddWarehouseLayoutProperty(WarehouseLayoutProperty model, IRepository<WarehouseLayoutProperty> _Repository)
        {

            _Repository.Insert(model);


        }
        public static void UpdateWarehouseLayoutProperty(WarehouseLayoutProperty model, IRepository<WarehouseLayoutProperty> _Repository)
        {

            _Repository.Update(model);


        }
        public static void DeleteWarehouseLayoutProperty(WarehouseLayoutProperty model, IRepository<WarehouseLayoutProperty> _Repository)
        {

            _Repository.Delete(model);


        }
        //-------------------------------------WarehouseRequestExit----------------------------------------------
        public static async Task<int> UpdateWarehouseRequestExitAsync(WarehouseRequestExit model, IRepository<WarehouseRequestExit> _Repository)
        {

            _Repository.Update(model);
            return await _Repository.SaveChangesAsync();

        }

        public static async Task<int> AddWarehouseRequestExitAsync(WarehouseRequestExit model, IRepository<WarehouseRequestExit> _Repository)
        {

            _Repository.Insert(model);
            return await _Repository.SaveChangesAsync();

        }

    }


}
