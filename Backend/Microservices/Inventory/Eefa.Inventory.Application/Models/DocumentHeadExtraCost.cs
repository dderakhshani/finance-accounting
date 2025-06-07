using Eefa.Common;
using Eefa.Inventory.Domain;
using System;

namespace Eefa.Inventory.Application
{
    public class DocumentHeadExtraCostModel : IMapFrom<DocumentHeadExtraCost>
    
    {
        public int Id { get; set; }
        public int DocumentHeadId { get; set; }
        public int ExtraCostAccountHeadId { get; set; }
        public Nullable<int> ExtraCostAccountReferenceGroupId { get; set; }
        public Nullable<int> ExtraCostAccountReferenceId { get; set; }


        public string ExtraCostAccountHeadTitle { get; set; }
        public string ExtraCostAccountReferenceGroupTitle { get; set; }
        public string ExtraCostAccountReferenceTitle { get; set; }


        public int BarCode { get; set; }
        public Nullable<decimal> ExtraCostAmount { get; set; }
        public string ExtraCostDescription { get; set; }
        public Nullable<int> ExtraCostCurrencyTypeBaseId { get; set; }
        public Nullable<double> ExtraCostCurrencyFee { get; set; }
        public Nullable<double> ExtraCostCurrencyAmount { get; set; }
        public string FinancialOperationNumber { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
