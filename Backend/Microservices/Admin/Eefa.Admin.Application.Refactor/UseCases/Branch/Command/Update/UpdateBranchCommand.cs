using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;

public class UpdateBranchCommand : IRequest<ServiceResult<BranchModel>>, IMapFrom<Branch>
{
    public int Id { get; set; }
    public string Title { get; set; } = default!;
    public int? ParentId { get; set; }
    public double? Lat { get; set; }
    public double? Lng { get; set; }


    public void Mapping(Profile profile)
    {
        profile.CreateMap<UpdateBranchCommand, Branch>()
            .IgnoreAllNonExisting();
    }
}

public class UpdateBranchCommandHandler : IRequestHandler<UpdateBranchCommand, ServiceResult<BranchModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateBranchCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<BranchModel>> Handle(UpdateBranchCommand request, CancellationToken cancellationToken)
    {
        Branch entity = await _unitOfWork.Branchs.GetByIdAsync(request.Id);

        _mapper.Map(request, entity);

        _unitOfWork.Branchs.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success(_mapper.Map<BranchModel>(entity));
    }
}