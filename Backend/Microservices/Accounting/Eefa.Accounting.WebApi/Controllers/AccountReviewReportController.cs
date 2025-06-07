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
using System.Collections.Generic;
using Eefa.Accounting.Data.Databases.Sp;
using System.Linq;
using Eefa.Accounting.WebApi.ReportModels;
using Eefa.Accounting.Application.UseCases.VouchersHead.Query.GetAll;
using Eefa.Accounting.Application.UseCases.VouchersHead.Query.GetNo;
using Eefa.Accounting.Application.UseCases.VouchersHead.Model;
using System.Drawing;
using System.Net;
using System.Security.Policy;
using DocumentFormat.OpenXml.Office2010.Excel;
using Stimulsoft.System.Windows.Forms;
using Eefa.Accounting.Application.UseCases.AccountHead.Query.Get;
using Eefa.Accounting.Application.UseCases.AccountHead.Model;
using Eefa.Accounting.Application.UseCases.AccountReference.Model;
using Eefa.Accounting.Application.UseCases.AccountReferencesGroup.Query.Get;
using Eefa.Accounting.Application.UseCases.AccountReferencesGroup.Model;
using Eefa.Accounting.Application.UseCases.AccountReference.Query.Get;

namespace Eefa.Accounting.WebApi.Controllers
{
    [Route("accountingreports/[controller]/[action]")]
    [Authorize]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class AccountReviewReportController : Controller
    {
        private IConfiguration _configuration;
        private IWebHostEnvironment _hostEnvironment;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        protected IMediator Mediator => HttpContext.RequestServices.GetService<IMediator>();
        public AccountReviewReportController(IConfiguration configuration, IWebHostEnvironment hostEnvironment, ICurrentUserAccessor currentUserAccessor)
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
        public async Task<IActionResult> GetReportAsync([FromQuery] int Column, [FromQuery] string AccountHeadIds, [FromQuery] string CodeVoucherGroupIds, [FromQuery] int? CurrencyTypeBaseId,
           [FromQuery] int Level, [FromQuery] string ReferenceGroupIds, [FromQuery] string ReferenceIds, [FromQuery] DateTime fromDate, [FromQuery] DateTime toDate,
           [FromQuery] int? VoucherNoFrom, [FromQuery] int? VoucherNoTo)
        {
            GetNoResultModel noResult = new(0, 0);
            if (VoucherNoFrom == null || VoucherNoTo == null)
            {
                GetVouchersHeadNoByDateQuery nomodel = new();
                nomodel.FromDateTime = fromDate;
                nomodel.ToDateTime = toDate;
                var noresult = await Mediator.Send(nomodel);
                noResult = (GetNoResultModel)noresult.ObjResult;
            }
            GetAccountReviewQuery model = new GetAccountReviewQuery();
            model.CompanyId = _currentUserAccessor.GetCompanyId();
            model.YearIds = new int[1];
            model.YearIds[0] = _currentUserAccessor.GetYearId();
            model.AccountHeadIds = AccountHeadIds != null ? Array.ConvertAll(AccountHeadIds.Split(','), a => int.Parse(a)) : null;
            model.CodeVoucherGroupIds = CodeVoucherGroupIds != null ? Array.ConvertAll(CodeVoucherGroupIds.Split(','), a => int.Parse(a)) : null;
            model.CurrencyTypeBaseId = CurrencyTypeBaseId;
            model.Level = Level;
            model.ReferenceGroupIds = ReferenceGroupIds != null ? Array.ConvertAll(ReferenceGroupIds.Split(','), a => int.Parse(a)) : null;
            model.ReferenceIds = ReferenceIds != null ? Array.ConvertAll(ReferenceIds.Split(','), a => int.Parse(a)) : null;
            model.VoucherDateFrom = fromDate;
            model.VoucherDateTo = toDate;
            model.VoucherNoFrom = VoucherNoFrom;
            model.VoucherNoTo = VoucherNoTo;
            var result = await Mediator.Send(model);
            var datas = (List<StpReportBalance6Result>)result.ObjResult;
            datas = datas.OrderBy(a => a.Code).ToList();
            if (CurrencyTypeBaseId != 28306)
            {
                for (int i = 0; i < datas.Count; i++)
                {
                    datas[i].DebitBeforeDate = datas[i].DebitCurrencyAmountBefore;
                    datas[i].Debit = datas[i].DebitCurrencyAmount;
                    datas[i].RemainDebit = datas[i].DebitCurrencyRemain;
                    datas[i].DebitAfterDate = datas[i].DebitCurrencyAmountAfter;


                    datas[i].CreditBeforeDate = datas[i].CreditCurrencyAmountBefore;
                    datas[i].Credit = datas[i].CreditCurrencyAmount;
                    datas[i].RemainCredit = datas[i].CreditCurrencyRemain;
                    datas[i].CreditAfterDate = datas[i].CreditCurrencyAmountAfter;
                }
            }

            string filter = "";
            if (AccountHeadIds != null)
            {
                var accountHeadIds = AccountHeadIds.Split(",");
                GetAccountHeadQuery getAccount = new();
                foreach (var accountHeadId in accountHeadIds)
                {
                    getAccount.Id = int.Parse(accountHeadId.Trim());
                    var accountresult = await Mediator.Send(getAccount);
                    var accounthead = (AccountHeadModel)accountresult.ObjResult;
                    if (!filter.Contains(accounthead.ParentCode))
                        filter += accounthead.ParentTitle + " " + accounthead.ParentCode + " - ";
                }
            }

            if (ReferenceGroupIds != null)
            {
                var referenceGroupIds = ReferenceGroupIds.Split(",");
                GetAccountReferencesGroupQuery getAccountRefrenceGroup = new();
                foreach (var referenceGroupId in referenceGroupIds)
                {
                    getAccountRefrenceGroup.Id = int.Parse(referenceGroupId.Trim());
                    var accountRefrencesGroupResult = await Mediator.Send(getAccountRefrenceGroup);
                    var accountRefrenceGroup = (AccountReferencesGroupModel)accountRefrencesGroupResult.ObjResult;
                    filter += accountRefrenceGroup.Title + " " + accountRefrenceGroup.Code + " - ";
                }
            }

            if (ReferenceIds != null)
            {
                var referenceIds = ReferenceIds.Split(",");
                GetAccountReferenceQuery getAccountReference = new();
                foreach (var referenceId in referenceIds)
                {
                    getAccountReference.Id = int.Parse(referenceId);
                    var accountRefrenceResult = await Mediator.Send(getAccountReference);
                    var accountRefrence = (AccountReferenceModel)accountRefrenceResult.ObjResult;
                    filter += accountRefrence.Title + " " + accountRefrence.Code + " - ";
                }
            }
            filter = filter.Length > 0 ? filter.Substring(0, filter.Length - 2) : "";

            string levelName = "";
            if (Level == 1)
                levelName = "گروه";
            else if (Level == 2)
                levelName = "کل";
            else if (Level == 3)
                levelName = "معین";
            else if (Level == 4)
                levelName = "تفصیل";

            string url = _configuration.GetSection("Logo:Url").Value;
            var request = WebRequest.Create(url);
            var response = request.GetResponse();
            var stream = response.GetResponseStream();
            Image image = Bitmap.FromStream(stream);

            AccountReviewReportModel otherData = new AccountReviewReportModel(fromDate.AddDays(1).AddSeconds(-1), toDate, levelName, 
                VoucherNoFrom == null ? noResult.MinNo.ToString() : VoucherNoFrom.ToString(),
                VoucherNoTo == null ? noResult.MaxNo.ToString() : VoucherNoTo.ToString(), image, filter);
            LoadFonts();

            StiReport report = new StiReport();
            if (Column == 4)
            {
                var path = StiNetCoreHelper.MapPath(this, $"Reports/accountReviewReport.mrt");
                report.Load(path);
            }
            else if (Column == 6)
            {
                var path = StiNetCoreHelper.MapPath(this, $"Reports/accountReviewReport6.mrt");
                report.Load(path);
            }
            else
            {
                var path = StiNetCoreHelper.MapPath(this, $"Reports/accountReviewReport8.mrt");
                report.Load(path);
            }
            report.RegBusinessObject("AccountReview", datas);
            report.RegBusinessObject("OtherData", otherData);

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
