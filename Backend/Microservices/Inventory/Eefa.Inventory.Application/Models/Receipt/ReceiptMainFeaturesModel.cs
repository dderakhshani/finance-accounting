using System;
using Eefa.Common;

namespace Eefa.Inventory.Application
{

    public record ReceiptMainFeaturesModel : IMapFrom<Domain.Receipt>
    {
        public int Id { get; set; }
        public int? AccountReferencesGroupId { get; set; }
        public string AccountReferencesGroupTitle { get; set; }
        public int CodeVoucherGroupId { get; set; } = default!;
        public string CodeVoucherGroupTitle { get; set; } = default!;
        
        /// <description>
        /// کد سال
        ///</description>
        public string DocumentSerial { get; set; }

        public int YearId { get; set; } = default!;
        /// <description>
        /// کد انبار
        ///</description>

        public int WarehouseId { get; set; } = default!;
        
        public string WarehouseTitle { get; set; } = default!;

       
        /// <description>
        /// کد مرجع
        ///</description>

        public int? ReferenceId { get; set; }
        public string ReferenceTitle { get; set; }
        public string ReferenceNumber { get; set; }
        /// <description>
        /// شماره سند
        ///</description>

        public int? DocumentNo { get; set; }
        /// <description>
        /// تاریخ سند
        ///</description>

        public DateTime DocumentDate { get; set; } = default!;
        /// <description>
        /// توضیحات سند
        ///</description>

        public string? DocumentDescription { get; set; }

        /// <description>
        /// تاریخ انقضا
        ///</description>

        public DateTime? ExpireDate { get; set; } = default!;
        public DateTime? ModifiedAt { get; set; } = default!;
        
        /// <description>
        /// کد وضعیت سند
        ///</description>

        public int DocumentStateBaseId { get; set; } = default!;
        public string DocumentStateBaseTitle{ get; set; } = default!;

        /// <description>
        /// شماره درخواست سیستم آرانی
        /// </description>
        public string RequestNo { get; set; } = default!;

        /// <description>
        /// جادهی کالا کامل انجام شده است
        /// </description>
        public bool IsPlacementComplete { get; set; } = default!;
        /// <description>
        /// شماره فاکتور فروشنده
        ///</description>
        public string InvoiceNo { get; set; }

        public string Tags { get; set; } = default!;



    }



}
