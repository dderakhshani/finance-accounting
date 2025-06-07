using ClosedXML.Excel;
using Library.Interfaces;
using Library.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Eefa.Admin.Application.UseCases.Person.Query.GetCentralBankReport
{
    public class GetCentralBankReportQuery : IRequest<ServiceResult>, IQuery
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public List<int> AccountHeadIds { get; set; }
        public List<int> AccountReferenceIds { get; set; }
        public List<int> AccountReferenceGroupIds { get; set; }
    }
    public class GetCentralBankReportQueryHandler : IRequestHandler<GetCentralBankReportQuery, ServiceResult>
    {
        private readonly IRepository _repository;
        public ICurrentUserAccessor _currentUser { get; }

        public GetCentralBankReportQueryHandler(IRepository repository, ICurrentUserAccessor currentUser)
        {
            this._repository = repository;
            _currentUser = currentUser;
        }


        public async Task<ServiceResult> Handle(GetCentralBankReportQuery request, CancellationToken cancellationToken)
        {
            var query = _repository
                .GetAll<Data.Databases.Entities.Person>()
                .Include(x => x.PersonAddresses)
                .Include(x => x.PersonPhones)
                .Include(x => x.AccountReference).ThenInclude(x => x.VouchersDetailReferences)
              ;

            var people = await query.Select(x => new
            {
                Name = x.LastName,
                x.NationalNumber,
                x.EconomicCode,
                AccountReferenceCode = x.AccountReference.Code,
                x.AccountReferenceId,
                Address = x.PersonAddresses.FirstOrDefault().Address,
                Phone = x.PersonPhones.FirstOrDefault().PhoneNumber,
                Credit = x.AccountReference.VouchersDetailReferenceId1Navigation
                .Where(x => (request.AccountHeadIds.Count > 0) ? request.AccountHeadIds.Contains(x.AccountHeadId) : true)
                .Where(x => (request.AccountReferenceIds.Count > 0) ? (x.ReferenceId1 != null && request.AccountReferenceIds.Contains((int)x.ReferenceId1)) : true)
                .Where(x => (request.AccountReferenceIds.Count > 0) ? request.AccountReferenceGroupIds.Contains(x.AccountReferencesGroupId) : true)
                .Where(x => x.Voucher.VoucherDate >= request.FromDate && x.Voucher.VoucherDate <= request.ToDate)
                .Sum(x => x.Credit),
                Debit = x.AccountReference.VouchersDetailReferenceId1Navigation
                .Where(x => (request.AccountHeadIds.Count > 0) ? request.AccountHeadIds.Contains(x.AccountHeadId) : true)
                .Where(x => (request.AccountReferenceIds.Count > 0) ? (x.ReferenceId1 != null && request.AccountReferenceIds.Contains((int)x.ReferenceId1)) : true)
                .Where(x => (request.AccountReferenceIds.Count > 0) ? request.AccountReferenceGroupIds.Contains(x.AccountReferencesGroupId) : true)
                .Where(x => x.Voucher.VoucherDate >= request.FromDate && x.Voucher.VoucherDate <= request.ToDate)
                .Sum(x => x.Debit),
            }).Where(x => x.Credit > 0 || x.Debit > 0).ToListAsync();


            DataTable data = new DataTable();

            data.TableName = "گزارش بانک مرکزی";
            data.Columns.Add("نام", typeof(string));
            data.Columns.Add("کد ملی", typeof(string));
            data.Columns.Add("کد اقتصادی", typeof(string));
            data.Columns.Add("آدرس", typeof(string));
            data.Columns.Add("شماره تلفن", typeof(string));
            data.Columns.Add("کد تفضیل", typeof(string));
            data.Columns.Add("بدهکار", typeof(double));
            data.Columns.Add("بستانکار", typeof(double));


            foreach (var person in people)
            {
                data.Rows.Add(
                    person.Name,
                    person.NationalNumber,
                    person.EconomicCode,
                    person.Address,
                    person.Phone,
                    person.AccountReferenceCode,
                    person.Debit,
                    person.Credit
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
