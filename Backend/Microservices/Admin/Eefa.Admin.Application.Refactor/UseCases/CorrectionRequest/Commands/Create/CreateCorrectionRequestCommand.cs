using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

public class CreateCorrectionRequestCommand : IRequest<ServiceResult<CorrectionRequestModel>>, IMapFrom<CreateCorrectionRequestCommand>
{
    public short Status { get; set; } = default!;
    public int CodeVoucherGroupId { get; set; } = default!;
    public int? AccessPermissionId { get; set; } = default!;
    public int? DocumentId { get; set; }
    public string OldData { get; set; } = default!;
    public int VerifierUserId { get; set; } = default!;
    public string? PayLoad { get; set; }
    public string ApiUrl { get; set; } = default!;
    public string? ViewUrl { get; set; }
    public string? Description { get; set; }


    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreateCorrectionRequestCommand, CorrectionRequest>()
            .IgnoreAllNonExisting();
    }
}

public class CreateCorrectionRequestCommandHandler : IRequestHandler<CreateCorrectionRequestCommand, ServiceResult<CorrectionRequestModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public CreateCorrectionRequestCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<CorrectionRequestModel>> Handle(CreateCorrectionRequestCommand request, CancellationToken cancellationToken)
    {
        var isAlreadyAPendingRequestExist = await _unitOfWork.CorrectionRequests.ExistsAsync(x => x.CodeVoucherGroupId == request.CodeVoucherGroupId && x.DocumentId == request.DocumentId && x.Status == 0);
        if (isAlreadyAPendingRequestExist) throw new Exception("این درخواست تغیرات قبلا ثبت شده است");

        CorrectionRequest entity = _mapper.Map<CorrectionRequest>(request);
        entity.ModifiedAt = new DateTime();
        entity.AccessPermissionId = 1251;
        
        _unitOfWork.CorrectionRequests.Add(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success(_mapper.Map<CorrectionRequestModel>(entity));
    }
}