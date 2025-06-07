using System;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

public class UpdateYearCommand : IRequest<ServiceResult<YearModel>>, IMapFrom<Year>
{
    public int Id { get; set; }
    public int YearName { get; set; }
    public DateTime FirstDate { get; set; }
    public DateTime LastDate { get; set; }
    public bool? IsEditable { get; set; }
    public bool IsCalculable { get; set; }
    public DateTime? LastEditableDate { get; set; }
    public bool IsCurrentYear { get; set; } = default!;
    public void Mapping(Profile profile)
    {
        profile.CreateMap<UpdateYearCommand, Year>()
            .IgnoreAllNonExisting();
    }
}


public class UpdateYearCommandHandler : IRequestHandler<UpdateYearCommand, ServiceResult<YearModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateYearCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<YearModel>> Handle(UpdateYearCommand request, CancellationToken cancellationToken)
    {
        Year entity = await _unitOfWork.Years.GetByIdAsync(request.Id);

        if (request.IsCurrentYear)
        {
            if (entity.IsCurrentYear == false)
            {
                var entities = await _unitOfWork.Years.GetListAsync();
                foreach (var year in entities)
                {
                    year.IsCurrentYear = false;
                    _unitOfWork.Years.Update(year);
                }
            }
        }

        entity.FirstDate = request.FirstDate;
        entity.LastEditableDate = request.LastEditableDate;
        entity.LastDate = request.LastDate;
        entity.IsEditable = request.IsEditable;
        entity.IsCalculable = request.IsCalculable;
        entity.IsCurrentYear = request.IsCurrentYear;
        entity.YearName = request.YearName;

        _unitOfWork.Years.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success(_mapper.Map<YearModel>(entity));
    }
}