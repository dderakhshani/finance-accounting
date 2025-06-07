using System.Collections.Generic;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using System.Linq;

public class GetAllVouchersHeadQuery : Specification<VouchersHead>, IRequest<ServiceResult<PaginatedList<VouchersHeadModel>>>
{
}

public class GetAllVouchersHeadQueryHandler : IRequestHandler<GetAllVouchersHeadQuery, ServiceResult<PaginatedList<VouchersHeadModel>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IApplicationUser _applicationUser;

    public GetAllVouchersHeadQueryHandler(IUnitOfWork unitOfWork, IApplicationUser applicationUser)
    {
        _applicationUser = applicationUser;
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<PaginatedList<VouchersHeadModel>>> Handle(GetAllVouchersHeadQuery request, CancellationToken cancellationToken)
    {
        request.ApplicationConditions.Add(x => x.YearId == _applicationUser.YearId);
        return ServiceResult.Success(await _unitOfWork.VouchersHeads
                .GetPaginatedProjectedListAsync<VouchersHeadModel>(request));
    }
}