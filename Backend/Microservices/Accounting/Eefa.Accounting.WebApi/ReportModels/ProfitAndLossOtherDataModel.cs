using System;
using System.Drawing;

namespace Eefa.Accounting.WebApi.ReportModels
{
    public class ProfitAndLossOtherDataModel
    {
        public ProfitAndLossOtherDataModel(DateTime fromDate, DateTime toDate, Image logo)
        {
            Logo = logo;
            FromDate = fromDate;
            ToDate = toDate;
        }
        public Image Logo { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
