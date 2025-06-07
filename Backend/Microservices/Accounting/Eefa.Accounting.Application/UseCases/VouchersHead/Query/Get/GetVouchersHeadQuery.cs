using AutoMapper;
using AutoMapper.QueryableExtensions;
using Eefa.Accounting.Application.UseCases.VouchersDetail.Model;
using Eefa.Accounting.Application.UseCases.VouchersHead.Model;
using Eefa.Accounting.Data.Databases.Sp;
using Eefa.Accounting.Data.Databases.SqlServer.Context;
using Eefa.Persistence.Data.SqlServer.QueryProvider;
using Library.Exceptions;
using Library.Interfaces;
using Library.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
namespace Eefa.Accounting.Application.UseCases.VouchersHead.Query.Get
{
    public class GetVouchersHeadQuery : IRequest<ServiceResult>, IQuery
    {
        public int Id { get; set; }
        public int? PrintType { get; set; }
        public bool? Isprint { get; set; }
        public string SelectedVoucherDetailIds { get; set; }
    }
    public class GetVouchersHeadQueryHandler : IRequestHandler<GetVouchersHeadQuery, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;
        private AccountingUnitOfWork _context;
        private readonly ICurrentUserAccessor _currentUserAccessor;

        public GetVouchersHeadQueryHandler(IRepository repository, IMapper mapper, AccountingUnitOfWork context, ICurrentUserAccessor currentUserAccessor)
        {
            _mapper = mapper;
            _repository = repository;
            _context = context;
            _currentUserAccessor = currentUserAccessor;
        }

        public async Task<ServiceResult> Handle(GetVouchersHeadQuery request, CancellationToken cancellationToken)
        {
            //var voucher = await _repository
            //    .Find<Data.Entities.VouchersHead>(c
            //        => c.ObjectId(request.Id))
            //    .ProjectTo<VouchersHeadModel>(_mapper.ConfigurationProvider)
            //    .FirstOrDefaultAsync(cancellationToken);
            //if (/*voucher.VoucherStateId != 3*/ true)
            //{
            if (request.Isprint == true)
            {
                var VoucherDetailIds = request.SelectedVoucherDetailIds?.Split(',');
                IQueryable<GetVouchersHeadModel> query;
                if (VoucherDetailIds != null && VoucherDetailIds.Length > 0)
                {
                    query = (from VH in _context.VouchersHeads
                             join CVG in _context.CodeVoucherGroups on VH.CodeVoucherGroupId equals CVG.Id
                             join VD in _context.VouchersDetails.ApplyPermission(_context, _currentUserAccessor, false, true) on VH.Id equals VD.VoucherId
                             join AH in _context.AccountHeads.Include(a => a.Parent) on VD.AccountHeadId equals AH.Id
                             from AR in _context.AccountReferences.Where(a => a.Id == VD.ReferenceId1).DefaultIfEmpty()
                             where VoucherDetailIds.Any(a => a == VD.Id.ToString())
                             where VH.Id == request.Id
                             select new GetVouchersHeadModel()
                             {
                                 VH = VH,
                                 CVG = CVG,
                                 VD = VD,
                                 AH = AH,
                                 AR = AR
                             });
                }

                else
                {
                    query = (from VH in _context.VouchersHeads
                             join CVG in _context.CodeVoucherGroups on VH.CodeVoucherGroupId equals CVG.Id
                             join VD in _context.VouchersDetails.ApplyPermission(_context, _currentUserAccessor, false, true) on VH.Id equals VD.VoucherId
                             join AH in _context.AccountHeads.Include(a => a.Parent) on VD.AccountHeadId equals AH.Id
                             from AR in _context.AccountReferences.Where(a => a.Id == VD.ReferenceId1).DefaultIfEmpty()
                             where VH.Id == request.Id
                             select new GetVouchersHeadModel()
                             {
                                 VH = VH,
                                 CVG = CVG,
                                 VD = VD,
                                 AH = AH,
                                 AR = AR
                             });
                }

                if (request.PrintType == 1)
                {
                    return await Print1(request, query, cancellationToken);
                }
                else if (request.PrintType == 2)
                {
                    return await Print2(request, query, cancellationToken);
                }
                else if (request.PrintType == 3)
                {

                }
            }


            var voucherHead = await _context.VouchersHeads.Where(a => a.Id == request.Id)
                .ProjectTo<VouchersHeadWithDetailModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);
            if (voucherHead != null)
            {
                voucherHead.VouchersDetails = voucherHead.VouchersDetails.ApplyPermission<VouchersDetailModel, Data.Entities.VouchersDetail>(_context, _currentUserAccessor, false, true).OrderBy(x => x.RowIndex).ToList();
            }
            return ServiceResult.Success(voucherHead);
            //}
            //return ServiceResult.Set(voucher);
        }
        private async Task<ServiceResult> Print1(GetVouchersHeadQuery request, IQueryable<GetVouchersHeadModel> query, CancellationToken cancellationToken)
        {


            var sum = await (query.GroupBy(a => true).Select(g => new
            {
                SumDebit = g.Sum(a => a.VD.Debit),
                SumCredit = g.Sum(a => a.VD.Credit)
            })).FirstOrDefaultAsync();



            var debitorders = await query.Where(a => a.VD.Debit != 0)
                .Select(a => new VocherResult
                {
                    VoucherNo = a.VH.VoucherNo,
                    VoucherDate = a.VH.VoucherDate,
                    VoucherDailyId = a.VH.VoucherDailyId,
                    CodeVocherGroupName = a.CVG.Code + " " + a.CVG.Title,//نوع سند
                    VoucherDescription = a.VH.VoucherDescription,//شرح سند
                    Level2AccountHead = a.AH.Parent.Code + " - " + a.AH.Parent.Title, //کل
                    AccountHeadCode = a.AH.Code + " - " + a.AH.Title,//معین
                    AccountReferenceCode = a.AR.Code + " - " + a.AR.Title,//تفصیل
                    VoucherRowDescription = a.VD.VoucherRowDescription,//شرح
                    Debit = a.VD.Debit,
                    Description = a.AH.Title + " - " + a.AR.Title,//شرح تفصیل - شرح معین
                }).GroupBy(a => a.Level2AccountHead).Select(a => new
                {
                    a.Key,
                    debit = a.Sum(f => f.Debit)
                }).OrderByDescending(a => a.debit).Select(a => a.Key).ToListAsync();

            var debitdata = await query.Where(a => a.VD.Debit != 0)
                 .Select(a => new VocherResult
                 {
                     VoucherNo = a.VH.VoucherNo,
                     VoucherDate = a.VH.VoucherDate,
                     VoucherDailyId = a.VH.VoucherDailyId,
                     CodeVocherGroupName = a.CVG.Code + " " + a.CVG.Title,//نوع سند
                     VoucherDescription = a.VH.VoucherDescription,//شرح سند
                     Level2AccountHead = a.AH.Parent.Code + " - " + a.AH.Parent.Title, //کل
                     AccountHeadCode = a.AH.Code + " - " + a.AH.Title,//معین
                     AccountReferenceCode = a.AR.Code + " - " + a.AR.Title,//تفصیل
                     VoucherRowDescription = a.VD.VoucherRowDescription,//شرح
                     Debit = a.VD.Debit,
                     Description = a.AH.Title + " - " + a.AR.Title,//شرح تفصیل - شرح معین
                 }).OrderByMultipleColumns(propertyNames: "Level2AccountHead,AccountHeadCode,Debit").ToListAsync();

            List<string> creditorders = await query.Where(a => a.VD.Credit != 0)
                .Select(a => new VocherResult
                {
                    VoucherNo = a.VH.VoucherNo,
                    VoucherDate = a.VH.VoucherDate,
                    VoucherDailyId = a.VH.VoucherDailyId,
                    CodeVocherGroupName = a.CVG.Code + " " + a.CVG.Title,//نوع سند
                    VoucherDescription = a.VH.VoucherDescription,//شرح سند
                    Level2AccountHead = a.AH.Parent.Code + " - " + a.AH.Parent.Title, //کل
                    AccountHeadCode = a.AH.Code + " - " + a.AH.Title,//معین
                    AccountReferenceCode = a.AR.Code + " - " + a.AR.Title,//تفصیل
                    VoucherRowDescription = a.VD.VoucherRowDescription,//شرح
                    Credit = a.VD.Credit,
                    Description = a.AH.Title + " - " + a.AR.Title,//شرح تفصیل - شرح معین
                }).GroupBy(a => a.Level2AccountHead).Select(a => new
                {
                    a.Key,
                    credit = a.Sum(f => f.Credit)
                }).OrderByDescending(a => a.credit).Select(a => a.Key).ToListAsync();

            var creditdata = await query.Where(a => a.VD.Credit != 0)
                .Select(a => new VocherResult
                {
                    VoucherNo = a.VH.VoucherNo,
                    VoucherDate = a.VH.VoucherDate,
                    VoucherDailyId = a.VH.VoucherDailyId,
                    CodeVocherGroupName = a.CVG.Code + " " + a.CVG.Title,//نوع سند
                    VoucherDescription = a.VH.VoucherDescription,//شرح سند
                    Level2AccountHead = a.AH.Parent.Code + " - " + a.AH.Parent.Title, //کل
                    AccountHeadCode = a.AH.Code + " - " + a.AH.Title,//معین
                    AccountReferenceCode = a.AR.Code + " - " + a.AR.Title,//تفصیل
                    VoucherRowDescription = a.VD.VoucherRowDescription,//شرح
                    Credit = a.VD.Credit,
                    Description = a.AH.Title + " - " + a.AR.Title,//شرح تفصیل - شرح معین
                }).OrderByMultipleColumns(propertyNames: "Level2AccountHead,AccountHeadCode,Credit").ToListAsync();

            ResultModel result = new(debitorders, debitdata, creditorders, creditdata, sum.SumDebit, sum.SumCredit);
            return ServiceResult.Success(result);
        }
        private async Task<ServiceResult> Print2(GetVouchersHeadQuery request, IQueryable<GetVouchersHeadModel> query, CancellationToken cancellationToken)
        {
            var VoucherDetailIds = request.SelectedVoucherDetailIds?.Split(',');

            var sum = await (query.GroupBy(a => true).Select(g => new
            {
                SumDebit = g.Sum(a => a.VD.Debit),
                SumCredit = g.Sum(a => a.VD.Credit)
            })).FirstOrDefaultAsync();

            List<string> orders = query.Select(a =>
                                          new VocherResult
                                          {
                                              RowIndex = a.VD.RowIndex,
                                              VoucherNo = a.VH.VoucherNo,
                                              VoucherDate = a.VH.VoucherDate,
                                              VoucherDailyId = a.VH.VoucherDailyId,
                                              CodeVocherGroupName = a.CVG.Code + " " + a.CVG.Title,//نوع سند
                                              VoucherDescription = a.VH.VoucherDescription,//شرح سند
                                              Level2AccountHead = a.AH.Parent.Code + " - " + a.AH.Parent.Title, //کل
                                              AccountHeadCode = a.AH.Code + " - " + a.AH.Title,//معین
                                              AccountReferenceCode = a.AR.Code + " - " + a.AR.Title,//تفصیل
                                              VoucherRowDescription = a.VD.VoucherRowDescription,//شرح
                                              Debit = a.VD.Debit,
                                              Credit = a.VD.Credit,
                                              Description = a.AH.Title + " - " + a.AR.Title,//شرح تفصیل - شرح معین
                                          }).GroupByMany(a => new { a.Level2AccountHead, a.RowIndex })
                                          .OrderBy(a => a.Key.RowIndex).Select(a => (string)a.Key.Level2AccountHead).ToList();
            var data = await query.Select(a => new VocherResult
            {
                RowIndex = a.VD.RowIndex,
                VoucherNo = a.VH.VoucherNo,
                VoucherDate = a.VH.VoucherDate,
                VoucherDailyId = a.VH.VoucherDailyId,
                CodeVocherGroupName = a.CVG.Code + " " + a.CVG.Title,//نوع سند
                VoucherDescription = a.VH.VoucherDescription,//شرح سند
                Level2AccountHead = a.AH.Parent.Code + " - " + a.AH.Parent.Title, //کل
                AccountHeadCode = a.AH.Code + " - " + a.AH.Title,//معین
                AccountReferenceCode = a.AR.Code + " - " + a.AR.Title,//تفصیل
                VoucherRowDescription = a.VD.VoucherRowDescription,//شرح
                Debit = a.VD.Debit,
                Credit = a.VD.Credit,
                Description = a.AH.Title + " - " + a.AR.Title,//شرح تفصیل - شرح معین
            }).OrderBy(a => a.RowIndex).ToListAsync();
            Print2Model result = new(data, sum.SumDebit, sum.SumCredit, orders);
            return ServiceResult.Success(result);
        }
    }
    public class GetVouchersHeadModel
    {

        public Data.Entities.VouchersHead VH { get; set; }
        public Data.Entities.CodeVoucherGroup CVG { get; set; }
        public Data.Entities.VouchersDetail VD { get; set; }
        public Data.Entities.AccountHead AH { get; set; }
        public Data.Entities.AccountReference AR { get; set; }
    }


}
