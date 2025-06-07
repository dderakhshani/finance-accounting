using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

public class CreateBaseValueCommand : IRequest<ServiceResult<BaseValueModel>>, IMapFrom<CreateBaseValueCommand>
{
    public int BaseValueTypeId { get; set; } = default!;
    public int? ParentId { get; set; }
    public string LevelCode { get; set; }
    public string Title { get; set; } = default!;
    public string UniqueName { get; set; } = default!;
    public string Value { get; set; } = default!;
    public int OrderIndex { get; set; } = default!;
    public bool IsReadOnly { get; set; } = default!;
    public string Code { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreateBaseValueCommand, BaseValue>()
            .IgnoreAllNonExisting();
    }
}

public class CreateBaseValueCommandHandler : IRequestHandler<CreateBaseValueCommand, ServiceResult<BaseValueModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public CreateBaseValueCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }


    public async Task<ServiceResult<BaseValueModel>> Handle(CreateBaseValueCommand request, CancellationToken cancellationToken)
    {
        BaseValue entity = _mapper.Map<BaseValue>(request);

        _unitOfWork.BaseValues.Add(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success(_mapper.Map<BaseValueModel>(entity));
    }
}