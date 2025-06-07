using System;
using System.Collections.Generic;
using AutoMapper;
using Eefa.Common;
using Eefa.Inventory.Domain.Enum;

namespace Eefa.Inventory.Application
{

    public record WarehouseLayoutModel : IMapFrom<Domain.WarehouseLayout>
    {
        public int Id { get; set; }
        public int? WarehouseId { get; set; }
        public string WarehouseTitle { get; set; }
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
        public string CommodityTitle { get; set; } = default!;
        public int? CommodityId { get; set; } = default!;
        /// <description>
        /// ظرفیت قابل اختصاص به هر کالا 
        ///</description>

        public int Capacity { get; set; } = default!;

        /// <description>
        /// ظرفیت  کل اختصاص داده شده به هر مکان
        ///</description>
        public int TotlaCapacity { get; set; } = default!;

        public double? CapacityAvailable { get { return CalculateCapacityAvailable(Capacity, CapacityUsed); } }
        public double? CapacityUsedPercent { get; set; } = default!;
        /// <description>
        /// ظرفیت استفاده شده  هر کالا 
        ///</description>

        public double? CapacityUsed { get; set; } = default!;

        /// <description>
        /// ظرفیت کل استفاده شده  هر مکان 
        ///</description>
        public double? TotlaCapacityUsed { get; set; } = default!;

        public int EntryMode { get; set; } = default!;

        public string EntryModeTitle { get; set; }
        public bool LastLevel { get; set; } = default!;
        public int OrderIndex { get; set; } = default!;

        /// <description>
        /// Status =
        ///0 آزاد 
        ///1 فقط ورودی 
        ///2 فقط خروجی 
        ///3 قفل موقت 
        ///4 قفل دائم
        ///
        /// </description>


        public bool AllowInput { get { return LastLevel == true && Status < WarehouseLayoutStatus.OutputOnly ? true : false; } }
        public bool AllowOutput { get { return LastLevel == true && (Status == WarehouseLayoutStatus.Free || Status == WarehouseLayoutStatus.OutputOnly) ? true : false; } }
        public WarehouseLayoutStatus? Status { get; set; }
        public bool? IsDefault { get; set; } = default!;

        public List<WarehouseLayoutModel> Children { get; set; }
        public List<string[]> ParentNameString { get; set; }
        public List<WarehouseLayoutCategoryModel> Categoreis { get; set; }

        public virtual void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.WarehouseLayout, WarehouseLayoutModel>();
        }
        
        private double CalculateCapacityAvailable(double? Capacity, double? CapacityUsed)
        {
            return Convert.ToInt32(Capacity) - Convert.ToInt32(CapacityUsed);
        }
    }
 

    
    
    

}
