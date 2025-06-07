using System.Collections.Generic;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

public class CreateAccountHeadCommand : IRequest<ServiceResult<AccountHeadModel>>, IMapFrom<CreateAccountHeadCommand>
{
    public string Code { get; set; } = default!;
    public bool LastLevel { get; set; } = default!;
    public int? ParentId { get; set; }
    public string Title { get; set; } = default!;
    public int? BalanceId { get; set; }
    public int BalanceBaseId { get; set; } = default!;
    public int TransferId { get; set; } = default!;
    public int? GroupId { get; set; }
    public int CurrencyBaseTypeId { get; set; } = default!;
    public bool CurrencyFlag { get; set; } = default!;
    public bool ExchengeFlag { get; set; } = default!;
    public bool TraceFlag { get; set; } = default!;
    public bool QuantityFlag { get; set; } = default!;
    public bool? IsActive { get; set; } = default!;
    public string Description { get; set; }

    public List<AddAccountReferenceGroupToAccountHeadCommand> Groups { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreateAccountHeadCommand, AccountHead>()
            .IgnoreAllNonExisting();
    }
}

public class CreateAccountHeadCommandHandler : IRequestHandler<CreateAccountHeadCommand, ServiceResult<AccountHeadModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IApplicationUser _applicationUser;
    public CreateAccountHeadCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IApplicationUser currentUserAccessor)
    {
        _mapper = mapper;
        _applicationUser = currentUserAccessor;
        _unitOfWork= unitOfWork;
    }

    public async Task<ServiceResult<AccountHeadModel>> Handle(CreateAccountHeadCommand request, CancellationToken cancellationToken)
    {
        AccountHead entity = _mapper.Map<AccountHead>(request);

        if (request.ParentId != default)
        {
            entity.CodeLevel = await _unitOfWork.AccountHeads.GetProjectedByIdAsync(request.ParentId, x => int.Parse(x.LevelCode)) + 1;
        }
        else
        {
            entity.CodeLevel = 1;
        }
        entity.CompanyId = _applicationUser.CompanyId;

        if (request.Groups.Count > 0)
        {
            foreach (var group in request.Groups)
            {
                var newGroup = new AccountHeadRelReferenceGroup
                {
                    AccountHead = entity,
                    ReferenceGroupId = group.ReferenceGroupId,
                    ReferenceNo = group.ReferenceNo,
                };
                entity.AccountHeadRelReferenceGroups.Add(newGroup);
                _unitOfWork.AccountHeadRelReferenceGroups.Add(newGroup);
            }
        }

        _unitOfWork.AccountHeads.Add(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success(_mapper.Map<AccountHeadModel>(entity));
    }
}
