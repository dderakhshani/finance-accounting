using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;

public class UpdateBaseValueCommand : IRequest<ServiceResult<BaseValueModel>>, IMapFrom<BaseValue>
{
    public int Id { get; set; }
    public int BaseValueTypeId { get; set; } = default!;
    public int? ParentId { get; set; }
    public string Title { get; set; } = default!;
    public string UniqueName { get; set; } = default!;
    public string Value { get; set; } = default!;
    public int OrderIndex { get; set; } = default!;
    public bool IsReadOnly { get; set; } = default!;
    public string Code { get; set; }


    public void Mapping(Profile profile)
    {
        profile.CreateMap<UpdateBaseValueCommand, BaseValue>()
            .IgnoreAllNonExisting();
    }
}


public class UpdateBaseValueCommandHandler : IRequestHandler<UpdateBaseValueCommand, ServiceResult<BaseValueModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateBaseValueCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<BaseValueModel>> Handle(UpdateBaseValueCommand request, CancellationToken cancellationToken)
    {
        BaseValue entity = await _unitOfWork.BaseValues.GetByIdAsync(request.Id);

        _mapper.Map(request, entity);

        _unitOfWork.BaseValues.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return ServiceResult.Success(_mapper.Map<BaseValueModel>(entity));
    }
}