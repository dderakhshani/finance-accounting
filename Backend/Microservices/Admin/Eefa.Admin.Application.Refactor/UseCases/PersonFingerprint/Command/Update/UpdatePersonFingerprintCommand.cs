using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;

public class UpdatePersonFingerprintCommand : IRequest<ServiceResult<PersonFingerprintModel>>, IMapFrom<PersonFingerprint>
{
    public int Id { get; set; }
    public int PersonId { get; set; } = default!;
    public int FingerBaseId { get; set; } = default!;
    public IFormFile FingerPrintPhoto { get; set; }
    public string FingerprintTemplate { get; set; } = default!;


    public void Mapping(Profile profile)
    {
        profile.CreateMap<UpdatePersonFingerprintCommand, PersonFingerprint>()
            .IgnoreAllNonExisting();
    }
}


public class UpdatePersonFingerprintCommandHandler : IRequestHandler<UpdatePersonFingerprintCommand, ServiceResult<PersonFingerprintModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IUpLoader _upLoader;

    public UpdatePersonFingerprintCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IUpLoader upLoader)
    {
        _mapper = mapper;
        _upLoader = upLoader;
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<PersonFingerprintModel>> Handle(UpdatePersonFingerprintCommand request, CancellationToken cancellationToken)
    {
        PersonFingerprint entity = await _unitOfWork.PersonFingerprints.GetByIdAsync(request.Id);

        if (request.FingerPrintPhoto != null)
        {
            var fingerPrintPhotoUrl = await _upLoader.UpLoadAsync(request.FingerPrintPhoto,
                Guid.NewGuid().ToString(),
                CustomPath.PersonProfile, cancellationToken);

            entity.FingerPrintPhotoURL = fingerPrintPhotoUrl.ReletivePath;
        }

        entity.FingerBaseId = request.FingerBaseId;
        entity.FingerPrintPhotoURL = request.FingerprintTemplate;

        _unitOfWork.PersonFingerprints.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success(_mapper.Map<PersonFingerprintModel>(entity));
    }
}