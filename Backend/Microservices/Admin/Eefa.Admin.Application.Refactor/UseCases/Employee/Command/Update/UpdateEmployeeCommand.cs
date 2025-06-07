using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;

public class UpdateEmployeeCommand : IRequest<ServiceResult<EmployeeModel>>, IMapFrom<Employee>
{
    public int Id { get; set; }
    public int UnitPositionId { get; set; } = default!;
    public string EmployeeCode { get; set; } = default!;
    public DateTime EmploymentDate { get; set; } = default!;
    public bool Floating { get; set; } = default!;
    public DateTime? LeaveDate { get; set; }


    public void Mapping(Profile profile)
    {
        profile.CreateMap<UpdateEmployeeCommand, Employee>()
            .IgnoreAllNonExisting();
    }
}

public class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand, ServiceResult<EmployeeModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public UpdateEmployeeCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<EmployeeModel>> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
    {
        Employee entity = await _unitOfWork.Employees.GetByIdAsync(request.Id);

        _mapper.Map(request, entity);

        _unitOfWork.Employees.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success(_mapper.Map<EmployeeModel>(entity));
    }
}