using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using System.Collections.Generic;
using System.Linq;

public class GetVouchersHeadQuery : IRequest<ServiceResult>
{
    public int Id { get; set; }
    public bool? IsPrint { get; set; }
}
//public class GetVouchersHeadQueryHandler : IRequestHandler<GetVouchersHeadQuery, ServiceResult>
//{
//    private readonly IUnitOfWork _unitOfWork;
//    private readonly IMapper _mapper;
//    //private AccountingUnitOfWork _context;

//    public GetVouchersHeadQueryHandler(IUnitOfWork unitOfWork, IMapper mapper/*, AccountingUnitOfWork context*/)
//    {
//        _mapper = mapper;
//        _unitOfWork= unitOfWork;
//        //_context = context;
//    }

//    public async Task<ServiceResult> Handle(GetVouchersHeadQuery request, CancellationToken cancellationToken)
//    {
//        //var voucher = await _repository
//        //    .Find<VouchersHead>(c
//        //        => c.ObjectId(request.Id))
//        //    .ProjectTo<VouchersHeadModel>(_mapper.ConfigurationProvider)
//        //    .FirstOrDefaultAsync(cancellationToken);
//        //if (/*voucher.VoucherStateId != 3*/ true)
//        //{
//        if (request.IsPrint == true)
//        {
//            var sum = await (from VH in _context.VouchersHeads
//                             join CVG in _context.CodeVoucherGroups on VH.CodeVoucherGroupId equals CVG.Id
//                             join VD in _context.VouchersDetails on VH.Id equals VD.VoucherId
//                             join AH in _context.AccountHeads on VD.AccountHeadId equals AH.Id
//                             from AR in _context.AccountReferences.Where(a => a.Id == VD.ReferenceId1).DefaultIfEmpty()
//                             where VH.Id == request.Id
//                             group VD by true into g
//                             select new
//                             {
//                                 SumDebit = g.Sum(a => a.Debit),
//                                 SumCredit = g.Sum(a => a.Credit)
//                             }).FirstOrDefaultAsync();
//            List<string> debitorders = await (from VH in _context.VouchersHeads
//                                              join CVG in _context.CodeVoucherGroups on VH.CodeVoucherGroupId equals CVG.Id
//                                              join VD in _context.VouchersDetails on VH.Id equals VD.VoucherId
//                                              join AH in _context.AccountHeads.Include(a => a.Parent) on VD.AccountHeadId equals AH.Id
//                                              from AR in _context.AccountReferences.Where(a => a.Id == VD.ReferenceId1).DefaultIfEmpty()
//                                              where VH.Id == request.Id
//                                              where VD.Debit != 0
//                                              orderby VD.Debit descending
//                                              orderby AH.Parent.Code
//                                              orderby AH.Code
//                                              select new VocherResult
//                                              {
//                                                  VoucherNo = VH.VoucherNo,
//                                                  VoucherDate = VH.VoucherDate,
//                                                  VoucherDailyId = VH.VoucherDailyId,
//                                                  CodeVocherGroupName = CVG.Code + " " + CVG.Title,//نوع سند
//                                                  VoucherDescription = VH.VoucherDescription,//شرح سند
//                                                  Level2AccountHead = AH.Parent.Code + " - " + AH.Parent.Title, //کل
//                                                  AccountHeadCode = AH.Code + " - " + AH.Title,//معین
//                                                  AccountReferenceCode = AR.Code + " - " + AR.Title,//تفصیل
//                                                  VoucherRowDescription = VD.VoucherRowDescription,//شرح
//                                                  Debit = VD.Debit,
//                                                  Description = AH.Title + " - " + AR.Title,//شرح تفصیل - شرح معین
//                                              }).GroupBy(a => a.Level2AccountHead).Select(a => new
//                                              {
//                                                  a.Key,
//                                                  debit = a.Sum(f => f.Debit)
//                                              }).OrderByDescending(a => a.debit).Select(a => a.Key).ToListAsync();
//            var debitdata = await (from VH in _context.VouchersHeads
//                                   join CVG in _context.CodeVoucherGroups on VH.CodeVoucherGroupId equals CVG.Id
//                                   join VD in _context.VouchersDetails on VH.Id equals VD.VoucherId
//                                   join AH in _context.AccountHeads.Include(a => a.Parent) on VD.AccountHeadId equals AH.Id
//                                   from AR in _context.AccountReferences.Where(a => a.Id == VD.ReferenceId1).DefaultIfEmpty()
//                                   where VH.Id == request.Id
//                                   where VD.Debit != 0
//                                   orderby VD.Debit descending
//                                   orderby AH.Parent.Code
//                                   orderby AH.Code
//                                   select new VocherResult
//                                   {
//                                       VoucherNo = VH.VoucherNo,
//                                       VoucherDate = VH.VoucherDate,
//                                       VoucherDailyId = VH.VoucherDailyId,
//                                       CodeVocherGroupName = CVG.Code + " " + CVG.Title,//نوع سند
//                                       VoucherDescription = VH.VoucherDescription,//شرح سند
//                                       Level2AccountHead = AH.Parent.Code + " - " + AH.Parent.Title, //کل
//                                       AccountHeadCode = AH.Code + " - " + AH.Title,//معین
//                                       AccountReferenceCode = AR.Code + " - " + AR.Title,//تفصیل
//                                       VoucherRowDescription = VD.VoucherRowDescription,//شرح
//                                       Debit = VD.Debit,
//                                       Description = AH.Title + " - " + AR.Title,//شرح تفصیل - شرح معین
//                                   }).OrderByMultipleColumns(propertyNames: "Level2AccountHead,AccountHeadCode,Debit").ToListAsync();


//            List<string> creditorders = await (from VH in _context.VouchersHeads
//                                               join CVG in _context.CodeVoucherGroups on VH.CodeVoucherGroupId equals CVG.Id
//                                               join VD in _context.VouchersDetails on VH.Id equals VD.VoucherId
//                                               join AH in _context.AccountHeads.Include(a => a.Parent) on VD.AccountHeadId equals AH.Id
//                                               from AR in _context.AccountReferences.Where(a => a.Id == VD.ReferenceId1).DefaultIfEmpty()
//                                               where VH.Id == request.Id
//                                               where VD.Credit != 0
//                                               orderby VD.Debit descending
//                                               orderby AH.Parent.Code
//                                               orderby AH.Code
//                                               select new VocherResult
//                                               {
//                                                   VoucherNo = VH.VoucherNo,
//                                                   VoucherDate = VH.VoucherDate,
//                                                   VoucherDailyId = VH.VoucherDailyId,
//                                                   CodeVocherGroupName = CVG.Code + " " + CVG.Title,//نوع سند
//                                                   VoucherDescription = VH.VoucherDescription,//شرح سند
//                                                   Level2AccountHead = AH.Parent.Code + " - " + AH.Parent.Title, //کل
//                                                   AccountHeadCode = AH.Code + " - " + AH.Title,//معین
//                                                   AccountReferenceCode = AR.Code + " - " + AR.Title,//تفصیل
//                                                   VoucherRowDescription = VD.VoucherRowDescription,//شرح
//                                                   Credit = VD.Credit,
//                                                   Description = AH.Title + " - " + AR.Title,//شرح تفصیل - شرح معین
//                                               }).GroupBy(a => a.Level2AccountHead).Select(a => new
//                                               {
//                                                   a.Key,
//                                                   credit = a.Sum(f => f.Credit)
//                                               }).OrderByDescending(a => a.credit).Select(a => a.Key).ToListAsync();

//            var creditdata = await (from VH in _context.VouchersHeads
//                                    join CVG in _context.CodeVoucherGroups on VH.CodeVoucherGroupId equals CVG.Id
//                                    join VD in _context.VouchersDetails on VH.Id equals VD.VoucherId
//                                    join AH in _context.AccountHeads.Include(a => a.Parent) on VD.AccountHeadId equals AH.Id
//                                    from AR in _context.AccountReferences.Where(a => a.Id == VD.ReferenceId1).DefaultIfEmpty()
//                                    where VH.Id == request.Id
//                                    where VD.Credit != 0
//                                    orderby VD.Debit descending
//                                    orderby AH.Parent.Code
//                                    orderby AH.Code
//                                    select new VocherResult
//                                    {
//                                        VoucherNo = VH.VoucherNo,
//                                        VoucherDate = VH.VoucherDate,
//                                        VoucherDailyId = VH.VoucherDailyId,
//                                        CodeVocherGroupName = CVG.Code + " " + CVG.Title,//نوع سند
//                                        VoucherDescription = VH.VoucherDescription,//شرح سند
//                                        Level2AccountHead = AH.Parent.Code + " - " + AH.Parent.Title, //کل
//                                        AccountHeadCode = AH.Code + " - " + AH.Title,//معین
//                                        AccountReferenceCode = AR.Code + " - " + AR.Title,//تفصیل
//                                        VoucherRowDescription = VD.VoucherRowDescription,//شرح
//                                        Credit = VD.Credit,
//                                        Description = AH.Title + " - " + AR.Title,//شرح تفصیل - شرح معین
//                                    }).OrderByMultipleColumns(propertyNames: "Level2AccountHead,AccountHeadCode,Credit").ToListAsync();
//            ResultModel result = new(debitorders, debitdata, creditorders, creditdata, sum.SumDebit, sum.SumCredit);
//            return ServiceResult.Success(result);
//        }
//        return ServiceResult.Success(await _unitOfWork.VouchersHeads.GetProjectedByIdAsync<VouchersHeadWithDetailModel>(request.Id));
//        //}
//        //return ServiceResult.Set(voucher);
//    }
//}