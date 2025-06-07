using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

public class CreateLanguageCommand : IRequest<ServiceResult<Language>>, IMapFrom<CreateLanguageCommand>
{
    public string Title { get; set; } = default!;
    public string Culture { get; set; } = default!;
    public string? SeoCode { get; set; }
    public string? FlagImageUrl { get; set; }
    public int DirectionBaseId { get; set; } = default!;
    public int DefaultCurrencyBaseId { get; set; } = default!;


    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreateLanguageCommand, Language>()
            .IgnoreAllNonExisting();
    }
}
public class CreateLanguageCommandHandler : IRequestHandler<CreateLanguageCommand, ServiceResult<Language>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IUpLoader _upLoader;

    public CreateLanguageCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IUpLoader upLoader)
    {
        _mapper = mapper;
        _upLoader = upLoader;
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<Language>> Handle(CreateLanguageCommand request, CancellationToken cancellationToken)
    {
        Language entity = _mapper.Map<Language>(request);

        _unitOfWork.Languages.Add(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        if (!string.IsNullOrEmpty(request.FlagImageUrl))
        {
            var profileUrl = await _upLoader.UpLoadAsync(request.FlagImageUrl,
                CustomPath.FlagImage, cancellationToken);

            entity.FlagImageUrl = profileUrl.ReletivePath;

            _unitOfWork.Languages.Update(entity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        return ServiceResult.Success(entity);
    }
}