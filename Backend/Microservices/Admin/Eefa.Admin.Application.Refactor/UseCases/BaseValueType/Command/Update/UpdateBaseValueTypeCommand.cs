using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;


public class UpdateBaseValueTypeCommand : IRequest<ServiceResult<BaseValueTypeModel>>, IMapFrom<BaseValueType>
{
    public int Id { get; set; }
    public int? ParentId { get; set; }
    public string Title { get; set; } = default!;
    public string UniqueName { get; set; } = default!;
    public string? GroupName { get; set; }
    public bool IsReadOnly { get; set; } = default!;
    public string? SubSystem { get; set; }


    public void Mapping(Profile profile)
    {
        profile.CreateMap<UpdateBaseValueTypeCommand, BaseValueType>()
            .IgnoreAllNonExisting();
    }
}


public class UpdateBaseValueTypeCommandHandler : IRequestHandler<UpdateBaseValueTypeCommand, ServiceResult<BaseValueTypeModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateBaseValueTypeCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<BaseValueTypeModel>> Handle(UpdateBaseValueTypeCommand request, CancellationToken cancellationToken)
    {
        BaseValueType entity = await _unitOfWork.BaseValueTyps.GetByIdAsync(request.Id);

        entity.UniqueName = request.UniqueName;
        entity.GroupName = request.GroupName;
        entity.IsReadOnly = request.IsReadOnly;
        entity.SubSystem = request.SubSystem;
        entity.Title = request.Title;

        _unitOfWork.BaseValueTyps.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success(_mapper.Map<BaseValueTypeModel>(request));

    }
}