using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;

public class CreateVoucherAttachmentCommand : IRequest<ServiceResult<ICollection<int>>>, IMapFrom<CreateVoucherAttachmentCommand>
{
    public int VoucherHeadId { get; set; }
    public ICollection<int> AttachmentIds { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreateVoucherAttachmentCommand, VoucherAttachment>()
            .IgnoreAllNonExisting();
    }
}

public class CreateVoucherAttachmentCommandHandler : IRequestHandler<CreateVoucherAttachmentCommand, ServiceResult<ICollection<int>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateVoucherAttachmentCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork= unitOfWork;
    }

    public async Task<ServiceResult<ICollection<int>>> Handle(CreateVoucherAttachmentCommand request, CancellationToken cancellationToken)
    {
        foreach (var attachment in request.AttachmentIds)
        {
            _unitOfWork.VoucherAttachments.Add(new VoucherAttachment()
            { VoucherHeadId = request.VoucherHeadId, AttachmentId = attachment });
        }
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success(request.AttachmentIds);
    }
}