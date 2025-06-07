using System.Globalization;
using System;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Accounting.Application.UseCases.Report;
using Eefa.Accounting.Data.Databases.Sp;
using Eefa.Accounting.Data.Entities;
using Library.Interfaces;
using Library.Utility;
using LinqToDB;
using Microsoft.AspNetCore.Mvc;
using Eefa.Accounting.Application.CommandQueries.Report;
using Eefa.Accounting.Application.Common.Extensions;
using Library.Models;
namespace Eefa.Accounting.WebApi.Controllers
{
    public class ReportingController : AccountingBaseController
    {
        private readonly IMapper _mapper;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly IRepository _repository;
        public ReportingController(IMapper mapper, ICurrentUserAccessor currentUserAccessor, IRepository repository)
        {
            _mapper = mapper;
            _currentUserAccessor = currentUserAccessor;
            _repository = repository;
        }

        [HttpPost]
        //[Authorize(Roles = "Reporting-*,Reporting-GetAccountReview")]
        public async Task<IActionResult> GetAccountReferenceBook([FromBody] GetAccountReferenceBookQuery model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        //[Authorize(Roles = "Reporting-*,Reporting-GetAccountReview")]
        public async Task<IActionResult> GetBankReport([FromBody] BankReportQuery model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpPost]
        //[Authorize(Roles = "Reporting-*,Reporting-GetAccountReview")]
        public async Task<IActionResult> GetAccountReview([FromBody] GetAccountReviewQuery model)
        {
            if (model.ReportFormat == SsrsUtil.ReportFormat.None)
            {
                return Ok(await Mediator.Send(model));
            }
            else
            {
                return await new SsrsUtil().GetReport(_mapper.Map<StpReportBalance6Input>(model), "Acc_Balance_6", model.ReportFormat);
            }
        }


        [HttpPost]
        //[Authorize(Roles = "Reporting-*,Reporting-GetCentralBankReport")]
        public async Task<IActionResult> GetCentralBankReport([FromBody] GetCentralBankReportQuery model)
        {
            if (model.ReportFormat == SsrsUtil.ReportFormat.None)
            {
                return Ok(await Mediator.Send(model));
            }
            else
            {
                return await new SsrsUtil().GetReport(_mapper.Map<StpReportBalance6Input>(model), "Acc_Balance_6", model.ReportFormat);
            }
        }



        [HttpPost]
        //[Authorize(Roles = "Reporting-*,Reporting-GetReportBalance6")]
        public async Task<IActionResult> GetReportBalance4([FromBody] GetReportBalance6Query model)
        {
            if (model.ReportFormat == SsrsUtil.ReportFormat.None)
            {
                return Ok(await Mediator.Send(model));
            }
            else
            {
                var stpModel = _mapper.Map<StpReportBalance6Input>(model);
                stpModel = await ReportBaseGenerator(stpModel);
                return await new SsrsUtil().GetReport(stpModel, "Acc_Balance_4", model.ReportFormat);
            }
        }


        [HttpPost]
        //[Authorize(Roles = "Reporting-*,Reporting-GetReportBalance6")]
        public async Task<IActionResult> GetReportBalance6([FromBody] GetReportBalance6Query model)
        {
            if (model.ReportFormat == SsrsUtil.ReportFormat.None)
            {
                return Ok(await Mediator.Send(model));
            }
            else
            {
                var stpModel = _mapper.Map<StpReportBalance6Input>(model);
                stpModel = await ReportBaseGenerator(stpModel);
                return await new SsrsUtil().GetReport(stpModel, "Acc_Balance_6", model.ReportFormat);
            }
        }

        [HttpPost]
        //[Authorize(Roles = "Reporting-*,Reporting-GetReportReference2AccountHead")]
        public async Task<IActionResult> GetReportReference2AccountHead([FromBody] StpReportReference2AccountHeadQuery model)
        {
            if (model.ReportFormat == SsrsUtil.ReportFormat.None)
            {
                return Ok(await Mediator.Send(model));
            }
            else
            {
                var stpModel = _mapper.Map<StpReportReference2AccountHeadInput>(model);
                stpModel = await ReportBaseGenerator(stpModel);
                return await new SsrsUtil().GetReport(stpModel, "Report1", model.ReportFormat);
            }
        }

        [HttpPost]
        //[Authorize(Roles = "Reporting-*,Reporting-GetReportLedger")]
        public async Task<IActionResult> GetReportLedger([FromBody] GetReportLedgerQuery model)
        {
            if (model.ReportFormat == SsrsUtil.ReportFormat.None)
            {
                return Ok(await Mediator.Send(model));
            }
            else
            {
                var stpModel = _mapper.Map<StpReportLedgerInput>(model);
                stpModel = await ReportBaseGenerator(stpModel);
                return await new SsrsUtil().GetReport(stpModel, "Acc_Ledger_1", model.ReportFormat);
            }
        }
        [HttpPost]
        //[Authorize(Roles = "Reporting-*,Reporting-GetReportLedger")]
        public async Task<IActionResult> GetReportJournal([FromBody] GetReportLedgerQuery model)
        {
            if (model.ReportFormat == SsrsUtil.ReportFormat.None)
            {
                return Ok(await Mediator.Send(model));
            }
            else
            {
                var stpModel = _mapper.Map<StpReportLedgerInput>(model);
                stpModel = await ReportBaseGenerator(stpModel);
                return await new SsrsUtil().GetReport(stpModel, "Acc_Journal", model.ReportFormat);
            }
        }

        [HttpPost]
        //[Authorize(Roles = "Reporting-*,Reporting-GetReportLedger")]
        public async Task<IActionResult> GetReportJournalGroup([FromBody] GetReportLedgerQuery model)
        {
            if (model.ReportFormat == SsrsUtil.ReportFormat.None)
            {
                return Ok(await Mediator.Send(model));
            }
            else
            {
                var stpModel = _mapper.Map<StpReportLedgerInput>(model);
                stpModel = await ReportBaseGenerator(stpModel);
                return await new SsrsUtil().GetReport(stpModel, "Acc_JournalGroup", model.ReportFormat);
            }
        }

        [HttpGet]
        public async Task<IActionResult> PrintVoucherHead([FromQuery] int id)
        {
            var stpModel = new stpAccVoucherInput
            {
                Id = id,
            };
            stpModel = await ReportBaseGenerator(stpModel);

            return await new SsrsUtil().GetReport(stpModel, "Acc_VoucherCover_2", SsrsUtil.ReportFormat.Pdf);
        }
        [HttpPost]
        public async Task<IActionResult> ProfitAndLossReport(ProfitAndLossReportQuery model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        public async Task<IActionResult> AnnualLedgerReport(AnnualLedgerReportQuery model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        public async Task<IActionResult> TadbirReport(TadbirReportQuery model)
        {
            return Ok(await Mediator.Send(model));
        }
        private async Task<TEntity> ReportBaseGenerator<TEntity>(TEntity model) where TEntity : IReportSpBase
        {
            model.PersianDateFrom = model.VoucherDateFrom?.ToString("yyyy/MM/dd", new CultureInfo("fa-IR"));
            model.PersianDateTo = model.VoucherDateTo?.ToString("yyyy/MM/dd", new CultureInfo("fa-IR"));
            model.ReportTime = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture);
            model.UserName = _currentUserAccessor.GetUsername();
            model.YearName = (await _repository.GetQuery<Year>()
                .FirstOrDefaultAsync(x => x.Id == _currentUserAccessor.GetYearId()))?.YearName.ToString();
            model.CompanyName = (await _repository.GetQuery<CompanyInformation>()
                .FirstOrDefaultAsync(x => x.Id == _currentUserAccessor.GetCompanyId()))?.Title.ToString();
            return model;
        }


        [HttpGet]
        public async Task<ServiceResult> GetSSRSReportAddress([FromQuery] string reportName = "acc_accounthead")
        {
            var url = "http://192.168.2.151:5020/default.aspx";

            if (!string.IsNullOrEmpty(reportName))
            {
                url += $"?ssrsreportname={reportName}";
                url += $"&userId={_currentUserAccessor.GetId().ToString().Encrypt("108B9E85E20540A3")}";
                url += $"&roleId={_currentUserAccessor.GetRoleId().ToString().Encrypt("108B9E85E20540A3")}";
            }

            return ServiceResult.Success(url);

        }

        [HttpPost]
        public async Task<IActionResult> VoucherDetails([FromBody] VoucherDetailsReportQuery query) => Ok(await Mediator.Send(query));
    }
}