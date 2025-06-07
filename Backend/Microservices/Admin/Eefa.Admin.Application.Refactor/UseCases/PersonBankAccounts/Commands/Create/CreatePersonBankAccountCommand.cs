using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Admin.Application.CommandQueries.PersonBankAccounts.Models;
using MediatR;

public class CreatePersonBankAccountCommand : IRequest<ServiceResult<PersonBankAccountModel>>, IMapFrom<PersonBankAccount>
{
    public int PersonId { get; set; }
    public int? BankId { get; set; }
    public string? BankBranchName { get; set; }
    public int AccountTypeBaseId { get; set; }
    public string AccountNumber { get; set; }
    public string? Description { get; set; }
    public bool IsDefault { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreatePersonBankAccountCommand, PersonBankAccount>();
    }
}

public class CreatePersonBankAccountCommandHandler : IRequestHandler<CreatePersonBankAccountCommand, ServiceResult<PersonBankAccountModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreatePersonBankAccountCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<ServiceResult<PersonBankAccountModel>> Handle(CreatePersonBankAccountCommand request, CancellationToken cancellationToken)
    {
        PersonBankAccount entity = _mapper.Map<PersonBankAccount>(request);

        _unitOfWork.PersonBankAccounts.Add(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success(_mapper.Map<PersonBankAccountModel>(entity));
    }
}