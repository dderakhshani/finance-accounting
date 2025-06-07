using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

public class CreatePersonPhoneCommand : IRequest<ServiceResult<PersonPhoneModel>>, IMapFrom<PersonPhone>
{
    public int PersonId { get; set; } = default!;
    public int PhoneTypeBaseId { get; set; } = default!;
    public string PhoneNumber { get; set; }
    public string? Description { get; set; }
    public bool IsDefault { get; set; } = default!;


    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreatePersonPhoneCommand, PersonPhone>();
    }
}

public class CreatePersonPhoneCommandHandler : IRequestHandler<CreatePersonPhoneCommand, ServiceResult<PersonPhoneModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreatePersonPhoneCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<ServiceResult<PersonPhoneModel>> Handle(CreatePersonPhoneCommand request, CancellationToken cancellationToken)
    {
        PersonPhone entity = _mapper.Map<PersonPhone>(request);

        _unitOfWork.PersonPhones.Add(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success(_mapper.Map<PersonPhoneModel>(entity));
    }
}