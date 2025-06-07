using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Stimulsoft.Base;
using Stimulsoft.Report;
using Stimulsoft.Report.Mvc;
using Microsoft.AspNetCore.Authorization;
using Eefa.Accounting.Application.UseCases.Report.Model;
using Eefa.Accounting.Application.UseCases.Report;
using System;
using MediatR;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Eefa.Accounting.WebApi.Controllers
{
    [Route("accountingreports/[controller]/[action]")]
    [Authorize]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class StimulSoftController : Controller
    {
        private IConfiguration _configuration;
        private IWebHostEnvironment _hostEnvironment;
        protected IMediator Mediator => HttpContext.RequestServices.GetService<IMediator>();
        public StimulSoftController(IConfiguration configuration, IWebHostEnvironment hostEnvironment)
        {
            _configuration = configuration;
            _hostEnvironment = hostEnvironment;
            var path = Path.Combine(hostEnvironment.ContentRootPath, "Content\\license.key");
            StiLicense.LoadFromFile(path);
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> GetReportAsync([FromQuery] int companyId, [FromQuery] int[] yearIds, [FromQuery] DateTime fromDate, [FromQuery] DateTime toDate)
        {
            AnnualLedgerReportQuery model = new AnnualLedgerReportQuery();
            model.CompanyId = companyId;
            model.YearIds = yearIds;
            model.VoucherDateFrom = fromDate;
            model.VoucherDateTo = toDate;
            var result = await Mediator.Send(model);
            var obj = (AnnualLedgerResultModel)result.ObjResult;
            var datas = obj.Datas;

            LoadFonts();

            StiReport report = new StiReport();
            var path = StiNetCoreHelper.MapPath(this, $"Reports/AnnualReport.mrt");
            report.Load(path);

            report.RegBusinessObject("first", datas);

            //var dbConnection = (StiSqlDatabase)report.Dictionary.Databases["Connection"];
            //dbConnection.ConnectionString = _configuration.GetConnectionString("DefaultString");

            return StiNetCoreViewer.GetReportResult(this, report);
        }

        public IActionResult ViewerEvent()
        {
            return StiNetCoreViewer.ViewerEventResult(this);
        }

        private void LoadFonts()
        {
            var fontDirectory = Path.Combine(_hostEnvironment.ContentRootPath, "Localization\\fonts");
            var fontFiles = Directory.GetFiles(fontDirectory);
            foreach (var font in fontFiles)
            {
                StiFontCollection.AddFontFile(font);
            }
        }
    }
}
