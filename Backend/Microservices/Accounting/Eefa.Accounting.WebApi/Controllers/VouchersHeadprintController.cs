using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq.Dynamic.Core;
using Eefa.Accounting.Application.UseCases.Report.Model;
using Eefa.Accounting.Application.UseCases.Report;
using Eefa.Accounting.WebApi.ReportModels;
using System.Net;
using System.Threading.Tasks;
using Library.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Stimulsoft.Base;
using Stimulsoft.Report.Mvc;
using Stimulsoft.Report;
using Eefa.Accounting.Application.UseCases.VouchersHead.Query.Get;
using Eefa.Accounting.Application.UseCases.VouchersHead.Utility;
using Eefa.Accounting.Data.Entities;
using Microsoft;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Accounting.WebApi.Controllers
{

    [Route("accountingreports/[controller]/[action]")]
    [Authorize]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class VouchersHeadprintController : Controller
    {
        private IConfiguration _configuration;
        private IWebHostEnvironment _hostEnvironment;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly IRepository _repository;
        protected IMediator Mediator => HttpContext.RequestServices.GetService<IMediator>();



        public VouchersHeadprintController(IConfiguration configuration, IWebHostEnvironment hostEnvironment, ICurrentUserAccessor currentUserAccessor,IRepository repository)
        {
            _configuration = configuration;
            _hostEnvironment = hostEnvironment;
            var path = Path.Combine(hostEnvironment.ContentRootPath, "Content\\license.key");
            StiLicense.LoadFromFile(path);
            _currentUserAccessor = currentUserAccessor;
            _repository = repository;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetReportAsync([FromQuery] int id, [FromQuery] int? printType, [FromQuery] string? selectedVoucherDetailIds)
        {

            GetVouchersHeadQuery model = new GetVouchersHeadQuery();
            model.Id = id;
            model.Isprint = true;
            model.PrintType = printType;
            model.SelectedVoucherDetailIds = selectedVoucherDetailIds;
            var result = await Mediator.Send(model);

            var connectionString = "";
            if (_currentUserAccessor.GetYearId() == 3)
                connectionString = _configuration.GetConnectionString("DanaString");
            else connectionString = _configuration.GetConnectionString("DefaultString");

            //string url = _configuration.GetSection("Logo:Url").Value;
            var company = await _repository
                .GetAll<CompanyInformation>(x => x.ObjectId(_currentUserAccessor.GetCompanyId())).FirstOrDefaultAsync();
            string url = company.Logo;

            var request = WebRequest.Create(url);
            var response = request.GetResponse();
            var stream = response.GetResponseStream();
            Image image = Bitmap.FromStream(stream);

            VoucherHeadReportOtherDataModel Other = new(image);

            StiReport report = new StiReport();

            LoadFonts();
            if (printType == 2)
            {
                var data = (Print2Model)result.ObjResult;
                var path = StiNetCoreHelper.MapPath(this, $"Reports/SimpleVoucherHeadPrint.mrt");
                report.Load(path);
                report.RegBusinessObject("VoucherHead", data.Datas);
            }
            
            else
            {
                var data = (ResultModel)result.ObjResult;
                var path = StiNetCoreHelper.MapPath(this, $"Reports/125.mrt");
                report.Load(path);
                report.RegBusinessObject("VoucherHead", data.DebitDatas);
            }

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






















//        public async Task<IActionResult> GetReportAsync([FromQuery] int id, [FromQuery] int? printType, [FromQuery] string? selectedVoucherDetailIds)
//        {

//           GetVouchersHeadQuery model = new GetVouchersHeadQuery();
//            model.Id = id;
//           model.Isprint = true;
//           model.PrintType = printType;
//           model.SelectedVoucherDetailIds = selectedVoucherDetailIds;
//           var result = await Mediator.Send(model);


//            string url = _configuration.GetSection("Logo:Url").Value;
//            var request = WebRequest.Create(url);
//            var response = request.GetResponse();
//            var stream = response.GetResponseStream();
//            Image image = Bitmap.FromStream(stream);
//            LoadFonts();


//            StiReport report = new StiReport();
//            if (printType == 1)
//           {
//               var data = (ResultModel)result.ObjResult;
//               VoucherHeadReportOtherDataModel otherData = new VoucherHeadReportOtherDataModel(image, 15);
//               report.RegBusinessObject("OtherData", otherData);
//              report.RegBusinessObject("VoucherHead", data);
//           }

//           else
//           {
//               var data = (Print2Model)result.ObjResult;
//                report.RegBusinessObject("VoucherHead", data);
//              VoucherHeadReportOtherDataModel otherData = new VoucherHeadReportOtherDataModel(image,55);
//               report.RegBusinessObject("OtherData", otherData);
//           }

//            var path = StiNetCoreHelper.MapPath(this, $"Reports/VoucherHeadReport - Copy.mrt");
//            report.Load(path);



//            return StiNetCoreViewer.GetReportResult(this, report);



//        }

//        public IActionResult ViewerEvent()
//        {
//            return StiNetCoreViewer.ViewerEventResult(this);
//        }

//        private void LoadFonts()
//        {
//            var fontDirectory = Path.Combine(_hostEnvironment.ContentRootPath, "Localization\\fonts");
//            var fontFiles = Directory.GetFiles(fontDirectory);
//            foreach (var font in fontFiles)
//            {
//                StiFontCollection.AddFontFile(font);
//            }
//        }



//    }
//}
