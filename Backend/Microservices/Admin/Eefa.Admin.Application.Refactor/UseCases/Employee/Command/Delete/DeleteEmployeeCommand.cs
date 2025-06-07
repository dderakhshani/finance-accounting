using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;

public class DeleteEmployeeCommand : IRequest<ServiceResult<EmployeeModel>>
{
    public int Id { get; set; }
}

public class DeleteEmployeeCommandHandler : IRequestHandler<DeleteEmployeeCommand, ServiceResult<EmployeeModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DeleteEmployeeCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<EmployeeModel>> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
    {
        Employee entity = await _unitOfWork.Employees.GetByIdAsync(request.Id);

        _unitOfWork.Employees.Delete(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success(_mapper.Map<EmployeeModel>(entity));
    }
}