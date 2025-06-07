using Eefa.Common;
using Eefa.Inventory.Domain;

namespace Eefa.Inventory.Application
{
    public class WarehouseHistoryModel : IMapFrom<WarehouseHistory>
    {
        public int Id { get; set; } = default!;
        public int Commodityld { get; set; } = default!;
        /// <description>
        /// کد چیدمان انبار
        ///</description>

        public int WarehouseLayoutId { get; set; } = default!;
        /// <description>
        /// تعداد
        ///</description>

        public double Quantity { get; set; } = default!;
        /// <description>
        /// نوع عملیات
        /// -1 --> خروجی
        /// 1 --> ورودی
        ///</description>

        public int Mode { get; set; } = default!;
        /// <description>
        /// شماره آیتم در سند
        ///</description>

        public int? DocumentItemId { get; set; }
        /// <description>
        ///شماره درخواست خروج
        ///</description>
        ///
        public int? RequestNo { get; set; }
        public string WarehouseLayoutTitle { get; set; }
       
        public string ModeTitle { get; set; }


    }
}
