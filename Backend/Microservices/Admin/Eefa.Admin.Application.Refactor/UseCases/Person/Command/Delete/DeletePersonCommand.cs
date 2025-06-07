using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;

public class DeletePersonCommand : IRequest<ServiceResult<PersonModel>>
{
    public int Id { get; set; }
}

public class DeletePersonCommandHandler : IRequestHandler<DeletePersonCommand, ServiceResult<PersonModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DeletePersonCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<PersonModel>> Handle(DeletePersonCommand request, CancellationToken cancellationToken)
    {
        Person entity = await _unitOfWork.Persons.GetByIdAsync(request.Id);

        _unitOfWork.Persons.Delete(entity);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success(_mapper.Map<PersonModel>(entity));
    }
}