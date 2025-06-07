using System.Collections.Generic;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

public class UpdateAccountReferenceCommand : IRequest<ServiceResult<AccountReferenceModel>>, IMapFrom<AccountReference>
{
    public int Id { get; set; }
    public string Title { get; set; } = default!;
    public bool IsActive { get; set; } = default!;
    public string Code { get; set; }
    public string? Description { get; set; }
    public ICollection<int> ReferenceGroupsId { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<UpdateAccountReferenceCommand, AccountReference>()
            .IgnoreAllNonExisting();
    }
}

public class UpdateAccountReferenceCommandHandler : IRequestHandler<UpdateAccountReferenceCommand, ServiceResult<AccountReferenceModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateAccountReferenceCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork= unitOfWork;
    }

    public async Task<ServiceResult<AccountReferenceModel>> Handle(UpdateAccountReferenceCommand request, CancellationToken cancellationToken)
    {
        AccountReference entity = await _unitOfWork.AccountReferences.GetByIdAsync(request.Id);

        _mapper.Map(request, entity);

        _unitOfWork.AccountReferences.Update(entity);

        var entityAccountRefRelRefGroup = (await _unitOfWork.AccountReferencesRelReferencesGroups
                                .GetListAsync(x => x.ReferenceId == entity.Id));

        foreach (var removedAccountRefRelRefGroup in entityAccountRefRelRefGroup.Select(x => 
                                                     x.ReferenceGroupId).Except(request.ReferenceGroupsId))
        {
            var removing = await _unitOfWork.AccountReferencesRelReferencesGroups.GetAsync(x =>
                x.ReferenceGroupId == removedAccountRefRelRefGroup && x.ReferenceId == request.Id);

            _unitOfWork.AccountReferencesRelReferencesGroups.Delete(removing);
        }

        foreach (var insertedAccountRefRelRefGroup in request.ReferenceGroupsId.Except(entityAccountRefRelRefGroup.Select(x => x.ReferenceGroupId)))
        {
            if (await _unitOfWork.AccountReferencesRelReferencesGroups.ExistsAsync(x =>
                x.ReferenceId == entity.Id && x.ReferenceGroupId == insertedAccountRefRelRefGroup &&
                x.IsDeleted != true))
            {
                continue;
            }
            _unitOfWork.AccountReferencesRelReferencesGroups.Add(new AccountReferencesRelReferencesGroup()
            { ReferenceGroupId = insertedAccountRefRelRefGroup, ReferenceId = entity.Id });
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success(_mapper.Map<AccountReferenceModel>(entity));
    }
}