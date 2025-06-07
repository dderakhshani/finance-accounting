using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;

public class CreatePersonFingerprintCommand : IRequest<ServiceResult<PersonFingerprintModel>>, IMapFrom<CreatePersonFingerprintCommand>
{
    public int PersonId { get; set; } = default!;
    public IFormFile FingerPrintPhoto { get; set; }
    public int FingerBaseId { get; set; } = default!;
    public string FingerPrintTemplate { get; set; } = default!;


    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreatePersonFingerprintCommand, PersonFingerprint>()
            .IgnoreAllNonExisting();
    }
}

public class CreatePersonFingerprintCommandHandler : IRequestHandler<CreatePersonFingerprintCommand, ServiceResult<PersonFingerprintModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IUpLoader _upLoader;

    public CreatePersonFingerprintCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IUpLoader upLoader)
    {
        _mapper = mapper;
        _upLoader = upLoader;
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<PersonFingerprintModel>> Handle(CreatePersonFingerprintCommand request, CancellationToken cancellationToken)
    {
        PersonFingerprint fingerPrint = _mapper.Map<PersonFingerprint>(request);

        if (request.FingerPrintPhoto is not null)
        {
            var fingerPrintPhotoUrl = await _upLoader.UpLoadAsync(request.FingerPrintPhoto,
                Guid.NewGuid().ToString(),
                CustomPath.Temp, cancellationToken);

            fingerPrint.FingerPrintPhotoURL = fingerPrintPhotoUrl.ReletivePath;
        }

        _unitOfWork.PersonFingerprints.Add(fingerPrint);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success(_mapper.Map<PersonFingerprintModel>(fingerPrint));
    }
}