using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;

public class RemoveReferencesFromGroupCommand : IRequest<ServiceResult<List<AccountReferencesRelReferencesGroup>>>
{
    public int GroupId { get; set; }
    public ICollection<int> AccountReferenceIds { get; set; }
}

public class RemoveReferencesFromGroupCommandHandler : IRequestHandler<RemoveReferencesFromGroupCommand, ServiceResult<List<AccountReferencesRelReferencesGroup>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public RemoveReferencesFromGroupCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<List<AccountReferencesRelReferencesGroup>>> Handle(RemoveReferencesFromGroupCommand request, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.AccountReferencesRelReferencesGroups
            .GetListAsync(x => x.ReferenceGroupId == request.GroupId &&
                        request.AccountReferenceIds.Contains(x.ReferenceId));

        entity.ForEach(x => _unitOfWork.AccountReferencesRelReferencesGroups.Delete(x));
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success(entity);
    }
}