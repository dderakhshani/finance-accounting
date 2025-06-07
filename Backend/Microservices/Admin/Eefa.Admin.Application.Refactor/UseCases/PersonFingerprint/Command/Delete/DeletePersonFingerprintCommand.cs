using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;

public class DeletePersonFingerprintCommand : IRequest<ServiceResult<PersonFingerprintModel>>
{
    public int Id { get; set; }
}

public class DeletePersonFingerprintCommandHandler : IRequestHandler<DeletePersonFingerprintCommand, ServiceResult<PersonFingerprintModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DeletePersonFingerprintCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<PersonFingerprintModel>> Handle(DeletePersonFingerprintCommand request, CancellationToken cancellationToken)
    {
        PersonFingerprint entity = await _unitOfWork.PersonFingerprints.GetByIdAsync(request.Id);

        _unitOfWork.PersonFingerprints.Delete(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success(_mapper.Map<PersonFingerprintModel>(entity));
    }
}