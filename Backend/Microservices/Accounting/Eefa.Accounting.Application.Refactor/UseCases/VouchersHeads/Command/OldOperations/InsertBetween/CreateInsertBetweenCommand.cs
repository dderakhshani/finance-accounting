using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;

public class CreateInsertBetweenCommand : IRequest<ServiceResult>, IMapFrom<CreateInsertBetweenCommand>
{
    public int InsertAfterVoucherNo { get; set; }
    public CreateVouchersHeadCommand CreateVouchersHeadCommand { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreateInsertBetweenCommand, VouchersHead>()
            .IgnoreAllNonExisting();
    }
}

//public class CreateInsertBetweenCommandHandler : IRequestHandler<CreateInsertBetweenCommand, ServiceResult>
//{
//    private readonly IUnitOfWork _unitOfWork;
//    private readonly IMapper _mapper;
//    private readonly IApplicationUser _applicationUser;
//    private readonly IMediator _mediator;
//    public CreateInsertBetweenCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IApplicationUser applicationUser, IMediator mediator)
//    {
//        _mapper = mapper;
//        _applicationUser = applicationUser;
//        _mediator = mediator;
//        _unitOfWork = unitOfWork;
//    }

//    public async Task<ServiceResult> Handle(CreateInsertBetweenCommand request, CancellationToken cancellationToken)
//    {
//        var year = await _unitOfWork.Years.GetByIdAsync(_applicationUser.YearId);

//        Specification<VouchersHead> specification = new Specification<VouchersHead>();
//        specification.ApplicationConditions.Add(x =>
//                        x.CompanyId == _applicationUser.CompanyId &&
//                        x.VoucherNo > request.InsertAfterVoucherNo);
//        specification.OrderBy = y =>
//           y.OrderBy(x => x.VoucherNo)
//            .ThenBy(x => x.CompanyId)
//            .ThenBy(x => x.YearId)
//            .ThenBy(x => x.CodeVoucherGroup.OrderIndex)
//            .ThenBy(x => x.VoucherNo)
//            .ThenBy(x => x.VoucherDailyId);

//        var voucherHeads = await _unitOfWork.VouchersHeads.GetListAsync(specification);

//        var settings = await new SystemSettings(_unitOfWork).Get(SubSystemType.AccountingSettings);
//        var voucherNumberType = new int();

//        foreach (var baseValue in settings)
//        {
//            if (baseValue.UniqueName == "VoucherNumberType")
//            {
//                voucherNumberType = int.Parse(baseValue.Value);
//                break;
//            }
//        }

//        var minVoucherNo = request.InsertAfterVoucherNo;

//        if (voucherNumberType == 2)
//        {
//            request.CreateVouchersHeadCommand.VoucherNo = int.Parse($"{_applicationUser.BranchId}{++minVoucherNo}");
//        }
//        else
//        {
//            request.CreateVouchersHeadCommand.VoucherNo = ++minVoucherNo;
//        }

//        request.SaveChanges = false;

//        var res = await _mediator.Send(request.CreateVouchersHeadCommand, cancellationToken);
//        if (res.Succeed is false)
//        {
//            return ServiceResult.Failure();
//        }


//        foreach (var vouchersHead in voucherHeads)
//        {
//            if (voucherNumberType == 2)
//            {
//                vouchersHead.VoucherNo = int.Parse($"{_applicationUser.BranchId}{++minVoucherNo}");
//            }
//            else
//            {
//                vouchersHead.VoucherNo = ++minVoucherNo;
//            }

//            _unitOfWork.VouchersHeads.Update(vouchersHead);
//        }

//        await _unitOfWork.SaveChangesAsync(cancellationToken);
//        return ServiceResult.Success();
//    }
//}