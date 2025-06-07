using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Eefa.Admin.Application.CommandQueries.PersonBankAccounts.Models;

public class UpdatePersonBankAccountCommand : IRequest<ServiceResult<PersonBankAccountModel>>, IMapFrom<PersonBankAccount>
{
    public int Id { get; set; }
    public int PersonId { get; set; }
    public int? BankId { get; set; }
    public string? BankBranchName { get; set; }
    public int AccountTypeBaseId { get; set; }
    public string AccountNumber { get; set; }
    public string? Description { get; set; }
    public bool IsDefault { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<UpdatePersonBankAccountCommand, PersonBankAccount>()
            .IgnoreAllNonExisting();
    }
}

public class UpdatePersonBankAccountCommandHandler : IRequestHandler<UpdatePersonBankAccountCommand, ServiceResult<PersonBankAccountModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdatePersonBankAccountCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<ServiceResult<PersonBankAccountModel>> Handle(UpdatePersonBankAccountCommand request, CancellationToken cancellationToken)
    {
        PersonBankAccount entity = await _unitOfWork.PersonBankAccounts.GetByIdAsync(request.Id);

        _mapper.Map(request, entity);

        _unitOfWork.PersonBankAccounts.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success(_mapper.Map<PersonBankAccountModel>(entity));
    }
}