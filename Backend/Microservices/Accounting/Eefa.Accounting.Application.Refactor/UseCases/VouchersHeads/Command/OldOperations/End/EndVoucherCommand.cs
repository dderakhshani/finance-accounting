using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class EndVoucherCommand : IRequest<ServiceResult>
{
    public bool ReplaceEndVoucherFlag { get; set; } = false;
}

//public class EndVoucherCommandHandler : IRequestHandler<EndVoucherCommand, ServiceResult>
//{
//    private readonly IUnitOfWork _unitOfWork;
//    private readonly IMapper _mapper;
//    private readonly IApplicationUser _applicationUser;
//    public EndVoucherCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IApplicationUser applicationUser)
//    {
//        _mapper = mapper;
//        _applicationUser = applicationUser;
//        _unitOfWork = unitOfWork;
//    }

//    public async Task<ServiceResult> Handle(EndVoucherCommand request, CancellationToken cancellationToken)
//    {
//        var year = await _unitOfWork.Years.GetByIdAsync(_applicationUser.YearId);

//        var endCodeVocuerGroup = await _unitOfWork.CodeVoucherGroups.GetAsync(x => x.UniqueName == "end");

//        var previousEndVoucher = await _unitOfWork.VouchersHeads.GetAsync(x =>
//            x.CodeVoucherGroupId == endCodeVocuerGroup.Id,
//            y => y.Include(x => x.VouchersDetails));

//        if (previousEndVoucher != null)
//        {
//            if (request.ReplaceEndVoucherFlag)
//            {
//                if (previousEndVoucher.VouchersDetails != null)
//                    foreach (var vouchersDetail in previousEndVoucher.VouchersDetails)
//                    {
//                        _unitOfWork.VouchersDetails.Delete(vouchersDetail);
//                    }

//                _unitOfWork.VouchersHeads.Delete(previousEndVoucher);
//            }
//            else
//            {
//                throw new Exception("EndVoucherIsAlreadyExists");
//            }
//        }

//        var endVoucherHead = new VouchersHead()
//        {
//            VoucherNo = await VoucherNo.GetNewVoucherNo(_unitOfWork, _applicationUser, year.LastDate, null),
//            VoucherDailyId = 1,
//            VoucherDate = DateTime.Now,
//            CodeVoucherGroupId = endCodeVocuerGroup.Id,
//            CompanyId = _applicationUser.CompanyId,
//            VoucherStateId = 3, // دائم
//            Year = year,
//            VoucherDescription = "اختتامیه",
//        };
//        _unitOfWork.VouchersHeads.Add(endVoucherHead);

//        var permanentAccountHeads = await _unitOfWork.AccountHeads
//            .GetListAsync(x => x.LastLevel && x.TransferId == 2 /* دائم */);

//        foreach (var permanentAccountHead in permanentAccountHeads)
//        {
//            var voucherDetails = _unitOfWork.VouchersDetails
//                .GetListAsync(x => x.Voucher.VoucherStateId == 3 &&
//                            x.AccountHeadId == permanentAccountHead.Id);

//            var debits = (await voucherDetails.Select(x => x.Debit).ToListAsync(cancellationToken)).Aggregate(new double(), (c, n) => c + n);
//            var credits = (await voucherDetails.Select(x => x.Credit).ToListAsync(cancellationToken)).Aggregate(new double(), (c, n) => c + n);


//            var dif = debits - credits;

//            if (dif != 0)
//            {
//                var vd = new VouchersDetail()
//                {
//                    Voucher = endVoucherHead,
//                    AccountHeadId = permanentAccountHead.Id,
//                    VoucherRowDescription = $"سند اختتامیه به شماره {endVoucherHead.VoucherNo}",
//                    VoucherDate = endVoucherHead.VoucherDate
//                };

//                if (dif > 0)
//                {
//                    vd.Credit = dif;
//                    vd.Debit = 0;
//                }
//                if (dif < 0)
//                {
//                    vd.Credit = 0;
//                    vd.Debit = dif;
//                }

//                _unitOfWork.VouchersDetails.Add(vd);
//            }
//        }

//        await _unitOfWork.SaveChangesAsync(cancellationToken);
//        return ServiceResult.Success();
//    }
//}