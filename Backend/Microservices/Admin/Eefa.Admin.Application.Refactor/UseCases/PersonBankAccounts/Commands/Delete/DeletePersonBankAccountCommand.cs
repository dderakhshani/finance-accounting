using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Eefa.Admin.Application.CommandQueries.PersonBankAccounts.Models;

public class DeletePersonBankAccountCommand : IRequest<ServiceResult<PersonBankAccountModel>>, IMapFrom<PersonBankAccount>
{
    public int Id { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<DeletePersonBankAccountCommand, PersonBankAccount>();
    }
}

public class DeletePersonBankAccountCommandHandler : IRequestHandler<DeletePersonBankAccountCommand, ServiceResult<PersonBankAccountModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DeletePersonBankAccountCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<ServiceResult<PersonBankAccountModel>> Handle(DeletePersonBankAccountCommand request, CancellationToken cancellationToken)
    {
        PersonBankAccount entity = await _unitOfWork.PersonBankAccounts.GetByIdAsync(request.Id);

        _unitOfWork.PersonBankAccounts.Delete(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success(_mapper.Map<PersonBankAccountModel>(entity));
    }
}