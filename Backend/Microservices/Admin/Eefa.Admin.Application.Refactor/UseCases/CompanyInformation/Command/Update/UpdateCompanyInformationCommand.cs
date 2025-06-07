using System;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

public class UpdateCompanyInformationCommand : IRequest<ServiceResult<CompanyInformationModel>>, IMapFrom<CompanyInformation>
{
    public int Id { get; set; }
    public string Title { get; set; } = default!;
    public string UniqueName { get; set; } = default!;
    public DateTime? ExpireDate { get; set; }
    public int MaxNumOfUsers { get; set; } = default!;
    public string? Logo { get; set; }


    public void Mapping(Profile profile)
    {
        profile.CreateMap<UpdateCompanyInformationCommand, CompanyInformation>()
            .IgnoreAllNonExisting();
    }
}

public class UpdateCompanyInformationCommandHandler : IRequestHandler<UpdateCompanyInformationCommand, ServiceResult<CompanyInformationModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateCompanyInformationCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<CompanyInformationModel>> Handle(UpdateCompanyInformationCommand request, CancellationToken cancellationToken)
    {
        CompanyInformation entity = await _unitOfWork.CompanyInformations
                                          .GetByIdAsync(request.Id);

        _mapper.Map(request, entity);

        _unitOfWork.CompanyInformations.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success(_mapper.Map<CompanyInformationModel>(entity));
    }
}
