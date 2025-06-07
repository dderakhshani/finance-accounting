using Library.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Data;
using AutoMapper;
using Eefa.Accounting.Application.UseCases.AccountHead.Command;
using Eefa.Accounting.Application.UseCases.Report.Model;
using Eefa.Accounting.Data.Databases.SqlServer.Context;
using Eefa.Persistence.Data.SqlServer.QueryProvider;
using Library.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Accounting.Application.UseCases.Report
{
    public class ProfitAndLossReportQuery : IRequest<ServiceResult>
    {
        public int CompanyId { get; set; }
        public int[] YearIds { get; set; }
        public DateTime? VoucherDateFrom { get; set; }
        public DateTime? VoucherDateTo { get; set; }
    }
    public class ProfitAndLossReportQueryHandler : IRequestHandler<ProfitAndLossReportQuery, ServiceResult>
    {
        private readonly IAccountingUnitOfWorkProcedures _accountingUnitOfWorkProcedures;
        private readonly ICurrentUserAccessor _currentUserAccessor;


        private readonly IMapper _mapper;
        private readonly IRepository _repository;
        private readonly IAccountingUnitOfWork _context;
        public ProfitAndLossReportQueryHandler(IRepository repository, IAccountingUnitOfWorkProcedures accountingUnitOfWorkProcedures, ICurrentUserAccessor currentUserAccessor, IMapper mapper, IAccountingUnitOfWork context)
        {
            _repository = repository;
            _accountingUnitOfWorkProcedures = accountingUnitOfWorkProcedures;
            _currentUserAccessor = currentUserAccessor;
            _mapper = mapper;
            _context = context;
        }
        public async Task<ServiceResult> Handle(ProfitAndLossReportQuery request, CancellationToken cancellationToken)
        {
            request.VoucherDateTo = request.VoucherDateTo?.AddDays(1).AddMilliseconds(-1);
            var voucherDetailQuery = _context.VouchersDetails.ApplyPermission(_context, _currentUserAccessor, false, true);

            if (request.VoucherDateFrom != null && request.VoucherDateTo != null) voucherDetailQuery = voucherDetailQuery.Where(x => x.Voucher.VoucherDate >= request.VoucherDateFrom && x.Voucher.VoucherDate <= request.VoucherDateTo);
            if (request.YearIds?.Length > 0) voucherDetailQuery = voucherDetailQuery.Where(x => request.YearIds.Contains(x.Voucher.YearId));
            if (request.CompanyId != 0) voucherDetailQuery = voucherDetailQuery.Where(x => x.Voucher.CompanyId == request.CompanyId);
            var query = voucherDetailQuery.Join(_context.AccountHeads, vd => vd.AccountHeadId, ah => ah.Id, (vd, ah) => new { vd, ah })
                .Where(a => a.ah.Code == "60101" ||
            a.ah.Code == "60102" || a.ah.Code == "60103" || a.ah.Code == "60104" || a.ah.Code == "60105" || a.ah.Code == "60106")
                .GroupBy(a => new { a.ah.Title, a.ah.Code })
                .Select(a => new ProfitAndLossReportResult
                {
                    Code = a.Key.Code,
                    Name = a.Key.Title,
                    Price = a.Sum(f => f.vd.Credit) - a.Sum(f => f.vd.Debit),
                });
            var lstlevel3 = await query.ToListAsync(cancellationToken);

            var firstrow = lstlevel3.Where(a => a.Code == "60101").FirstOrDefault();
            var secondrow = lstlevel3.Where(a => a.Code == "60102").FirstOrDefault();

            List<ProfitAndLossReportResult> result = new();
            result.Add(firstrow);
            result.Add(secondrow);
            var SaleSumPrice = lstlevel3.Where(a => a.Code == "60101" || a.Code == "60102").Sum(a => a.Price);
            ProfitAndLossReportResult salesum = new("", "جمع فروش ناخالص", SaleSumPrice);
            result.Add(salesum);

            var returnsaleprice = lstlevel3.Where(a => a.Code == "60103" || a.Code == "60104" ||
                                          a.Code == "60105" || a.Code == "60106").Sum(a => a.Price);


            ProfitAndLossReportResult returnsale = new("\u200E60103 \u200Fتا \u200E60106", "برگشت از فروش وتخفیفات", returnsaleprice);
            result.Add(returnsale);



            var TotalOperatingIncomePrice = SaleSumPrice + returnsaleprice;
            ProfitAndLossReportResult TotalOperatingIncome = new("", "جمع درآمد عملیاتی", TotalOperatingIncomePrice);
            result.Add(TotalOperatingIncome);


            var level2 = await voucherDetailQuery.Join(_context.AccountHeads, vd => vd.Level2, ah => ah.Id, (vd, ah) => new { vd, ah })
                .Where(a => a.ah.Code == "702" || a.ah.Code == "701" || a.ah.Code == "801" || a.ah.Code == "802" || a.ah.Code == "803" ||
                            a.ah.Code == "805" || a.ah.Code == "602" || a.ah.Code == "806" || a.ah.Code == "808")
                .GroupBy(a => new { a.ah.Title, a.ah.Code }).
                Select(a => new ProfitAndLossReportResult
                {
                    Code = a.Key.Code,
                    Name = a.Key.Title,
                    Price = a.Sum(f => f.vd.Credit) - a.Sum(f => f.vd.Debit),
                }).ToListAsync(cancellationToken);

           
           
            var CostofoperatingincomePrice = level2.Where(a => a.Code == "701" || a.Code == "702").Sum(a => a.Price);
            ProfitAndLossReportResult Costofoperatingincome = new("\u200E701 \u200Fو \u200E702", "بهای تمام شده درآمد های عملیاتی", CostofoperatingincomePrice);
                result.Add(Costofoperatingincome);


            //if (level2firstrow == null || level2secondrow== null)
            //{
            //    ProfitAndLossReportResult Costofoperatingincome = new("702", "بهای تمام شده درآمد های عملیاتی", 0);
            //    result.Add(Costofoperatingincome);
            //}
            //else
            //{
            //    result.Add(level2firstrow);
            //}
            //var GrossProfitPrice = (level2firstrow == null ? 0 : level2firstrow.Price) + TotalOperatingIncomePrice;
           
            var GrossProfitPrice = TotalOperatingIncomePrice + CostofoperatingincomePrice;
            ProfitAndLossReportResult GrossProfit = new("", "سود ناخالص", GrossProfitPrice);
            result.Add(GrossProfit);


            var level2thriedrow = level2.Where(a => a.Code == "801").FirstOrDefault();
            var level2fourthrow = level2.Where(a => a.Code == "802").FirstOrDefault();
            var level2fivethrow = level2.Where(a => a.Code == "803").FirstOrDefault();
            var level2sixthrow = level2.Where(a => a.Code == "805").FirstOrDefault();
            var level2seventhrow = level2.Where(a => a.Code == "602").FirstOrDefault();
            result.Add(level2thriedrow);
            result.Add(level2fourthrow);
            result.Add(level2fivethrow);
            result.Add(level2sixthrow);
            result.Add(level2seventhrow);

            var TotalOperatingCostsPrice = level2.Where(a => a.Code == "801" || a.Code == "802" || a.Code == "803" || a.Code == "805" || a.Code == "602").Sum(a => a.Price);
            ProfitAndLossReportResult TotalOperatingCosts = new("", "جمع هزینه های عملیاتی", TotalOperatingCostsPrice);
            result.Add(TotalOperatingCosts);
            ProfitAndLossReportResult OperatingProfitAndLoss = new("", "سود و زیان عملیاتی", TotalOperatingCostsPrice + GrossProfitPrice);
            result.Add(OperatingProfitAndLoss);


            var level2eightthrow = level2.Where(a => a.Code == "806").FirstOrDefault();
            var level2ninethrow = level2.Where(a => a.Code == "808").FirstOrDefault();
            result.Add(level2eightthrow);
            result.Add(level2ninethrow);



            ProfitAndLossReportResult GrossProfitBeforeTax = new("", "سود ناخالص قبل از مالیات", OperatingProfitAndLoss.Price + level2.Where(a => a.Code == "806" || a.Code == "808").Sum(a => a.Price));
            result.Add(GrossProfitBeforeTax);
            ProfitAndLossReportResult IncomeTaxEexpense = new("", "هزینه مالیات بر درآمد", 0);
            result.Add(IncomeTaxEexpense);

            
            ProfitAndLossReportResult NetProfit = new("", "سود خالص", IncomeTaxEexpense.Price + GrossProfitBeforeTax.Price);
            ProfitAndLossReportResult SalesMargin = new("", "حاشیه فروش (درصد)", (GrossProfit.Price / TotalOperatingIncome.Price)*100);
            ProfitAndLossReportResult OperatingPprofitMargin = new("", "حاشیه سود عملیاتی (درصد)", (OperatingProfitAndLoss.Price / TotalOperatingIncome.Price)*100);
            ProfitAndLossReportResult ProfitMarigin = new("", "حاشیه سود (درصد)", (NetProfit.Price / TotalOperatingIncome.Price)*100);
            result.Add(NetProfit);
            result.Add(SalesMargin);
            result.Add(OperatingPprofitMargin);
            result.Add(ProfitMarigin);
            result = result.Where((a => a != null)).ToList();

            
            return ServiceResult.Success(result);
        }
    }
}
