using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;

public class UpdateAttachmentCommand : IRequest<ServiceResult<AttachmentModel>>, IMapFrom<Attachment>
{
    public int Id { get; set; }
    public int LanguageId { get; set; } = default!;
    public int TypeBaseId { get; set; } = default!;
    public string Extention { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public string? KeyWords { get; set; }
    public string Url { get; set; } = default!;
    public bool IsUsed { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<UpdateAttachmentCommand, Attachment>()
            .IgnoreAllNonExisting();
    }
}

public class UpdateAttachmentCommandHandler : IRequestHandler<UpdateAttachmentCommand, ServiceResult<AttachmentModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IUpLoader _upLoader;
    public UpdateAttachmentCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IUpLoader upLoader)
    {
        _mapper = mapper;
        _upLoader = upLoader;
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<AttachmentModel>> Handle(UpdateAttachmentCommand request, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.Attachments.GetByIdAsync(request.Id);

        var profileUrl = await _upLoader.UpLoadAsync(request.Url,
            CustomPath.Attachment, cancellationToken);

        entity.Url = profileUrl.ReletivePath;
        entity.Title = request.Title;
        entity.LanguageId = request.LanguageId;
        entity.Description = request.Description;
        entity.Extention = request.Extention;
        entity.KeyWords = request.KeyWords;
        entity.TypeBaseId = request.TypeBaseId;
        entity.IsUsed = request.IsUsed;

        _unitOfWork.Attachments.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success(_mapper.Map<AttachmentModel>(entity));
    }
}