using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Accounting.Data.Databases.Sp;
using Eefa.Accounting.Data.Databases.SqlServer.Context;
using Library.Interfaces;
using Library.Mappings;
using Library.Models;
using Library.Utility;
using MediatR;
using Newtonsoft.Json;
using Library.Exceptions;
using Microsoft.EntityFrameworkCore;
using Library.Common;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using ClosedXML.Excel;
using System.IO;
using MassTransit.Initializers;
using Eefa.Accounting.Application.UseCases.Report.Model;
using DocumentFormat.OpenXml.Office2013.Word;
namespace Eefa.Accounting.Application.UseCases.Report
{
    public class GetCentralBankReportQuery : Pagination, IMapFrom<GetCentralBankReportQuery>, IRequest<ServiceResult>, ISearchableRequest, IQuery
    {
        public int ReportType { get; set; }
        public int Level { get; set; }
        public int AccountReferencesGroupflag { get; set; }
        public int CompanyId { get; set; }
        public int[]? YearIds { get; set; }
        public int? VoucherStateId { get; set; }
        public int[]? CodeVoucherGroupIds { get; set; }
        public int? TransferId { get; set; }
        public int[]? AccountHeadIds { get; set; }
        public int[]? ReferenceGroupIds { get; set; }
        public int[]? ReferenceIds { get; set; }
        public int? ReferenceNo { get; set; } = 1;
        public int? VoucherNoFrom { get; set; }
        public int? VoucherNoTo { get; set; }
        public DateTime? VoucherDateFrom { get; set; }
        public DateTime? VoucherDateTo { get; set; }
        public long? DebitFrom { get; set; }
        public long? DebitTo { get; set; }
        public long? CreditFrom { get; set; }
        public long? CreditTo { get; set; }
        public int? DocumentIdFrom { get; set; }
        public int? DocumentIdTo { get; set; }
        public int? CurrencyTypeBaseId { get; set; }

        public string? VoucherDescription { get; set; }
        public string? VoucherRowDescription { get; set; }
        public bool Remain { get; set; }
        public string? ReportTitle { get; set; }
        public SsrsUtil.ReportFormat ReportFormat { get; set; } = SsrsUtil.ReportFormat.None;
        public void Mapping(Profile profile)
        {
            profile.CreateMap<GetCentralBankReportQuery, StpReportBalance6Input>()
                .ForMember(x => x.YearIds, opt => opt.MapFrom(t => (t.YearIds != null && t.YearIds.Length != 0) ? JsonConvert.SerializeObject(t.YearIds) : null))
                .ForMember(x => x.AccountHeadIds, opt => opt.MapFrom(t => (t.AccountHeadIds != null && t.AccountHeadIds.Length != 0) ? JsonConvert.SerializeObject(t.AccountHeadIds) : null))
                .ForMember(x => x.ReferenceGroupIds, opt => opt.MapFrom(t => (t.ReferenceGroupIds != null && t.ReferenceGroupIds.Length != 0) ? JsonConvert.SerializeObject(t.ReferenceGroupIds) : null))
                .ForMember(x => x.ReferenceIds, opt => opt.MapFrom(t => (t.ReferenceIds != null && t.ReferenceIds.Length != 0) ? JsonConvert.SerializeObject(t.ReferenceIds) : null))
                .ForMember(x => x.CodeVoucherGroupIds, opt => opt.MapFrom(t => (t.CodeVoucherGroupIds != null && t.CodeVoucherGroupIds.Length != 0) ? JsonConvert.SerializeObject(t.CodeVoucherGroupIds) : null))
                .IgnoreAllNonExisting();
        }
    }

    public class GetCentralBankReportQueryHandler : IRequestHandler<GetCentralBankReportQuery, ServiceResult>
    {
        private readonly IAccountingUnitOfWorkProcedures _accountingUnitOfWorkProcedures;
        private readonly IAccountingUnitOfWork _unitOfWork;
        private readonly ICurrentUserAccessor _currentUser;
        private readonly IMapper _mapper;
        private readonly IAccountingUnitOfWork _context;
        private readonly DanaAccountingUnitOfWork _danaContext;

        public GetCentralBankReportQueryHandler(IAccountingUnitOfWorkProcedures accountingUnitOfWorkProcedures, IAccountingUnitOfWork unitOfWork, ICurrentUserAccessor currentUserAccessor, IMapper mapper, IAccountingUnitOfWork context, DanaAccountingUnitOfWork danaContext)
        {
            _accountingUnitOfWorkProcedures = accountingUnitOfWorkProcedures;
            this._unitOfWork = unitOfWork;
            _currentUser = currentUserAccessor;

            _mapper = mapper;
            _context = context;
            this._danaContext = danaContext;
        }

        public async Task<ServiceResult> Handle(GetCentralBankReportQuery request, CancellationToken cancellationToken)
        {

            PermissionExtention permission = new();
            string permissions = permission.GetPermissions<Data.Entities.VouchersDetail>(_context, _currentUser);


            if (_currentUser.GetYearId() == 3) request.YearIds = await _danaContext.Years.Where(x => x.FirstDate <= request.VoucherDateTo && x.LastDate >= request.VoucherDateFrom && x.UserYears.Any(x => x.UserId == _currentUser.GetId())).Select(a => a.Id).ToArrayAsync();
            else request.YearIds = await _context.Years.Where(x => x.FirstDate <= request.VoucherDateTo && x.LastDate >= request.VoucherDateFrom && x.UserYears.Any(x => x.UserId == _currentUser.GetId())).Select(a => a.Id).ToArrayAsync();

            if (request.VoucherDateTo.HasValue)
                request.VoucherDateTo = request.VoucherDateTo.Value.AddDays(1).AddSeconds(-1);


            var input = _mapper.Map<StpReportBalance6Input>(request);
            input.CompanyId = _currentUser.GetCompanyId();


            if (!string.IsNullOrEmpty(permissions))
                input.UserAccessCondition = permissions;

            var results = await _accountingUnitOfWorkProcedures
                .StpReportBalance6Async(input,
                    cancellationToken: cancellationToken
                );


            var report = results.Select(x => new CentralBankReportModel
            {
                Code = x.Code,
                Credit = x.Credit,
                Debit = x.Debit,
                RemainCredit = x.RemainCredit,
                RemainDebit = x.RemainDebit,
            }).ToList();

            var accountReferencesIds = await _unitOfWork.AccountReferences.Where(x => report.Select(x => x.Code).ToList().Contains(x.Code)).Select(x => new
            {
                x.Id,
                x.Code
            }).ToListAsync();

            var persons = await _unitOfWork.Persons.Where(x => x.AccountReferenceId != null && accountReferencesIds.Select(x => x.Id).Contains((int)x.AccountReferenceId))
                .Select(x => new
                {
                    Code = x.AccountReference.Code,
                    Name = x.LastName,
                    x.EconomicCode,
                    x.NationalNumber,
                    PersonType = x.LegalBaseId == 28100 ? 2 : 1 ,
                    Address = x.PersonAddresses.FirstOrDefault().Address ?? "",
                    Phone = x.PersonPhones.FirstOrDefault().PhoneNumber ?? ""
                }).ToListAsync();


            report = report.Where(x => persons.Select(x =>x.Code).Contains(x.Code)).Select(x =>
            {
                var person = persons.FirstOrDefault(y => y.Code == x.Code);
                x.Name = person.Name;
                x.Address = person.Address;
                x.Phone = person.Phone; 
                x.NationalNumber = person.NationalNumber;
                x.EconomicCode = person.EconomicCode;
                
                return x;
            }).ToList();


            DataTable data = new DataTable();

            data.TableName = "گزارش بانک مرکزی";
            data.Columns.Add("Radif", typeof(double));
            data.Columns.Add("Sarjam", typeof(bool));
            data.Columns.Add("IsHagholAmalKari", typeof(bool));
            data.Columns.Add("KalaType", typeof(double));
            data.Columns.Add("KalaKhadamatName", typeof(string));
            data.Columns.Add("KalaCode", typeof(double));
            data.Columns.Add("BargashtType", typeof(bool));
            data.Columns.Add("Price", typeof(double));
            data.Columns.Add("MaliatArzeshAfzoodeh", typeof(double));
            data.Columns.Add("AvarezArzeshAfzoodeh", typeof(double));
            data.Columns.Add("SayerAvarez", typeof(double));
            data.Columns.Add("TakhfifPrice", typeof(double));
            data.Columns.Add("MaliatMaksoore", typeof(double));
            data.Columns.Add("HCForoushandeTypeCode", typeof(double));
            data.Columns.Add("ForoushandePostCode", typeof(string));
            data.Columns.Add("ForoushandePerCityCode", typeof(string));
            data.Columns.Add("ForoushandeTell", typeof(string));
            data.Columns.Add("ForoushandeAddress", typeof(string));
            data.Columns.Add("ForoushandeName", typeof(string));
            data.Columns.Add("ForoushandeLastNameSherkatName", typeof(string));
            data.Columns.Add("ForoushandeEconomicNO", typeof(string));
            data.Columns.Add("ForoushandeNationalCode", typeof(string));
            data.Columns.Add("HCForoushandeType1Code", typeof(string));
            data.Columns.Add("StateCode", typeof(string));
            data.Columns.Add("CityCode", typeof(string));
            data.Columns.Add("ArzType", typeof(string));
            data.Columns.Add("Arz_Price", typeof(string));
            data.Columns.Add("Arz_MaliatArzeshAfzoodeh", typeof(string));
            data.Columns.Add("Arz_AvarezArzeshAfzoodeh", typeof(string));
            data.Columns.Add("Arz_SayerAvarez", typeof(string));
            data.Columns.Add("Arz_TakhfifPrice", typeof(string));
            data.Columns.Add("Arz_MaliatMaksoore", typeof(string));
            data.Columns.Add("ArzBarabari_Price", typeof(string));
            data.Columns.Add("ArzBarabari_TakhfifPrice", typeof(string));
            data.Columns.Add("ArzBarabari_MaliatArzeshAfzoodeh", typeof(string));
            data.Columns.Add("ArzBarabari_AvarezArzeshAfzoodeh", typeof(string));
            data.Columns.Add("ArzBarabari_SayerAvarez", typeof(string));
            data.Columns.Add("ArzBarabari_MaliatMaksoore", typeof(string));
            data.Columns.Add("MoadelRialiPrice", typeof(string));
            data.Columns.Add("MoadelRialiTakhfifPrice", typeof(string));
            data.Columns.Add("MoadelRialiMaliatArzeshAfzoodeh", typeof(string));
            data.Columns.Add("MoadelRialiAvarezArzeshAfzoodeh", typeof(string));
            data.Columns.Add("MoadelRialiSayerAvarez", typeof(string));
            data.Columns.Add("MoadelRialiMaliatMaksoore", typeof(string));
            data.Columns.Add("FactorNo", typeof(string));
            data.Columns.Add("FactorDate", typeof(string));
            data.Columns.Add("SanadNO", typeof(string));
            data.Columns.Add("SanadDate", typeof(string));
            data.Columns.Add("IsSent", typeof(bool));
            data.Columns.Add("AccountReferenceCode", typeof(string));



            foreach (var item in report.Select((value, i) => new {i , person= value }))
            {
                data.Rows.Add(
                    item.i,
                    false,
                    false,
                    12,
                    "",
                    1,
                    false,
                    item.person.Credit * .9,
                    item.person.Credit * .1,
                    0,
                    0,
                    0,
                    0,
                    item.person.PersonType,
                    "",
                    "",
                    item.person.Phone,
                    item.person.Address[0..Math.Min(item.person.Address.Length, 80)],
                    item.person.Name[0..Math.Min(item.person.Name.Length, 50)],
                    "",
                    item.person.EconomicCode,
                    item.person.NationalNumber,
                    5,
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    false,
                    item.person.Code
                    );
            }
            string base64String;


            using (var wb = new XLWorkbook())
            {
                wb.RightToLeft = true;
                wb.Author = _currentUser.GetUsername();
                wb.RowHeight = 22.5;


                var sheet = wb.AddWorksheet("دفتر تفصیلی مطابق ردیفهای سند");


                var headerColor = XLColor.FromHtml("#6D72ED");
                var alternateRowColor = XLColor.FromHtml("#F2F2FE");


                for (int col = 0; col < data.Columns.Count; col++)
                {
                    var headerCell = sheet.Cell(1, col + 1);
                    headerCell.Value = data.Columns[col].ColumnName;
                    headerCell.Style.Font.Bold = true;
                    headerCell.Style.Fill.BackgroundColor = headerColor;
                    headerCell.Style.Font.FontColor = XLColor.White;
                    headerCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                }


                for (int row = 0; row < data.Rows.Count; row++)
                {
                    for (int col = 0; col < data.Columns.Count; col++)
                    {
                        var cell = sheet.Cell(row + 2, col + 1);
                        var value = data.Rows[row][col];


                        if (value is int || value is long)
                        {
                            cell.Value = Convert.ToInt64(value);
                            cell.Style.NumberFormat.SetFormat("#,##0");
                        }
                        else if (value is decimal || value is double)
                        {
                            double numericValue = Convert.ToDouble(value);
                            cell.Value = numericValue;
                            if (Math.Floor(numericValue) == numericValue)
                            {
                                cell.Style.NumberFormat.SetFormat("#,##0");
                            }
                            else
                            {
                                cell.Style.NumberFormat.SetFormat("#,##0.00");
                            }
                        }
                        else if (value is DateTime)
                        {
                            cell.Value = (DateTime)value;
                            cell.Style.DateFormat.Format = "yyyy-mm-dd";
                        }
                        else
                        {
                            cell.Value = value.ToString();
                        }
                        if (row % 2 == 1)
                        {
                            cell.Style.Fill.BackgroundColor = alternateRowColor;
                        }
                        cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    }
                }
                double maxWidth = 50;
                foreach (var column in sheet.ColumnsUsed())
                {
                    column.AdjustToContents();
                    if (column.Width > maxWidth)
                    {
                        column.Width = maxWidth;
                    }
                }

                using (var ms = new MemoryStream())
                {
                    wb.SaveAs(ms);
                    base64String = Convert.ToBase64String(ms.ToArray());
                }
            }
            return ServiceResult.Success(base64String);
        }
    }
}