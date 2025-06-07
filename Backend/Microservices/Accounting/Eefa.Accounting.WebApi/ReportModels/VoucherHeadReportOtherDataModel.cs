using System.Drawing;
using Eefa.Accounting.Application.UseCases.VouchersHead.Utility;

namespace Eefa.Accounting.WebApi.ReportModels
{
    public class VoucherHeadReportOtherDataModel
    {

        public VoucherHeadReportOtherDataModel(Image logo)
        {
            Logo = logo;

        }
        public Image Logo { get; set; }
       
    }
}
