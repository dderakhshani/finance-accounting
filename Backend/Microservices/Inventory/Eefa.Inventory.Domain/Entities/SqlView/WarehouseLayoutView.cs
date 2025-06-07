using System;

namespace Eefa.Inventory.Domain
{

   
    public partial class WarehouseLayoutView
    {
        [System.ComponentModel.DataAnnotations.Key]
        public int Id { get; set; }
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


        public int? IsLock { get; set; }

        public bool LastLevel { get; set; } = default!;
        public bool? IsDefault { get; set; } = default!;





    }
}
