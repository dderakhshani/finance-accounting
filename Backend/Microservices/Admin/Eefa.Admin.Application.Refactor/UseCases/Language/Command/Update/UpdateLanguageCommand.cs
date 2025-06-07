using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

public class UpdateLanguageCommand : IRequest<ServiceResult<LanguageModel>>, IMapFrom<Language>
{
    public int Id { get; set; }
    public string Title { get; set; } = default!;
    public string Culture { get; set; } = default!;
    public string? SeoCode { get; set; }
    public string? FlagImageUrl { get; set; }
    public int DirectionBaseId { get; set; } = default!;
    public int DefaultCurrencyBaseId { get; set; } = default!;


    public void Mapping(Profile profile)
    {
        profile.CreateMap<UpdateLanguageCommand, Language>()
            .IgnoreAllNonExisting();
    }
}


public class UpdateLanguageCommandHandler : IRequestHandler<UpdateLanguageCommand, ServiceResult<LanguageModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateLanguageCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<LanguageModel>> Handle(UpdateLanguageCommand request, CancellationToken cancellationToken)
    {
        Language entity = await _unitOfWork.Languages.GetByIdAsync(request.Id);

        _mapper.Map(request, entity);

        _unitOfWork.Languages.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success(_mapper.Map<LanguageModel>(entity));
    }
}