using System;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;

public class CreateCloseVoucherCommand : IRequest<ServiceResult>
{
    public int CloserAccountHeadId { get; set; }
    public bool ReplaceCloseVoucherFlag { get; set; } = false;
}

//public class CreateCloseVoucherCommandHandler : IRequestHandler<CreateCloseVoucherCommand, ServiceResult>
//{
//    private readonly IUnitOfWork _unitOfWork;
//    private readonly IMapper _mapper;
//    private readonly IApplicationUser _applicationUser;
//    public CreateCloseVoucherCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IApplicationUser applicationUser)
//    {
//        _mapper = mapper;
//        _applicationUser = applicationUser;
//        _unitOfWork = unitOfWork;
//    }

//    public async Task<ServiceResult> Handle(CreateCloseVoucherCommand request, CancellationToken cancellationToken)
//    {
//        if (request.CloserAccountHeadId == 0)
//        {
//            var settings =
//                await new SystemSettings(_unitOfWork).Get(SubSystemType.AccountingSettings);

//            foreach (var baseValue in settings)
//            {
//                if (baseValue.UniqueName == "CloseTemproryAccounts")
//                {
//                    request.CloserAccountHeadId = int.Parse(baseValue.Value);
//                    break;
//                }
//            }
//        }

//        var closeCodeVocuerGroup = await _unitOfWork.CodeVoucherGroups
//            .GetAsync(x => x.UniqueName == "temproraryEnd");

//        var previousCloseCodeVoucher = await _unitOfWork.VouchersHeads
//            .GetAsync(x => x.CodeVoucherGroupId == closeCodeVocuerGroup.Id);

//        if (previousCloseCodeVoucher != null)
//        {
//            if (request.ReplaceCloseVoucherFlag)
//            {
//                if (previousCloseCodeVoucher.VouchersDetails != null)
//                    foreach (var vouchersDetail in previousCloseCodeVoucher.VouchersDetails)
//                    {
//                        _unitOfWork.VouchersDetails.Delete(vouchersDetail);
//                    }

//                _unitOfWork.VouchersHeads.Delete(previousCloseCodeVoucher);
//            }
//            else
//            {
//                throw new Exception("CloseVoucherIsAlreadyExists");
//            }
//        }

//        var temproraryAccoundHeads = await _unitOfWork.AccountHeads
//            .GetListAsync(x => x.TransferId == 1);

//        var sumCredit = temproraryAccoundHeads.Sum(temproraryAccoundHead =>
//               _unitOfWork.VouchersDetails
//            .GetListAsync(x => x.AccountHeadId == temproraryAccoundHead.Id,
//                          y => y.Select(z => z.Credit))
//            .Aggregate(new double(), (c, n) => c + n));

//        var sumDebit = temproraryAccoundHeads.Sum(temproraryAccoundHead =>
//            _unitOfWork.VouchersDetails
//                .GetListAsync(x => x.AccountHeadId == temproraryAccoundHead.Id,
//                              y => y.Select(x => x.Debit))
//            .Aggregate(new double(), (c, n) => c + n));

//        Specification<VouchersHead> spciLastVoucherId = new Specification<VouchersHead>();
//        spciLastVoucherId.ApplicationConditions.Add(x => x.VoucherDate.Date == DateTime.Now);
//        spciLastVoucherId.OrderBy = y => y.OrderBy(x => x.Id);
//        var lastVoucherId = (await _unitOfWork.VouchersHeads.GetAsync(spciLastVoucherId)/*.LastOrDefaultAsync(cancellationToken)*/)?.VoucherDailyId ?? 0;

//        var voucherNo = await VoucherNo.GetNewVoucherNo(_unitOfWork, _applicationUser, DateTime.Now, null);
//        var closeVoucherHead = new VouchersHead()
//        {
//            VoucherNo = voucherNo,
//            VoucherDailyId = lastVoucherId + 1,
//            VoucherDate = DateTime.Now,
//            CodeVoucherGroupId = closeCodeVocuerGroup.Id,
//            CompanyId = _applicationUser.CompanyId,
//            VoucherStateId = 3, // دائم
//            YearId = _applicationUser.YearId,
//            VoucherDescription = "بستن حساب های موقت",
//            TotalCredit = sumDebit,
//            TotalDebit = sumCredit
//        };
//        _unitOfWork.VouchersHeads.Add(closeVoucherHead);

//        _unitOfWork.VouchersHeads.Add(new VouchersDetail()
//        {
//            Voucher = closeVoucherHead,
//            AccountHeadId = request.CloserAccountHeadId,
//            Debit = sumCredit,
//            Credit = sumDebit,
//            VoucherRowDescription = "بستن حساب های موقت"
//        });

//        await _unitOfWork.SaveChangesAsync(cancellationToken);
//        return ServiceResult.Success(closeVoucherHead);
//    }
//}