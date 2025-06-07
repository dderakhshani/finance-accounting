using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;

public class UnLockVoucherCommand : IRequest<ServiceResult<VouchersHeadModel>>
{
    public DateTime? FromDateTime { get; set; }
    public DateTime? ToDateTime { get; set; }

    public int? FromNo { get; set; }
    public int? ToNo { get; set; }

    public int? VoucherStateId { get; set; }
    public int? CodeVoucherGroupId { get; set; }
}

public class UnLockVoucherCommandHandler : IRequestHandler<UnLockVoucherCommand, ServiceResult<VouchersHeadModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IApplicationUser _applicationUser;

    public UnLockVoucherCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IApplicationUser applicationUserAccessor)
    {
        _mapper = mapper;
        _applicationUser = applicationUserAccessor;
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<VouchersHeadModel>> Handle(UnLockVoucherCommand request, CancellationToken cancellationToken)
    {
        var year = await _unitOfWork.Years.GetByIdAsync(_applicationUser.YearId);

        Specification<VouchersHead> specification = new Specification<VouchersHead>();


        if (request.FromDateTime != null)
        {
            specification.ApplicationConditions.Add(x => x.VoucherDate >= request.FromDateTime);
        }
        else
        {
            specification.ApplicationConditions.Add(x => x.VoucherDate >= year.FirstDate
            );
        }

        if (request.ToDateTime != null)
        {
            specification.ApplicationConditions.Add(x => x.VoucherDate <= request.ToDateTime);
        }
        else
        {
            specification.ApplicationConditions.Add(x => x.VoucherDate <= year.LastDate);
        }

        if (request.FromNo != null)
        {
            specification.ApplicationConditions.Add(x => x.VoucherNo >= request.FromNo);
        }

        if (request.ToNo != null)
        {
            specification.ApplicationConditions.Add(x => x.VoucherNo <= request.ToNo);
        }

        if (request.VoucherStateId != null)
        {
            specification.ApplicationConditions.Add(x => x.VoucherStateId == request.VoucherStateId);
        }

        if (request.CodeVoucherGroupId != null)
        {
            specification.ApplicationConditions.Add(x => x.CodeVoucherGroupId == request.CodeVoucherGroupId);
        }
        var entity = await _unitOfWork.VouchersHeads.GetAsync(specification);

        entity.VoucherStateId = 3;

        _unitOfWork.VouchersHeads.Update(entity);

        //foreach (var vouchersHead in await query.ToListAsync(cancellationToken))
        //{
        //    vouchersHead.VoucherStateId = 3; // دائم

        //    _unitOfWorkUpdate(vouchersHead);
        //}

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success(_mapper.Map<VouchersHeadModel>(entity));
    }
}