using System;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

public class CreateYearCommand : IRequest<ServiceResult<YearModel>>, IMapFrom<CreateYearCommand>
{
    public int YearName { get; set; }
    public DateTime FirstDate { get; set; }
    public DateTime LastDate { get; set; }
    public bool? IsEditable { get; set; }
    public bool IsCalculable { get; set; }
    public DateTime? LastEditableDate { get; set; }
    public bool IsCurrentYear { get; set; } = default!;
    public int CompanyId { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreateYearCommand, Year>()
            .IgnoreAllNonExisting();
    }
}

public class CreateYearCommandHandler : IRequestHandler<CreateYearCommand, ServiceResult<YearModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public CreateYearCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }


    public async Task<ServiceResult<YearModel>> Handle(CreateYearCommand request, CancellationToken cancellationToken)
    {
        if (request.IsCurrentYear)
        {
            var entities = await _unitOfWork.Years.GetListAsync();
            foreach (var year in entities)
            {
                year.IsCurrentYear = false;
                _unitOfWork.Years.Update(year);
            }
        }

        Year entity = _mapper.Map<Year>(request);

        _unitOfWork.Years.Add(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success(_mapper.Map<YearModel>(entity));
    }
}