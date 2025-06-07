using System;

namespace Eefa.Accounting.Data.Databases.Sp
{
    public class StpVoucherDetailInput : IReportSpBase
    {
        public int? CodeVoucherGroupId { get; set; }
        public int? VoucherNoFrom { get; set; }
        public int? VoucherNoTo { get; set; }
        public int UserId { get; set; }
        public int YearId { get; set; }
        public int CompanyId { get; set; }

        public DateTime? VoucherDateFrom { get; set; }
        public DateTime? VoucherDateTo { get; set; }
        public string CompanyName { get; set; }
        public string UserName { get; set; }
        public string PersianDateFrom { get; set; }
        public string PersianDateTo { get; set; }
        public string YearName { get; set; }
        public string ReportTitle { get; set; }
        public string ReportTime { get; set; }
    }
}