using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;

public class GetEndVoucherRequirementQuery : IRequest<ServiceResult>
{
}

//public class GetEndVoucherRequirementQueryHandler : IRequestHandler<GetEndVoucherRequirementQuery, ServiceResult>
//{
//    private readonly IUnitOfWork _unitOfWork;
//    private readonly IMapper _mapper;

//    public GetEndVoucherRequirementQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
//    {
//        _mapper = mapper;
//        _unitOfWork= unitOfWork;
//    }

//    public async Task<ServiceResult> Handle(GetEndVoucherRequirementQuery request, CancellationToken cancellationToken)
//    {
//        var requirements = new Dictionary<string, bool>();
//        requirements.Add("IsAllVouchersPermanented", await _unitOfWork.VouchersHeads
//            .ExistsAsync(x => x.VoucherStateId != 3));
//        requirements.Add("IsAllVoucherBalanced", !await _unitOfWork.VouchersHeads.Include(x => x.VouchersDetails)
//            .ExistsAsync(x => x.Difference != 0));
//        requirements.Add("IsEndVoucherGroupActive", !await _unitOfWork.CodeVoucherGroups
//            .ExistsAsync(x => x.UniqueName == "end" && x.IsActive == false));
//        requirements.Add("IsEndVoucherGroupExists", await _unitOfWork.CodeVoucherGroups
//            .ExistsAsync(x => x.UniqueName == "end"));
//        requirements.Add("IsAdjustmentVoucherExists", await _unitOfWork.VouchersHeads
//            .ExistsAsync(x => x.CodeVoucherGroup.UniqueName == "balancing"));
//        requirements.Add("IsCloseVoucherExists", await _unitOfWork.VouchersHeads
//            .ExistsAsync(x => x.CodeVoucherGroup.UniqueName == "temproraryEnd"));

//        return ServiceResult.Success(requirements);
//    }
//}