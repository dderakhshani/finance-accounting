using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

// TODO Handler class have to chack...
public class CreateVouchersHeadCommand : IRequest<ServiceResult>, IMapFrom<CreateVouchersHeadCommand>
{
    public int? VoucherNo { get; set; } = default!;
    public int VoucherDailyId { get; set; }
    public DateTime VoucherDate { get; set; } = default!;
    public string VoucherDescription { get; set; } = default!;
    public int CodeVoucherGroupId { get; set; } = default!;
    public int VoucherStateId { get; set; } = default!;
    public int? AutoVoucherEnterGroup { get; set; }
    public List<CreateVouchersDetailCommand> VouchersDetailsList { get; set; } = new List<CreateVouchersDetailCommand> { };


    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreateVouchersHeadCommand, VouchersHead>()
            .IgnoreAllNonExisting();
    }
}

//public class CreateVouchersHeadCommandHandler : IRequestHandler<CreateVouchersHeadCommand, ServiceResult>
//{
//    private readonly IUnitOfWork _unitOfWork;
//    private readonly IMapper _mapper;
//    private readonly IApplicationUser _applicationUser;
//    private readonly IMediator _mediator;
//    public IVoucherHeadCacheServices _voucherHeadCacheServices { get; }
//    public CreateVouchersHeadCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IApplicationUser applicationUser, IMediator mediator, IVoucherHeadCacheServices voucherHeadCacheServices)
//    {
//        _mapper = mapper;
//        _applicationUser = applicationUser;
//        _mediator = mediator;
//        _voucherHeadCacheServices = voucherHeadCacheServices;
//        _unitOfWork= unitOfWork;
//    }

//    public async Task<ServiceResult> Handle(CreateVouchersHeadCommand request, CancellationToken cancellationToken)
//    {
//        var voucher = _mapper.Map<VouchersHead>(request);
//        voucher.YearId = _applicationUser.YearId;
//        voucher.CompanyId = _applicationUser.CompanyId;


//        if (voucher.VoucherNo == 0)
//        {
//            voucher.VoucherNo = await _voucherHeadCacheServices.GetNewVoucherNumber();
//        }

//        if (request.VoucherDailyId is 0)
//        {
//            request.VoucherDailyId = ((await _unitOfWorkGetQuery<VouchersHead>().Where(x =>
//                    x.VoucherDate.Date == request.VoucherDate.Date).OrderBy(x => x.Id)
//                .LastOrDefaultAsync(cancellationToken))?.VoucherDailyId ?? 0) + 1;
//        }

//        voucher.TotalCredit = request.VouchersDetailsList.Sum(x => x.Credit);
//        voucher.TotalDebit = request.VouchersDetailsList.Sum(x => x.Debit);

//        var voucherEntity = _unitOfWorkInsert(voucher);

//        foreach (var createVouchersDetailCommand in request.VouchersDetailsList)
//        {
//            createVouchersDetailCommand.SaveChanges = false;
//            createVouchersDetailCommand.VoucherDate = voucher.VoucherDate;
//            createVouchersDetailCommand.EntityEntryVouchersHead = voucherEntity.Entity;
//            await _mediator.Send(createVouchersDetailCommand, cancellationToken);
//        }

//        await _unitOfWork.SaveChangesAsync(cancellationToken);

//        return ServiceResult.Success(_mapper.Map<VouchersHeadWithDetailModel>(voucher));
//    }
//}