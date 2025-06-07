using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;

public class CreateCodeVoucherExtendTypeCommand : IRequest<ServiceResult<CodeVoucherExtendTypeModel>>, IMapFrom<CreateCodeVoucherExtendTypeCommand>
{
    public string Title { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreateCodeVoucherExtendTypeCommand, CodeVoucherExtendType>()
            .IgnoreAllNonExisting();
    }
}

public class CreateCodeVoucherExtendTypeCommandHandler : IRequestHandler<CreateCodeVoucherExtendTypeCommand, ServiceResult<CodeVoucherExtendTypeModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateCodeVoucherExtendTypeCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }


    public async Task<ServiceResult<CodeVoucherExtendTypeModel>> Handle(CreateCodeVoucherExtendTypeCommand request, CancellationToken cancellationToken)
    {
        CodeVoucherExtendType entity = _mapper.Map<CodeVoucherExtendType>(request);

        _unitOfWork.CodeVoucherExtendTypes.Add(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success(_mapper.Map<CodeVoucherExtendTypeModel>(entity));
    }
}