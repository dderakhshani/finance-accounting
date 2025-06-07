using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class UpdateInvoicesStatusByIdsCommand : IRequest<ServiceResult<bool>>, IMapFrom<UpdateInvoicesStatusByIdsCommand>
{
    public List<int> Ids { get; set; }
    public string Status { get; set; } = default!;

    public List<UpdateMoadianInvoiceDetailCommand> MoadianInvoiceDetails { get; set; } = default!;
    public void Mapping(Profile profile)
    {
        profile.CreateMap<UpdateInvoicesStatusByIdsCommand, MoadianInvoiceHeader>()
            .IgnoreAllNonExisting();
    }
}

public class UpdateInvoicesStatusByIdsCommandHandler : IRequestHandler<UpdateInvoicesStatusByIdsCommand, ServiceResult<bool>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public UpdateInvoicesStatusByIdsCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork= unitOfWork;
        _mapper = mapper;
    }
    public async Task<ServiceResult<bool>> Handle(UpdateInvoicesStatusByIdsCommand request, CancellationToken cancellationToken)
    {
        var entities = await _unitOfWork.MoadianInvoiceHeaders
                                      .GetListAsync(x => request.Ids.Contains(x.Id));

        foreach (var entity in entities)
        {
            entity.Status = request.Status;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success(true);
    }
}