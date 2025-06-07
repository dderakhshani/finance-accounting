using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;

public class DeleteBaseValueTypeCommand : IRequest<ServiceResult<BaseValueTypeModel>>
{
    public int Id { get; set; }
}

public class DeleteBaseValueTypeCommandHandler : IRequestHandler<DeleteBaseValueTypeCommand, ServiceResult<BaseValueTypeModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DeleteBaseValueTypeCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<BaseValueTypeModel>> Handle(DeleteBaseValueTypeCommand request, CancellationToken cancellationToken)
    {
        BaseValueType entity = await _unitOfWork.BaseValueTyps.GetByIdAsync(request.Id);

        _unitOfWork.BaseValueTyps.Delete(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success(_mapper.Map<BaseValueTypeModel>(entity));

    }
}