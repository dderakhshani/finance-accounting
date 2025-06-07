using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;

public class UpdateCodeVoucherExtendTypeCommand : IRequest<ServiceResult<CodeVoucherExtendTypeModel>>, IMapFrom<CodeVoucherExtendType>
{
    public int Id { get; set; }
    public string Title { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<UpdateCodeVoucherExtendTypeCommand, CodeVoucherExtendType>()
            .IgnoreAllNonExisting();
    }
}

public class UpdateCodeVoucherExtendTypeCommandHandler : IRequestHandler<UpdateCodeVoucherExtendTypeCommand, ServiceResult<CodeVoucherExtendTypeModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateCodeVoucherExtendTypeCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<CodeVoucherExtendTypeModel>> Handle(UpdateCodeVoucherExtendTypeCommand request, CancellationToken cancellationToken)
    {
        CodeVoucherExtendType entity = await _unitOfWork.CodeVoucherExtendTypes.GetByIdAsync(request.Id);

        _mapper.Map(entity, request);

        _unitOfWork.CodeVoucherExtendTypes.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success(_mapper.Map<CodeVoucherExtendTypeModel>(entity));
    }
}