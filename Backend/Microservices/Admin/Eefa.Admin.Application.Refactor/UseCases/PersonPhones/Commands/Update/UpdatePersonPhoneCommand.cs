using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

public class UpdatePersonPhoneCommand : IRequest<ServiceResult<PersonPhoneModel>>, IMapFrom<PersonPhone>
{
    public int Id { get; set; }
    public int PersonId { get; set; } = default!;
    public int PhoneTypeBaseId { get; set; } = default!;
    public string PhoneNumber { get; set; }
    public string? Description { get; set; }
    public bool IsDefault { get; set; } = default!;


    public void Mapping(Profile profile)
    {
        profile.CreateMap<UpdatePersonPhoneCommand, PersonPhone>()
            .IgnoreAllNonExisting();
    }
}

public class UpdatePersonPhoneCommandHandler : IRequestHandler<UpdatePersonPhoneCommand, ServiceResult<PersonPhoneModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdatePersonPhoneCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<ServiceResult<PersonPhoneModel>> Handle(UpdatePersonPhoneCommand request, CancellationToken cancellationToken)
    {
        PersonPhone entity = await _unitOfWork.PersonPhones.GetByIdAsync(request.Id);

        _mapper.Map(request, entity);

        _unitOfWork.PersonPhones.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success(_mapper.Map<PersonPhoneModel>(entity));
    }
}