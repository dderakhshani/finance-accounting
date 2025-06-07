using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

public class DeletePersonCustomerCommand : IRequest<ServiceResult<PersonCustomerModel>>, IMapFrom<Customer>
{
    public int Id { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<DeletePersonCustomerCommand, Customer>();
    }
}

public class DeletePersonCustomerCommandHandler : IRequestHandler<DeletePersonCustomerCommand, ServiceResult<PersonCustomerModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DeletePersonCustomerCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<ServiceResult<PersonCustomerModel>> Handle(DeletePersonCustomerCommand request, CancellationToken cancellationToken)
    {
        Customer entity = await _unitOfWork.Customers.GetByIdAsync(request.Id);

        _unitOfWork.Customers.Delete(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success(_mapper.Map<PersonCustomerModel>(entity));
    }
}