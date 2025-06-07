
using Eefa.Common.Data;
using Eefa.Common.Domain;
using System;
using System.Collections.Generic;

namespace Eefa.Inventory.Domain
{
  
    public partial class VouchersHead : DomainBaseEntity
    {
 
        public int CompanyId { get; set; } = default!;


        public int VoucherDailyId { get; set; } = default!;

        public int? TraceNumber { get; set; }

        public int YearId { get; set; } = default!;

        public int VoucherNo { get; set; } = default!;

        public DateTime VoucherDate { get; set; } = default!;


        public string VoucherDescription { get; set; } = default!;

        public int CodeVoucherGroupId { get; set; } = default!;

        public int VoucherStateId { get; set; } = default!;

        public string? VoucherStateName { get; set; }

        public int? AutoVoucherEnterGroup { get; set; }

        public double? TotalDebit { get; set; }

        public double? TotalCredit { get; set; }

        public double Difference { get; set; }


    }
}
