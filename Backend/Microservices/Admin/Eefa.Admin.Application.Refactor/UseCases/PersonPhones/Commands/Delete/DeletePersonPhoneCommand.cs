using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

public class DeletePersonPhoneCommand : IRequest<ServiceResult<PersonPhoneModel>>, IMapFrom<PersonPhone>
{
    public int Id { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<DeletePersonPhoneCommand, PersonPhone>();
    }
}

public class DeletePersonPhoneCommandHandler : IRequestHandler<DeletePersonPhoneCommand, ServiceResult<PersonPhoneModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DeletePersonPhoneCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<ServiceResult<PersonPhoneModel>> Handle(DeletePersonPhoneCommand request, CancellationToken cancellationToken)
    {
        PersonPhone entity = await _unitOfWork.PersonPhones.GetByIdAsync(request.Id);

        _unitOfWork.PersonPhones.Delete(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success(_mapper.Map<PersonPhoneModel>(entity));
    }
}