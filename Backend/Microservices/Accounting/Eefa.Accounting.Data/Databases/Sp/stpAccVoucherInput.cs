using System;

namespace Eefa.Accounting.Data.Databases.Sp
{
    public class stpAccVoucherInput : IReportSpBase
    {
        public int Id { get; set; }
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
