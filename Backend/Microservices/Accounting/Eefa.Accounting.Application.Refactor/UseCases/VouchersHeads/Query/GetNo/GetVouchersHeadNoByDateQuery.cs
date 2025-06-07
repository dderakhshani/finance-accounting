using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

// TODO the Handler class
public class GetVouchersHeadNoByDateQuery : IRequest<ServiceResult<GetNoResultModel>>
{
    public DateTime FromDateTime { get; set; }
    public DateTime ToDateTime { get; set; }
}
//public class GetVouchersHeadNoByDateQueryHandler : IRequestHandler<GetVouchersHeadNoByDateQuery, ServiceResult<GetNoResultModel>>
//{
//    private readonly IAccountingUnitOfWork _context;
//    private readonly IApplicationUser _applicationUser;

//    public GetVouchersHeadNoByDateQueryHandler(IApplicationUser applicationUser, IAccountingUnitOfWork context)
//    {
//        _applicationUser = applicationUser;
//        _context = context;
//    }

//    public async Task<ServiceResult<GetNoResultModel>> Handle(GetVouchersHeadNoByDateQuery request, CancellationToken cancellationToken)
//    {
//        var minno = await _context.VouchersHeads.Where(a => a.VoucherDate >= request.FromDateTime && a.VoucherDate <= request.ToDateTime).MinAsync(a => a.VoucherNo);
//        var maxno = await _context.VouchersHeads.Where(a => a.VoucherDate >= request.FromDateTime && a.VoucherDate <= request.ToDateTime).MaxAsync(a => a.VoucherNo);
//        GetNoResultModel resultModel = new(minno, maxno);
//        return ServiceResult.Success(resultModel);
//    }
//}