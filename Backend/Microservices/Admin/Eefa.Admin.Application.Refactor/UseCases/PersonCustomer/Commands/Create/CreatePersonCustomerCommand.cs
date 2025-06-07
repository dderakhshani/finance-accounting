using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;

public class CreatePersonCustomerCommand : IRequest<ServiceResult<PersonCustomerModel>>, IMapFrom<Customer>
{
    public int PersonId { get; set; } = default!;
    public int CustomerTypeBaseId { get; set; } = default!;
    public string CustomerCode { get; set; } = default!;
    public int CurentExpertId { get; set; } = default!;
    public string? EconomicCode { get; set; }
    public string? Description { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreatePersonCustomerCommand, Customer>();
    }
}

public class CreatePersonCustomerCommandHandler : IRequestHandler<CreatePersonCustomerCommand, ServiceResult<PersonCustomerModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreatePersonCustomerCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<ServiceResult<PersonCustomerModel>> Handle(CreatePersonCustomerCommand request, CancellationToken cancellationToken)
    {
        Customer entity = _mapper.Map<Customer>(request);

        _unitOfWork.Customers.Add(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success(_mapper.Map<PersonCustomerModel>(entity));
    }
}