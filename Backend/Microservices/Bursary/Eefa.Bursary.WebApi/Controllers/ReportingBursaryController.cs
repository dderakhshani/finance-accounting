using Eefa.Bursary.Application.Models;
using Eefa.Common.Common.Utilities;
using Eefa.Common.Web;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using static Eefa.Common.Common.Utilities.SSRSUtility;

namespace Eefa.Bursary.WebApi.Controllers
{
    public class ReportingBursaryController : ApiControllerBase
    {
        public ReportingBursaryController()
        {

        }
        [HttpPost]
        public async Task<IActionResult> GetReportBalance4([FromBody] QueryModel model)
        {
                return await new SSRSUtility().GetReport(model, "Acc_Balance_4", ReportFormat.Pdf);
        }


    


    }
 }
