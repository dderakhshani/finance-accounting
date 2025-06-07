using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

public class DeleteYearCommand : IRequest<ServiceResult<YearModel>>
{
    public int Id { get; set; }
}

public class DeleteYearCommandHandler : IRequestHandler<DeleteYearCommand, ServiceResult<YearModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DeleteYearCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<YearModel>> Handle(DeleteYearCommand request, CancellationToken cancellationToken)
    {
        Year entity = await _unitOfWork.Years.GetByIdAsync(request.Id);

        _unitOfWork.Years.Delete(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success(_mapper.Map<YearModel>(entity));
    }
}