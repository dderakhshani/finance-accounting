using Eefa.Accounting.Application.UseCases.Report.Model;
using Eefa.Accounting.Application.UseCases.Report;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Stimulsoft.Base;
using Stimulsoft.Report.Mvc;
using Stimulsoft.Report;
using System.IO;
using System.Threading.Tasks;
using System;
using Library.Interfaces;
using Library.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Data;
using AutoMapper;
using Eefa.Accounting.Application.UseCases.Report.Model;
using Eefa.Accounting.Data.Databases.SqlServer.Context;
using Eefa.Persistence.Data.SqlServer.QueryProvider;
using Library.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.Net;
using Eefa.Accounting.WebApi.ReportModels;

namespace Eefa.Accounting.WebApi.Controllers
{

    [Route("accountingreports/[controller]/[action]")]
    [Authorize]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ProfitAndLossController : Controller
    {
        private IConfiguration _configuration;
        private IWebHostEnvironment _hostEnvironment;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        protected IMediator Mediator => HttpContext.RequestServices.GetService<IMediator>();
        public ProfitAndLossController(IConfiguration configuration, IWebHostEnvironment hostEnvironment, ICurrentUserAccessor currentUserAccessor)
        {
            _configuration = configuration;
            _hostEnvironment = hostEnvironment;
            var path = Path.Combine(hostEnvironment.ContentRootPath, "Content\\license.key");
            StiLicense.LoadFromFile(path);
            _currentUserAccessor = currentUserAccessor;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetReportAsync([FromQuery] DateTime fromDate, [FromQuery] DateTime toDate)
        {

            ProfitAndLossReportQuery model = new ProfitAndLossReportQuery();
            model.CompanyId = _currentUserAccessor.GetCompanyId();
            model.YearIds = new int[1];
            model.YearIds[0] = _currentUserAccessor.GetYearId();
            model.VoucherDateFrom = fromDate;
            model.VoucherDateTo = toDate;
            var result = await Mediator.Send(model);
            var data = (List<ProfitAndLossReportResult>)result.ObjResult;

            string url = _configuration.GetSection("Logo:Url").Value;
            var request = WebRequest.Create(url);
            var response = request.GetResponse();
            var stream = response.GetResponseStream();
            Image image = Bitmap.FromStream(stream);
            ProfitAndLossOtherDataModel Other = new(fromDate.AddDays(1).AddSeconds(-1), toDate,image);
            StiReport report = new StiReport();
            var path = StiNetCoreHelper.MapPath(this, $"Reports/PerofitAndLossReport.mrt");
            LoadFonts();
            report.Load(path);

            report.RegBusinessObject("ProfitAndLoss", data);

            report.RegBusinessObject("OtherData", Other);


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
