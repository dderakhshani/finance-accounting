using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;

public class DeleteCodeVoucherExtendTypeCommand : IRequest<ServiceResult<CodeVoucherExtendTypeModel>>
{
    public int Id { get; set; }
}

public class DeleteCodeVoucherExtendTypeCommandHandler : IRequestHandler<DeleteCodeVoucherExtendTypeCommand, ServiceResult<CodeVoucherExtendTypeModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DeleteCodeVoucherExtendTypeCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<CodeVoucherExtendTypeModel>> Handle(DeleteCodeVoucherExtendTypeCommand request, CancellationToken cancellationToken)
    {
        CodeVoucherExtendType entity = await _unitOfWork.CodeVoucherExtendTypes.GetByIdAsync(request.Id);

        _unitOfWork.CodeVoucherExtendTypes.Delete(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success(_mapper.Map<CodeVoucherExtendTypeModel>(entity));
    }
}