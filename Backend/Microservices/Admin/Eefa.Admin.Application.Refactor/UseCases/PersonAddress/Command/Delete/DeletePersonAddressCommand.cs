using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;

public class DeletePersonAddressCommand : IRequest<ServiceResult<PersonAddressModel>>
{
    public int Id { get; set; }
}

public class DeletePersonAddressCommandHandler : IRequestHandler<DeletePersonAddressCommand, ServiceResult<PersonAddressModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DeletePersonAddressCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<PersonAddressModel>> Handle(DeletePersonAddressCommand request, CancellationToken cancellationToken)
    {
        PersonAddress entity = await _unitOfWork.PersonsAddress.GetByIdAsync(request.Id);

        _unitOfWork.PersonsAddress.Delete(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success(_mapper.Map<PersonAddressModel>(entity));
    }

}