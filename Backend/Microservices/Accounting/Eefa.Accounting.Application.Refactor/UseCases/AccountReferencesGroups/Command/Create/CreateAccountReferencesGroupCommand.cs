using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;

public class CreateAccountReferencesGroupCommand : IRequest<ServiceResult<AccountReferencesGroupModel>>, IMapFrom<CreateAccountReferencesGroupCommand>
{
    public int? ParentId { get; set; }
    public string Title { get; set; }
    public bool? IsEditable { get; set; }
    public string Code { get; set; }


    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreateAccountReferencesGroupCommand, AccountReferencesGroup>()
            .IgnoreAllNonExisting();
    }
}

public class CreateAccountReferencesGroupCommandHandler : IRequestHandler<CreateAccountReferencesGroupCommand, ServiceResult<AccountReferencesGroupModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public CreateAccountReferencesGroupCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork= unitOfWork;
    }


    public async Task<ServiceResult<AccountReferencesGroupModel>> Handle(CreateAccountReferencesGroupCommand request, CancellationToken cancellationToken)
    {
        AccountReferencesGroup entity = _mapper.Map<AccountReferencesGroup>(request);

        _unitOfWork.AccountReferencesGroups.Add(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success(_mapper.Map<AccountReferencesGroupModel>(entity));
    }
}