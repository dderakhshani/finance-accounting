using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

public class UpdateCodeRowDescriptionCommand : IRequest<ServiceResult<CodeRowDescriptionModel>>, IMapFrom<CodeRowDescription>
{
    public int Id { get; set; }
    public string Title { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<UpdateCodeRowDescriptionCommand, CodeRowDescription>()
            .IgnoreAllNonExisting();
    }
}


public class UpdateCodeRowDescriptionCommandHandler : IRequestHandler<UpdateCodeRowDescriptionCommand, ServiceResult<CodeRowDescriptionModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IApplicationUser _applicationUser;
    public UpdateCodeRowDescriptionCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IApplicationUser applicationUser)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _applicationUser = applicationUser;
    }

    public async Task<ServiceResult<CodeRowDescriptionModel>> Handle(UpdateCodeRowDescriptionCommand request, CancellationToken cancellationToken)
    {
        CodeRowDescription entity = await _unitOfWork.CodeRowDescriptions.GetByIdAsync(request.Id);

        _mapper.Map(entity, request);
        entity.CompanyId = _applicationUser.CompanyId;

        _unitOfWork.CodeRowDescriptions.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success(_mapper.Map<CodeRowDescriptionModel>(entity));
    }
}