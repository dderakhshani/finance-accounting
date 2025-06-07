using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;

public class GetAllBySpecificQuery : Specification<VouchersHead>, IRequest<ServiceResult<PaginatedList<VouchersHeadModel>>>
{
    public DateTime? FromDateTime { get; set; }
    public DateTime? ToDateTime { get; set; }

    public int? FromNo { get; set; }
    public int? ToNo { get; set; }

    public int? VoucherStateId { get; set; }
    public int? CodeVoucherGroupId { get; set; }
}

public class GetAllBySpecificQueryHandler : IRequestHandler<GetAllBySpecificQuery, ServiceResult<PaginatedList<VouchersHeadModel>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IApplicationUser _applicationUser;
    public GetAllBySpecificQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IApplicationUser applicationUser)
    {
        _mapper = mapper;
        _applicationUser = applicationUser;
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<PaginatedList<VouchersHeadModel>>> Handle(GetAllBySpecificQuery request, CancellationToken cancellationToken)
    {
        var year = await _unitOfWork.Years.GetByIdAsync(_applicationUser.YearId);

        request.ApplicationConditions.Add(x => x.VoucherStateId != 3);

        if (request.FromDateTime != null)
        {
            request.ApplicationConditions.Add(x =>
                x.VoucherDate >= request.FromDateTime);
        }
        else
        {
            request.ApplicationConditions.Add(x =>
                x.VoucherDate >= year.FirstDate);
        }

        if (request.ToDateTime != null)
        {
            request.ApplicationConditions.Add(x =>
                x.VoucherDate <= request.ToDateTime);
        }
        else
        {
            request.ApplicationConditions.Add(x =>
                x.VoucherDate <= year.LastDate);
        }

        if (request.FromNo != null)
        {
            request.ApplicationConditions.Add(x =>
                x.VoucherNo >= request.FromNo);
        }

        if (request.ToNo != null)
        {
            request.ApplicationConditions.Add(x =>
                x.VoucherNo <= request.ToNo);
        }

        if (request.VoucherStateId != null)
        {
            request.ApplicationConditions.Add(x => x.VoucherStateId == request.VoucherStateId);
        }

        if (request.CodeVoucherGroupId != null)
        {
            request.ApplicationConditions.Add(x => x.CodeVoucherGroupId == request.CodeVoucherGroupId);
        }

        return ServiceResult.Success(await _unitOfWork.VouchersHeads
            .GetPaginatedProjectedListAsync<VouchersHeadModel>(request));
    }
}