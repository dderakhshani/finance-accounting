using MediatR;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Eefa.Admin.Application.CommandQueries.PersonBankAccounts.Models;
using Microsoft.EntityFrameworkCore;

public class GetPersonBankAccountQuery : IRequest<ServiceResult<PersonBankAccountModel>>
{
    public int Id { get; set; }
}

public class GetPersonBankAccountQueryHandler : IRequestHandler<GetPersonBankAccountQuery, ServiceResult<PersonBankAccountModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetPersonBankAccountQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ServiceResult<PersonBankAccountModel>> Handle(GetPersonBankAccountQuery request, CancellationToken cancellationToken)
    {
        return ServiceResult.Success(await _unitOfWork.PersonBankAccounts
            .GetProjectedByIdAsync<PersonBankAccountModel>(request.Id));
    }
}