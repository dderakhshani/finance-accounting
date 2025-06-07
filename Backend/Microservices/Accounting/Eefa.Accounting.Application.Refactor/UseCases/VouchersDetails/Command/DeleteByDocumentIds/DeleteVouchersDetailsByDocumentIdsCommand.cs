using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;


public class DeleteVouchersDetailsByDocumentIdsCommand : IRequest<ServiceResult<VouchersDetailModel>>
{
    public int VoucherId { get; set; }
    public List<int> DocumentIds { get; set; }


}

//public class DeleteVouchersDetailsByDocumentIdsCommandHandler : IRequestHandler<DeleteVouchersDetailsByDocumentIdsCommand, ServiceResult<VouchersDetailModel>>
//{
//    private readonly IApplicationUser _applicationUser;
//    private readonly IMapper _mapper;
//    private readonly IUnitOfWork _unitOfWork;
//    public DeleteVouchersDetailsByDocumentIdsCommandHandler(IApplicationUser applicationUser, IMapper mapper, IUnitOfWork unitOfWork)
//    {
//        _unitOfWork = unitOfWork;
//        _mapper = mapper;
//        _applicationUser = applicationUser;
//    }

//    public async Task<ServiceResult<VouchersDetailModel>> Handle(DeleteVouchersDetailsByDocumentIdsCommand request, CancellationToken cancellationToken)
//    {
//        var entity = await _repository
//            .Find<Data.Entities.VouchersHead>(c =>
//         c.ObjectId(request.VoucherId))
//            .Include(x => x.VouchersDetails)
//         .FirstOrDefaultAsync(cancellationToken);
//        if (entity.VoucherStateId > 1) throw new ApplicationValidationException(new ApplicationErrorModel { Message = "سندی که قصد ویرایش آن را دارید در وضعیت دائم میباشد." });
//        var voucherDetailsToRemove = entity?.VouchersDetails.Where(x => x.DocumentId != null && request.DocumentIds.Contains((int)x.DocumentId)).ToList();
//        foreach (var detail in voucherDetailsToRemove)
//        {
//            _repository.Delete(detail);
//            entity.TotalCredit -= detail.Credit;
//            entity.TotalDebit -= detail.Debit;
//        }
//        var doesVoucherHasAnyOtherVoucherDetails = voucherDetailsToRemove.Count != entity.VouchersDetails.Count;
//        if (entity.TotalDebit == 0 && entity.TotalCredit == 0 && !doesVoucherHasAnyOtherVoucherDetails) _repository.Delete(entity);
//        else _repository.Update(entity);

//        await _repository.SaveChangesAsync(request.MenueId, cancellationToken);

//        return ServiceResult.Success();
//    }
//}