using System;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

public class UpdateCodeVoucherGroupCommand : IRequest<ServiceResult<CodeVoucherGroupModel>>, IMapFrom<CodeVoucherGroup>
{
    public int Id { get; set; }
    public string Code { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string UniqueName { get; set; }
    public DateTime? LastEditableDate { get; set; }
    public bool IsAuto { get; set; } = default!;
    public bool IsEditable { get; set; } = default!;
    public bool IsActive { get; set; } = default!;
    public bool AutoVoucherEnterGroup { get; set; } = default!;
    public string? BlankDateFormula { get; set; }
    public int? ViewId { get; set; }
    public int? ExtendTypeId { get; set; }
    public string? TableName { get; set; }
    public string? SchemaName { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<UpdateCodeVoucherGroupCommand, CodeVoucherGroup>()
            .IgnoreAllNonExisting();
    }
}


public class UpdateCodeVoucherGroupCommandHandler : IRequestHandler<UpdateCodeVoucherGroupCommand, ServiceResult<CodeVoucherGroupModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IApplicationUser _applicationUser;
    public UpdateCodeVoucherGroupCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<CodeVoucherGroupModel>> Handle(UpdateCodeVoucherGroupCommand request, CancellationToken cancellationToken)
    {
        CodeVoucherGroup entity = await _unitOfWork.CodeVoucherGroups.GetByIdAsync(request.Id);

        _mapper.Map(entity, request);
        entity.CompanyId = _applicationUser.CompanyId;

        _unitOfWork.CodeVoucherGroups.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success(_mapper.Map<CodeVoucherGroupModel>(entity));
    }
}