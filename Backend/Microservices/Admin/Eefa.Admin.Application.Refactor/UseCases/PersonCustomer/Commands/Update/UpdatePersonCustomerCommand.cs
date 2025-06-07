using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

public class UpdatePersonCustomerCommand : IRequest<ServiceResult<PersonCustomerModel>>,
    IMapFrom<Customer>
{
    public int Id { get; set; }
    public int PersonId { get; set; } = default!;
    public int CustomerTypeBaseId { get; set; } = default!;
    public string CustomerCode { get; set; } = default!;
    public int CurentExpertId { get; set; } = default!;
    public string? EconomicCode { get; set; }
    public string? Description { get; set; }


    public void Mapping(Profile profile)
    {
        profile.CreateMap<UpdatePersonCustomerCommand, Customer>()
            .IgnoreAllNonExisting();
    }
}

public class UpdatePersonCustomerCommandHandler : IRequestHandler<UpdatePersonCustomerCommand, ServiceResult<PersonCustomerModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdatePersonCustomerCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ServiceResult<PersonCustomerModel>> Handle(UpdatePersonCustomerCommand request,
        CancellationToken cancellationToken)
    {
        Customer entity = await _unitOfWork.Customers.GetByIdAsync(request.Id);

        _mapper.Map(request, entity);

        _unitOfWork.Customers.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success(_mapper.Map<PersonCustomerModel>(entity));
    }
}