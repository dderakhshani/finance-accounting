using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;

public class CreateEmployeeCommand : IRequest<ServiceResult<EmployeeModel>>, IMapFrom<CreateEmployeeCommand>
{
    public int PersonId { get; set; } = default!;
    public int UnitPositionId { get; set; } = default!;
    public string EmployeeCode { get; set; } = default!;
    public DateTime EmploymentDate { get; set; } = default!;


    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreateEmployeeCommand, Employee>()
            .IgnoreAllNonExisting();
    }
}

public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, ServiceResult<EmployeeModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateEmployeeCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<EmployeeModel>> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
    {
        Employee entity = _mapper.Map<Employee>(request);

        _unitOfWork.Employees.Add(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success(_mapper.Map<EmployeeModel>(request));
    }
}