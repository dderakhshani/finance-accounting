using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
//TODO the handler
public class AdjustmentCommand : IRequest<ServiceResult>
{
    public bool ReplaceAdjustmentVoucherFlag { get; set; } = false;
}

//public class AdjustmentCommandHandler : IRequestHandler<AdjustmentCommand, ServiceResult>
//{
//    private readonly IUnitOfWork _unitOfWork;
//    private readonly IMapper _mapper;
//    private readonly IApplicationUser _applicationUser;

//    public AdjustmentCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IApplicationUser applicationUser)
//    {
//        _mapper = mapper;
//        _applicationUser = applicationUser;
//        _unitOfWork = unitOfWork;
//    }

//    public async Task<ServiceResult> Handle(AdjustmentCommand request, CancellationToken cancellationToken)
//    {
//        var accountHeads = await _unitOfWork.AccountHeads
//            .GetListAsync(x => (x.BalanceId == 1 || x.BalanceId == 2) && x.LastLevel);

//        var balancingCodeVoucherGroup = await _unitOfWork.CodeVoucherGroups
//            .GetAsync(x => x.UniqueName == "balancing");

//        var year = await _unitOfWork.Years.GetByIdAsync(_applicationUser.YearId);


//        var previousbalancingCodeVoucher = await _unitOfWork.VouchersHeads
//            .GetAsync(x => x.CodeVoucherGroupId == balancingCodeVoucherGroup.Id);

//        if (previousbalancingCodeVoucher != null)
//        {
//            if (request.ReplaceAdjustmentVoucherFlag)
//            {
//                if (previousbalancingCodeVoucher.VouchersDetails != null)
//                    foreach (var vouchersDetail in previousbalancingCodeVoucher.VouchersDetails)
//                    {
//                        _unitOfWork.VouchersDetails.Delete(vouchersDetail);
//                    }

//                _unitOfWork.VouchersHeads.Delete(previousbalancingCodeVoucher);
//            }
//            else
//            {
//                throw new BalancingVoucherIsAlreadyExists("BalancingVoucherIsAlreadyExists");
//            }
//        }
//        Specification<VouchersHead> specilastVoucherId = new Specification<VouchersHead>();
//        specilastVoucherId.ApplicationConditions.Add(x => x.VoucherDate.Date == DateTime.Now);
//        specilastVoucherId.OrderBy = x => x.OrderBy(y => y.Id);

//        var lastVoucherId = (await _unitOfWork.VouchersHeads.GetAsync(specilastVoucherId))?.VoucherDailyId ?? 0;
//        //.LastOrDefaultAsync(cancellationToken))

//        var voucherNo =
//            await VoucherNo.GetNewVoucherNo(_unitOfWork, _applicationUser, DateTime.Now, null);

//        var entity = new VouchersHead()
//        {
//            VoucherNo = voucherNo,
//            VoucherDailyId = lastVoucherId + 1,
//            VoucherDate = DateTime.Now,
//            CodeVoucherGroupId = balancingCodeVoucherGroup.Id,
//            CompanyId = _applicationUser.CompanyId,
//            VoucherStateId = 1, // موقت
//            YearId = _applicationUser.YearId,
//            VoucherDescription = "بابت تعدیل حساب"
//        }
//        _unitOfWork.VouchersHeads.Add(entity);

//        foreach (var accountHead in accountHeads)
//        {
//            var accountDetails = _unitOfWork.VouchersDetails
//                .GetAsync(x => x.AccountHeadId == accountHead.Id
//                            && x.VoucherDate >= year.FirstDate
//                            && x.VoucherDate <= year.LastDate
//                            && x.Voucher.CompanyId == _applicationUser.CompanyId);

//            var sumCredit = accountDetails.Sum(vouchersDetail => vouchersDetail.Credit);

//            var sumDebit = accountDetails.Sum(vouchersDetail => vouchersDetail.Debit);

//            var def = sumDebit - sumCredit;
//            if (def != 0)
//            {
//                if (def > 0) // بدهکار
//                {
//                    _unitOfWork.VouchersDetails.Add(new VouchersDetail()
//                    {
//                        Voucher = entity,
//                        AccountHeadId = accountHead.Id,
//                        Debit = def,
//                        Credit = 0,
//                        VoucherRowDescription = "بابت تعدیل حساب"
//                    });
//                }

//                if (def < 0)
//                {
//                    if (def > 0) // بدهکار
//                    {
//                        _unitOfWork.VouchersDetails.Add(new VouchersDetail()
//                        {
//                            Voucher = entity,
//                            AccountHeadId = accountHead.Id,
//                            Debit = 0,
//                            Credit = def,
//                            VoucherRowDescription = "بابت تعدیل حساب"
//                        });
//                    }
//                }
//            }
//        }
//        await _unitOfWork.SaveChangesAsync(cancellationToken);
//        return ServiceResult.Success();
//    }
//}