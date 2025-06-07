using System.Collections.Generic;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using System.Linq;
using Microsoft.EntityFrameworkCore;

public class GetAllMoadianInvoiceHeadersQuery : Specification<MoadianInvoiceHeader>, IRequest<ServiceResult<PaginatedList<MoadianInvoiceHeaderModel>>>
{
    public bool IsProduction { get; set; }
}

public class GetAllMoadianInvoiceHeadersQueryHandler : IRequestHandler<GetAllMoadianInvoiceHeadersQuery, ServiceResult<PaginatedList<MoadianInvoiceHeaderModel>>>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetAllMoadianInvoiceHeadersQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<PaginatedList<MoadianInvoiceHeaderModel>>> Handle(GetAllMoadianInvoiceHeadersQuery request, CancellationToken cancellationToken)
    {
        request.Includes = x => x.Include(x => x.CreatedBy).ThenInclude(x => x.Person);

        return ServiceResult.Success(await _unitOfWork.MoadianInvoiceHeaders
            .GetPaginatedProjectedListAsync<MoadianInvoiceHeaderModel>(request));
    }
}