using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;

public class UpdateAccountReferencesGroupCommand : IRequest<ServiceResult<AccountReferencesGroupModel>>, IMapFrom<AccountReferencesGroup>
{
    public int Id { get; set; }
    public int? ParentId { get; set; }
    public string Code { get; set; }
    [UniqueIndex]
    public string Title { get; set; } = default!;
    public bool? IsEditable { get; set; } = default!;

    public void Mapping(Profile profile)
    {
        profile.CreateMap<UpdateAccountReferencesGroupCommand, AccountReferencesGroup>()
            .IgnoreAllNonExisting();
    }
}

public class UpdateReferencesGroupCommandHandler : IRequestHandler<UpdateAccountReferencesGroupCommand, ServiceResult<AccountReferencesGroupModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public UpdateReferencesGroupCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork= unitOfWork;
    }

    public async Task<ServiceResult<AccountReferencesGroupModel>> Handle(UpdateAccountReferencesGroupCommand request, CancellationToken cancellationToken)
    {
        AccountReferencesGroup entity = await _unitOfWork.AccountReferencesGroups.GetByIdAsync(request.Id);

        _mapper.Map(request, entity);

        _unitOfWork.AccountReferencesGroups.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success(_mapper.Map<AccountReferencesGroupModel>(entity));
    }
}