using System;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

public class CreateCompanyInformationCommand : IRequest<ServiceResult<CompanyInformationModel>>, IMapFrom<CreateCompanyInformationCommand>
{
    public string Title { get; set; } = default!;
    public string UniqueName { get; set; } = default!;
    public DateTime? ExpireDate { get; set; }
    public int MaxNumOfUsers { get; set; } = default!;
    public string? Logo { get; set; }


    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreateCompanyInformationCommand, CompanyInformation>()
            .IgnoreAllNonExisting();
    }
}

public class CreateCompanyInformationCommandHandler : IRequestHandler<CreateCompanyInformationCommand, ServiceResult<CompanyInformationModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateCompanyInformationCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<CompanyInformationModel>> Handle(CreateCompanyInformationCommand request, CancellationToken cancellationToken)
    {
        CompanyInformation entity = _mapper.Map<CompanyInformation>(request);

        _unitOfWork.CompanyInformations.Add(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return ServiceResult.Success(_mapper.Map<CompanyInformationModel>(entity));
    }
}