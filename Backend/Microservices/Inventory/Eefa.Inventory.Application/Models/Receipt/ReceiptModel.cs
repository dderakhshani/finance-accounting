using System.Collections.Generic;
using Eefa.Common;

namespace Eefa.Inventory.Application
{

    public record ReceiptModel : ReceiptMainFeaturesModel, IMapFrom<Domain.Receipt>
    {
        
        /// <description>
        /// کد والد
        ///</description>

        public int? ParentId { get; set; }
     
        /// <description>
        /// درصد تخفیف کل فاکتور
        ///</description>

        public double? DiscountPercent { get; set; } = default!;
        /// <description>
        /// تخفیف کل سند
        ///</description>

        public double? DocumentDiscount { get; set; } = default!;

        /// <description>
        /// دستی
        ///</description>

        public bool IsManual { get; set; } = default!;
        public double TotalWeight { get; set; } = default!;
        public double TotalQuantity { get; set; } = default!;

        public string RequesterReferenceTitle { get; set; } = default!;

        public string FollowUpReferenceTitle { get; set; } = default!;

        public string CorroborantReferenceTitle { get; set; } = default!;
       
        

        public int? RequesterReferenceId { get; set; } = default!;

        public int? FollowUpReferenceId { get; set; } = default!;
        public int? CorroborantReferenceId { get; set; } = default!;

        public  List<ReceiptItemModel> Items { get; set; } = default!;
       
        public List<DocumentHeadExtendModel> DocumentHeadExtend { get; set; } = default!;



    }

   

}
