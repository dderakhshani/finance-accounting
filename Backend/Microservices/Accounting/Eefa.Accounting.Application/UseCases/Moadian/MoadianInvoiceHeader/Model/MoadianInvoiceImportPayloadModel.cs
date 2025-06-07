using System;

namespace Eefa.Accounting.Application.UseCases.Moadian.MoadianInvoiceHeader.Model
{
    public class MoadianInvoiceImportPayloadModel
    {
        // ُSystemData
        public string Acc { get; set; }
        public string Ccc { get; set; }

        // Header
        public string Inno { get; set; } = default!;
        public DateTime Indatim { get; set; } = default!;
        public int Ins { get; set; } = default!;
        public int Inp { get; set; } = default!;
        public int Tob { get; set; }
        public string Bid { get; set; }
        public string Tinb { get; set; }
        public string Bpc { get; set; }
        public string Irtaxid { get; set; } = default!;

        //Export Headers
        public string Scln { get; set; }
        public string Scc { get; set; }
        public string Cdcn { get; set; }
        public DateTime Cdcd { get; set; }

        // Body
        public string Sstid { get; set; } = default!;
        public string Sstt { get; set; } = default!;
        public string Mu { get; set; } = default!;
        public decimal Am { get; set; } = default!;
        public decimal Fee { get; set; } = default!;
        public decimal Dis { get; set; } = default!;


        // Export Body 
        public decimal Cfee { get; set; } = default!;
        public string Cut { get; set; }
        public decimal Exr { get; set; } = default!;
        public decimal Nw { get; set; } = default!;
        public decimal Sscv { get; set; } = default!;
        public decimal Ssrv { get; set; } = default!;

    }
}
