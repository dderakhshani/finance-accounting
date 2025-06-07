using System.Collections.Generic;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

public class UpdateAccountHeadCommand : IRequest<ServiceResult<AccountHeadModel>>, IMapFrom<UpdateAccountHeadCommand>
{
    public int Id { get; set; }
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
        profile.CreateMap<UpdateAccountHeadCommand, AccountHead>();
    }
}

public class UpdateAccountHeadCommandHandler : IRequestHandler<UpdateAccountHeadCommand, ServiceResult<AccountHeadModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public List<AccountHead> AccountHeads = new List<AccountHead>();
    public UpdateAccountHeadCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork= unitOfWork;
    }

    public async Task<ServiceResult<AccountHeadModel>> Handle(UpdateAccountHeadCommand request, CancellationToken cancellationToken)
    {
        AccountHead entity = await _unitOfWork.AccountHeads.GetByIdAsync(request.Id, 
            x => x.Include(y => y.AccountHeadRelReferenceGroups));

        if (request.Code != entity.Code)
        {
            this.AccountHeads = await _unitOfWork.AccountHeads.GetListAsync();

            await this.UpdateChildrenCode(entity.Id, entity.Code, request.Code);
        }

        _mapper.Map(request, entity);

        if (entity.AccountHeadRelReferenceGroups?.Count > 0 || request.Groups?.Count > 0)
        {
            var groupIdsToAdd = request.Groups?.Select(x => x.ReferenceGroupId).ToList().Except(entity.AccountHeadRelReferenceGroups?.Select(x => x.ReferenceGroupId).ToList()).ToList();
            var groupIdsToRemove = entity.AccountHeadRelReferenceGroups?.Select(x => x.ReferenceGroupId).ToList().Except(request.Groups?.Select(x => x.ReferenceGroupId).ToList()).ToList();

            foreach (var groupId in groupIdsToRemove)
            {
                _unitOfWork.AccountHeadRelReferenceGroups.Delete(entity.AccountHeadRelReferenceGroups.FirstOrDefault(x => x.ReferenceGroupId == groupId));
            }
            foreach (var groupId in groupIdsToAdd)
            {
                var newRelation = new AccountHeadRelReferenceGroup
                {
                    ReferenceGroupId = groupId,
                    ReferenceNo = request.Groups.FirstOrDefault(x => x.ReferenceGroupId == groupId).ReferenceNo
                };
                entity.AccountHeadRelReferenceGroups.Add(newRelation);
                _unitOfWork.AccountHeadRelReferenceGroups.Add(newRelation);
            }
        }

        _unitOfWork.AccountHeads.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success(_mapper.Map<AccountHeadModel>(entity));
    }

    public async Task UpdateChildrenCode(int parentId, string oldCode, string newCode)
    {
        foreach (var childAccountHead in this.AccountHeads.Where(x => x.ParentId == parentId).ToList())
        {
            var itemCode = childAccountHead.Code.Substring(oldCode.Length, (childAccountHead.Code.Length - oldCode.Length));
            childAccountHead.Code = newCode + itemCode;
            _unitOfWork.AccountHeads.Update(childAccountHead);
            if (this.AccountHeads.Any(x => x.ParentId == childAccountHead.Id)) await UpdateChildrenCode(childAccountHead.Id, oldCode, newCode);
        }
    }
}