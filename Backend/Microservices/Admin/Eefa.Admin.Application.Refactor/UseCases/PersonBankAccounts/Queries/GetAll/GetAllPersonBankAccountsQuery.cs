using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Eefa.Admin.Application.CommandQueries.PersonBankAccounts.Models;
using Microsoft.EntityFrameworkCore;

public class GetAllPersonBankAccountsQuery : Specification<PersonBankAccount>, IRequest<ServiceResult<PaginatedList<PersonBankAccountModel>>>
{
}

public class GetAllPersonBankAccountsQueryHandler : IRequestHandler<GetAllPersonBankAccountsQuery, ServiceResult<PaginatedList<PersonBankAccountModel>>>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetAllPersonBankAccountsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<ServiceResult<PaginatedList<PersonBankAccountModel>>> Handle(GetAllPersonBankAccountsQuery request, CancellationToken cancellationToken)
    {
        return ServiceResult.Success(await _unitOfWork.PersonBankAccounts
                            .GetPaginatedProjectedListAsync<PersonBankAccountModel>(request));
    }
}