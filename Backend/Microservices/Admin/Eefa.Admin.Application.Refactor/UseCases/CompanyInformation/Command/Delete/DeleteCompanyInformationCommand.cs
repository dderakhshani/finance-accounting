using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

public class DeleteCompanyInformationCommand : IRequest<ServiceResult<int>>
{
    public int Id { get; set; }
}

public class DeleteCompanyInformationCommandHandler : IRequestHandler<DeleteCompanyInformationCommand, ServiceResult<int>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DeleteCompanyInformationCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<int>> Handle(DeleteCompanyInformationCommand request, CancellationToken cancellationToken)
    {
        CompanyInformation entity = await _unitOfWork.CompanyInformations.GetByIdAsync(request.Id);

        _unitOfWork.CompanyInformations.Delete(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success(entity.Id);
    }
}