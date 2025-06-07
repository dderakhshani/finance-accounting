using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

public class CreateCodeRowDescriptionCommand : IRequest<ServiceResult<CodeRowDescriptionModel>>, IMapFrom<CreateCodeRowDescriptionCommand>
{

    public string Title { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreateCodeRowDescriptionCommand, CodeRowDescription>()
            .IgnoreAllNonExisting();
    }
}

public class CreateCodeRowDescriptionCommandHandler : IRequestHandler<CreateCodeRowDescriptionCommand, ServiceResult<CodeRowDescriptionModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IApplicationUser _applicationUser;
    public CreateCodeRowDescriptionCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IApplicationUser applicationUser)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _applicationUser = applicationUser;
    }

    public async Task<ServiceResult<CodeRowDescriptionModel>> Handle(CreateCodeRowDescriptionCommand request, CancellationToken cancellationToken)
    {
        CodeRowDescription entity = _mapper.Map<CodeRowDescription>(request);
        entity.CompanyId = _applicationUser.CompanyId;

        _unitOfWork.CodeRowDescriptions.Add(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success(_mapper.Map<CodeRowDescriptionModel>(entity));
    }
}