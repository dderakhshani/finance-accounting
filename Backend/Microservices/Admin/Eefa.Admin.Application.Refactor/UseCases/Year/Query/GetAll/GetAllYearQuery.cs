using System.Collections.Generic;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

public class GetAllYearQuery : Specification<Year>, IRequest<ServiceResult<PaginatedList<YearModel>>>
{
}

public class GetAllYearQueryHandler : IRequestHandler<GetAllYearQuery, ServiceResult<PaginatedList<YearModel>>>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetAllYearQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<PaginatedList<YearModel>>> Handle(GetAllYearQuery request, CancellationToken cancellationToken)
    {
        return ServiceResult.Success(await _unitOfWork.Years
                            .GetPaginatedProjectedListAsync<YearModel>(request));
    } 
}