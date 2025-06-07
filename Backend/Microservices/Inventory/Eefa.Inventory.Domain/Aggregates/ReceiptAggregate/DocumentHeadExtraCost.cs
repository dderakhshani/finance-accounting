

namespace Eefa.Inventory.Domain
{
    using Eefa.Common.Data;
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
   
   
    [Table("DocumentHeadExtraCost", Schema = "common")]
    public partial class DocumentHeadExtraCost : BaseEntity
    {

        public int DocumentHeadId { get; set; }
        public int ExtraCostAccountHeadId { get; set; }
        public Nullable<int> ExtraCostAccountReferenceGroupId { get; set; }
        public Nullable<int> ExtraCostAccountReferenceId { get; set; }
        public Nullable<decimal> ExtraCostAmount { get; set; }
        public string ExtraCostDescription { get; set; }

        public int BarCode { get; set; }
        
        public Nullable<int> ExtraCostCurrencyTypeBaseId { get; set; }
        public Nullable<double> ExtraCostCurrencyFee { get; set; }
        public Nullable<double> ExtraCostCurrencyAmount { get; set; }
        public string FinancialOperationNumber { get; set; }

    }
}
