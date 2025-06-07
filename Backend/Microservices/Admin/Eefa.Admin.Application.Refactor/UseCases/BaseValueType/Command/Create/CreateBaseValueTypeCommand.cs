using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;

public class CreateBaseValueTypeCommand : IRequest<ServiceResult<BaseValueTypeModel>>, IMapFrom<CreateBaseValueTypeCommand>
{
    public int? ParentId { get; set; }
    public string Title { get; set; } = default!;
    public string UniqueName { get; set; } = default!;
    public string? GroupName { get; set; }
    public bool IsReadOnly { get; set; } = default!;
    public string? SubSystem { get; set; }
    public string LevelCode { get; set; } = "0";


    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreateBaseValueTypeCommand, BaseValueType>()
            .IgnoreAllNonExisting();
    }
}

public class CreateBaseValueTypeCommandHandler : IRequestHandler<CreateBaseValueTypeCommand, ServiceResult<BaseValueTypeModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public CreateBaseValueTypeCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }


    public async Task<ServiceResult<BaseValueTypeModel>> Handle(CreateBaseValueTypeCommand request, CancellationToken cancellationToken)
    {
        BaseValueType entity = _mapper.Map<BaseValueType>(request);

        _unitOfWork.BaseValueTyps.Add(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success(_mapper.Map<BaseValueTypeModel>(request));

    }
}