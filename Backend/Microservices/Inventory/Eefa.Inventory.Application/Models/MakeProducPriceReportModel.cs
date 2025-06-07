using Eefa.Common;
using Eefa.Inventory.Domain;
using System;
using System.Collections.Generic;

namespace Eefa.Inventory.Application
{

    public class MakeProductPrice
    {

        public double TotalWeight { get; set; } = default!;
        public double TotalRawMaterial { get; set; } = default!;
        public double TotalSalary { get; set; } = default!;
        public double TotalOverload { get; set; } = default!;
        public double TotalMeterage { get; set; } = default!;

        public double DocumentControls160 { get; set; } = default!;
        public double DocumentControls296 { get; set; } = default!;
        public double DocumentControls295 { get; set; } = default!;

        public DateTime? LastDate { get; set; } = default!;
        public bool AllowAssumeDocument { get; set; } = default!;

        public double SumALL { get; set; } = default!;

        public int VoucherNO { get; set; } = default!;
        public int  VoucherId { get; set; } = default!;
        public List<MakeProductPriceReportModel> MakeProductPriceReport { get; set; } = default!;
    }

    public class MakeProductPriceReportModel
    {
        public string Size { get; set; } = default!;
        public string Thickness { get; set; } = default!;
        public double? Meterage { get; set; } = default!;       
        public double? Weight { get; set; } = default!;
        public double? RawMaterial { get; set; } = default!;
        public double? Salary { get; set; } = default!;
        public double? Overload { get; set; } = default!;
        public double? Total { get; set; } = default!;

    }

}

