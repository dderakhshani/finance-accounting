using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

// TODO "return ServiceResult.Success()" Changed To "return ServiceResult.Success(true)"
public class MoveVoucherDetailsCommand : IRequest<ServiceResult<bool>>
{
    public int VoucherHeadId { get; set; }
    public List<int> VoucherDetailIds { get; set; }
}

public class MoveVoucherDetailsCommandHandler : IRequestHandler<MoveVoucherDetailsCommand, ServiceResult<bool>>
{
    private readonly IUnitOfWork _unitOfWork;

    public MoveVoucherDetailsCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork= unitOfWork;
    }

    public async Task<ServiceResult<bool>> Handle(MoveVoucherDetailsCommand request, CancellationToken cancellationToken)
    {
        var voucherDetails = await _unitOfWork.VouchersDetails.GetListAsync(x => request.VoucherDetailIds.Contains(x.Id));
        var voucherHeads = await _unitOfWork.VouchersHeads.GetListAsync(x => x.Id == request.VoucherHeadId || x.Id == voucherDetails.FirstOrDefault().VoucherId);
        var sourceVoucherHead = voucherHeads.FirstOrDefault(x => x.Id == voucherDetails.FirstOrDefault().VoucherId);
        var destinationVoucherHead = voucherHeads.FirstOrDefault(x => x.Id == request.VoucherHeadId);
        foreach (var voucherDetail in voucherDetails)
        {
            voucherDetail.VoucherId = request.VoucherHeadId;
            _unitOfWork.VouchersDetails.Update(voucherDetail);
        }

        sourceVoucherHead.TotalCredit -= voucherDetails.Sum(x => x.Credit);
        sourceVoucherHead.TotalDebit -= voucherDetails.Sum(x => x.Debit);
        _unitOfWork.VouchersHeads.Update(sourceVoucherHead);

        destinationVoucherHead.TotalCredit += voucherDetails.Sum(x => x.Credit);
        destinationVoucherHead.TotalDebit += voucherDetails.Sum(x => x.Debit);
        _unitOfWork.VouchersHeads.Update(destinationVoucherHead);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success(true);
    }
}