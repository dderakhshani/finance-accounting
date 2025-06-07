using Eefa.Accounting.Data.Events.Abstraction;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eefa.Accounting.Data.Events.VoucherHead
{
    public class CreateVoucherDetailEvent : EntityEvent
    {
        [JsonProperty("voucherDate")]
        public DateTime VoucherDate { get; set; }
        [JsonProperty("accountHeadId")]
        public int AccountHeadId { get; set; } = default!;
        [JsonProperty("accountReferencesGroupId")]
        public int? AccountReferencesGroupId { get; set; } = default!;
        [JsonProperty("referenceId1")]
        public int? ReferenceId1 { get; set; }
        [JsonProperty("voucherRowDescription")]
        public string VoucherRowDescription { get; set; } = default!;
        [JsonProperty("credit")]
        public double Credit { get; set; } = default!;
        [JsonProperty("debit")]
        public double Debit { get; set; } = default!;

        [JsonProperty("rowIndex")]
        public int? RowIndex { get; set; }

        [JsonProperty("level1")]
        public int? Level1 { get; set; }
        [JsonProperty("level2")]
        public int? Level2 { get; set; }
        [JsonProperty("level3")]
        public int? Level3 { get; set; }
        [JsonProperty("currencyTypeBaseId")]
        public int? CurrencyTypeBaseId { get; set; }
        [JsonProperty("currencyFee")]
        public double? CurrencyFee { get; set; }
        [JsonProperty("currencyAmount")]
        public double? CurrencyAmount { get; set; }

    }
}
