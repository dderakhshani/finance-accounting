using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
// TODO Handler class have to chack...
public class BulkVoucherStatusUpdateCommand : Specification<VouchersHead>, IRequest<ServiceResult>
{
    public List<int> VoucherIds { get; set; }
    public int Status { get; set; }
}

//public class BulkVoucherStatusUpdateCommandHandler : IRequestHandler<BulkVoucherStatusUpdateCommand, ServiceResult>
//{
//    private readonly IUnitOfWork _unitOfWork;

//    public BulkVoucherStatusUpdateCommandHandler(IUnitOfWork unitOfWork)
//    {
//        _unitOfWork= unitOfWork;
//    }

//    public async Task<ServiceResult> Handle(BulkVoucherStatusUpdateCommand request, CancellationToken cancellationToken)
//    {

//        var query = _unitOfWork.VouchersHeads;
//        var entities = query.GetListAsync();

//        if (request.VoucherIds?.Count > 0)
//        {
//            entities = query.GetListAsync(x => request.VoucherIds.Any(y => y == x.Id));
//        }
//        else
//        {
//            entities = query.GetListAsync(request);
//        }

//        await query.UpdateFromQueryAsync(x => new VouchersHead() { VoucherStateId = request.Status },
//            cancellationToken);

//        return ServiceResult.Success(null);
//    }
//}