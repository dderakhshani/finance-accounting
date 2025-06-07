using System;
using System.Collections.Generic;

public partial class MoadianInvoiceHeader : AuditableEntity
    {
        public int? VerificationCodeId { get; set; }

        public long ListNumber { get; set; }
        public string? Errors { get; set; }
        public string? Status { get; set; } = default!;
        public string? ReferenceId { get; set; }
        public string? UId { get; set; }
        public int? PersonId { get; set; }
        public int? CustomerId { get; set; }
        public int? AccountReferenceId { get; set; }
        public bool IsSandbox { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public DateTime? SubmissionDate { get; set; }
        public string TaxId { get; set; } = default!;
        public long? Indatim { get; set; } = default!;
        public long? Indati2m { get; set; } = default!;
        public int? Inty { get; set; } = default!;
        public string Inno { get; set; } = default!;
        public string? Irtaxid { get; set; }
        public int? Inp { get; set; } = default!;
        public int? Ins { get; set; } = default!;
        public string Tins { get; set; } = default!;
        public int? Tob { get; set; }
        public string? Bid { get; set; }
        public string? Tinb { get; set; }
        public string? Sbc { get; set; }
        public string? Bpc { get; set; }
        public string? Bbc { get; set; }
        public int? Ft { get; set; }
        public string? Bpn { get; set; }
        public string? Scln { get; set; }
        public string? Scc { get; set; }
        public string? Crn { get; set; }
        public string? Billid { get; set; }
        public decimal? Tprdis { get; set; } = default!;
        public decimal? Tdis { get; set; } = default!;
        public decimal? Tadis { get; set; } = default!;
        public decimal? Tvam { get; set; } = default!;
        public decimal? Todam { get; set; } = 0!;
        public decimal? Tbill { get; set; } = default!;
        public int? Setm { get; set; } = default!;
        public decimal? Cap { get; set; } = default!;
        public decimal? Insp { get; set; } = default!;
        public decimal? Tvop { get; set; } = default!;
        public decimal? Tax17 { get; set; } = default!;
        public string? Cdcn { get; set; }
        public int? Cdcd { get; set; }
        public decimal? Tonw { get; set; } = default!;
        public decimal? Torv { get; set; } = default!;
        public decimal? Tocv { get; set; } = default!;


        public virtual ICollection<MoadianInvoiceDetail> MoadianInvoiceDetails { get; set; } = default!;
        public virtual Person Person { get; set; } = default!;
        public virtual Customer Customer { get; set; } = default!;
        public virtual AccountReference AccountReference { get; set; } = default!;
        public virtual VerificationCode VerificationCode { get; set; } = default!;
        public virtual User CreatedBy { get; set; } = default!;

    }

