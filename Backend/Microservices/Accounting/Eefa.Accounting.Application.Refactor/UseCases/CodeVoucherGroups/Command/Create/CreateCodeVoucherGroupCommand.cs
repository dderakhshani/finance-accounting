using System;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

public class CreateCodeVoucherGroupCommand : IRequest<ServiceResult<CodeVoucherGroupModel>>, IMapFrom<CreateCodeVoucherGroupCommand>
{
    public string Code { get; set; }

    public string Title { get; set; }
    public string UniqueName { get; set; }
    public DateTime? LastEditableDate { get; set; }
    public bool IsAuto { get; set; }
    public bool IsEditable { get; set; }
    public bool IsActive { get; set; }
    public bool AutoVoucherEnterGroup { get; set; }
    public string BlankDateFormula { get; set; }
    public int? ViewId { get; set; }
    public int? ExtendTypeId { get; set; }
    public string? TableName { get; set; }
    public string? SchemaName { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreateCodeVoucherGroupCommand, CodeVoucherGroup>()
            .IgnoreAllNonExisting();
    }
}

public class CreateCodeVoucherGroupCommandHandler : IRequestHandler<CreateCodeVoucherGroupCommand, ServiceResult<CodeVoucherGroupModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IApplicationUser _applicationUser;
    public CreateCodeVoucherGroupCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IApplicationUser applicationUser)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _applicationUser = applicationUser;
    }


    public async Task<ServiceResult<CodeVoucherGroupModel>> Handle(CreateCodeVoucherGroupCommand request, CancellationToken cancellationToken)
    {
        CodeVoucherGroup entity = _mapper.Map<CodeVoucherGroup>(request);
        entity.CompanyId = _applicationUser.CompanyId;

        _unitOfWork.CodeVoucherGroups.Add(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success(_mapper.Map<CodeVoucherGroupModel>(entity));
    }
}