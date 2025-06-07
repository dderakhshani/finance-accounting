using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;

public class CreateBranchCommand : IRequest<ServiceResult<BranchModel>>, IMapFrom<CreateBranchCommand>
{
    public string Title { get; set; } = default!;
    public int? ParentId { get; set; }
    public double? Lat { get; set; }
    public double? Lng { get; set; }


    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreateBranchCommand, Branch>()
            .IgnoreAllNonExisting();
    }
}

public class CreateBranchCommandHandler : IRequestHandler<CreateBranchCommand, ServiceResult<BranchModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateBranchCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<BranchModel>> Handle(CreateBranchCommand request, CancellationToken cancellationToken)
    {
        Branch entity = _mapper.Map<Branch>(request);

        _unitOfWork.Branchs.Add(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success(_mapper.Map<BranchModel>(entity));
    }
}