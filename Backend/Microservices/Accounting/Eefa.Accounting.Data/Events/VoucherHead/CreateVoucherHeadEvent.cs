using Eefa.Accounting.Data.Events.Abstraction;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eefa.Accounting.Data.Events.VoucherHead
{
    public class CreateVoucherHeadEvent : EntityEvent
    {
        [JsonProperty("voucherDate")]
        public DateTime VoucherDate { get; set; }
        [JsonProperty("yearId")]
        public int YearId { get; set; }
        [JsonProperty("companyId")]
        public int CompanyId { get; set; }
        [JsonProperty("voucherNo")]
        public int VoucherNo { get; set; }
        [JsonProperty("voucherDailyId")]
        public int VoucherDailyId { get; set; }
        [JsonProperty("totalDebit")]
        public double TotalDebit { get; set; }
        [JsonProperty("totalCredit")]
        public double TotalCredit { get; set; }
        [JsonProperty("voucherDescription")]
        public string VoucherDescription { get; set; }
        [JsonProperty("codeVoucherGroupId")]
        public int CodeVoucherGroupId { get; set; }
        [JsonProperty("voucherStateId")]
        public int VoucherStateId { get; set; }

    }
}
