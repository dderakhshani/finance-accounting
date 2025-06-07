using System.Threading.Tasks;
using AutoMapper;
using Eefa.Accounting.Data.Databases.Sp;
using Eefa.Accounting.Data.Databases.SqlServer.Context;
using Library.Exceptions.Interfaces;
using Library.Interfaces;
using Library.Mappings;
using Library.Models;
using Library.Utility;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Library.Exceptions;
using System;
using System.Threading;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;
using System.Collections.Generic;
using Eefa.Persistence.Data.SqlServer.QueryProvider;
using System.Diagnostics;
using StackExchange.Redis;
using Eefa.Accounting.Data.Constants;
using Eefa.Accounting.Application.UseCases.Report.Model;
using PersianDate.Standard;
using System.Data;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using System.IO;
using System.Linq.Dynamic.Core;
using Eefa.Accounting.Application.UseCases.VouchersHead.Model;
using System.Globalization;

namespace Eefa.Accounting.Application.UseCases.Report
{
    public class TadbirReportQuery : IRequest<ServiceResult>
    {
        public List<int> VoucherHeadIds { get; set; }
    }
    public class TadbirReportQueryHandler : IRequestHandler<TadbirReportQuery, ServiceResult>
    {
        private readonly IAccountingUnitOfWorkProcedures _accountingUnitOfWorkProcedures;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly IMapper _mapper;
        private readonly IAccountingUnitOfWork _context;

        public TadbirReportQueryHandler(IAccountingUnitOfWorkProcedures accountingUnitOfWorkProcedures, ICurrentUserAccessor currentUserAccessor, IMapper mapper, IAccountingUnitOfWork context)
        {
            _accountingUnitOfWorkProcedures = accountingUnitOfWorkProcedures;
            _currentUserAccessor = currentUserAccessor;

            _mapper = mapper;
            _context = context;
        }
        public async Task<ServiceResult> Handle(TadbirReportQuery request, CancellationToken cancellationToken)
        {

            var voucherDetails = await _context.VouchersDetails.OrderBy(x => x.Voucher.VoucherNo).ThenBy(x => x.RowIndex).Where(x => request.VoucherHeadIds.Contains(x.VoucherId) && !x.IsDeleted).Select(x => new
            {
                AccountHeadCode = x.AccountHead.Code,
                AccountReferenceCode = x.ReferenceId1Navigation.Code,
                AccountReferenceGroupCode = x.AccountReferencesGroup.Code,
                Credit = x.Credit,
                Debit = x.Debit,
                Description = x.VoucherRowDescription,
                VoucherHeadDescription = x.Voucher.VoucherDescription,
                Date = x.Voucher.VoucherDate,
                VoucherNumber = x.Voucher.VoucherNo,

            }).ToListAsync();

            var result = new List<TadbirReportModel>();


            foreach (var voucherDetail in voucherDetails)
            {
                var persianDateString = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(voucherDetail.Date, "Iran Standard Time").ToString("yyyy/MM/dd", new CultureInfo("fa-IR"));

                var reportModel = new TadbirReportModel
                {
                    Credit = voucherDetail.Credit.ToString(),
                    Debit = voucherDetail.Debit.ToString(),
                    VoucherDetailDescription = voucherDetail.Description,
                    // TODO convert to persian date
                    VoucherDate = persianDateString,
                    VoucherDescription = voucherDetail.VoucherHeadDescription,
                    VoucherNo = voucherDetail.VoucherNumber,
                    ProjectCode = "0",
                    CostCenterCode = "0"

                };
                if (voucherDetail.AccountReferenceCode != null)
                {
                    var accountReferenceTadbirMap = await _context.MapDanaToTadbir.Select(x => new { x.DanaCode, x.Levelcode, x.TadbirCode, x.IsDeleted }).FirstOrDefaultAsync(x => x.DanaCode == voucherDetail.AccountReferenceCode && !x.IsDeleted);
                    if (accountReferenceTadbirMap != null)
                    {
                        reportModel.AccountHeadCode = accountReferenceTadbirMap.TadbirCode;
                    }
                    else
                    {
                        var accountHeadMaps = await _context.MapDanaToTadbir.Where(x => x.DanaCode == voucherDetail.AccountHeadCode && !x.IsDeleted).Select(x => new
                        {
                            x.TadbirCode,
                            x.Levelcode,
                            x.Condition
                        }).ToListAsync();
                        dynamic accountHeadMap = null;
                        if (accountHeadMaps.Count <= 1)
                        {
                            accountHeadMap = accountHeadMaps.FirstOrDefault();
                        }
                        if (accountHeadMaps.Count > 1)
                        {
                            accountHeadMap = accountHeadMaps.FirstOrDefault(x => !string.IsNullOrEmpty(x.Condition) && x.Condition.Contains("AccountReferenceGroupCode=" + voucherDetail.AccountReferenceGroupCode));
                        }

                        string accountHeadTadbirCode = accountHeadMap?.TadbirCode ?? string.Empty;
                        if (accountHeadTadbirCode == null) throw new ApplicationValidationException(new ApplicationErrorModel { Message = $"کد تدبیر برای حساب  {voucherDetail.AccountHeadCode} یافت نشد." });

                        if (accountHeadTadbirCode == "AccountReferenceGroupTadbirCode" && !string.IsNullOrEmpty(voucherDetail.AccountReferenceGroupCode)) accountHeadTadbirCode = await _context.AccountReferencesGroups.Where(x => x.Code == voucherDetail.AccountReferenceGroupCode).Select(x => x.TadbirCode).FirstOrDefaultAsync();

                        reportModel.AccountHeadCode = accountHeadTadbirCode;

                        string accountHeadMapCondition = accountHeadMap?.Condition ?? string.Empty;
                        if (reportModel.AccountHeadCode?.Length <= 5 || accountHeadMapCondition.Contains("ForcefullyUseAccountReference")) reportModel.AccountRefrenceCode = voucherDetail.AccountReferenceCode;
                    }
                }
                else
                {
                    var tadbirCode = await _context.MapDanaToTadbir.Select(x => new { x.DanaCode, x.Levelcode, x.TadbirCode, x.IsDeleted }).Where(x => x.DanaCode == voucherDetail.AccountHeadCode && !x.IsDeleted).Select(x => x.TadbirCode).FirstOrDefaultAsync();
                    if (tadbirCode == null) throw new ApplicationValidationException(new ApplicationErrorModel { Message = $"کد تدبیر برای حساب  {voucherDetail.AccountHeadCode} یافت نشد." });
                    reportModel.AccountHeadCode = tadbirCode;
                }

                result.Add(reportModel);
            }



            var exceldata = GetExcelDataRial(result);
            string base64String;
            using (var wb = new XLWorkbook())
            {
                wb.RightToLeft = true;
                wb.Author = _currentUserAccessor.GetUsername();
                wb.RowHeight = 20;

                var sheet = wb.AddWorksheet(exceldata, "گزارش برای تدبیر");
                using (var ms = new MemoryStream())
                {
                    wb.SaveAs(ms);
                    base64String = Convert.ToBase64String(ms.ToArray());
                }
            }

            return ServiceResult.Success(base64String);

        }
        private DataTable GetExcelDataRial(List<TadbirReportModel> input)
        {
            DataTable data = new DataTable();

            data.TableName = "گزارش روزانه برای تدبیر";
            data.Columns.Add("شماره سند", typeof(string));
            data.Columns.Add("تاریخ", typeof(string));
            data.Columns.Add("شرح سند", typeof(string));
            data.Columns.Add("کدحساب", typeof(string));

            data.Columns.Add("کد تفصیلی", typeof(string));
            data.Columns.Add("کد مرکز هزینه", typeof(string));
            data.Columns.Add("کد پروژ", typeof(string));
            data.Columns.Add("بدهکار", typeof(string));
            data.Columns.Add("بستانکار", typeof(string));
            data.Columns.Add("شرح آرتیکل", typeof(string));

            for (int i = 0; i < input.Count; i++)
            {
                data.Rows.Add(input[i].VoucherNo, input[i].VoucherDate, input[i].VoucherDescription, input[i].AccountHeadCode,
                    input[i].AccountRefrenceCode, input[i].CostCenterCode, input[i].ProjectCode, input[i].Debit, input[i].Credit, input[i].VoucherDetailDescription);
            }


            return data;
        }
    }
}
