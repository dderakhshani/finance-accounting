using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class DeleteMoadianInvoiceHeaderCommand : IRequest<ServiceResult<List<MoadianInvoiceHeaderDetailedModel>>>, IMapFrom<DeleteMoadianInvoiceHeaderCommand>
{
    public List<int> InvoiceIds { get; set; }

    public bool IsProduction { get; set; } = false;
    public void Mapping(Profile profile)
    {
        profile.CreateMap<DeleteMoadianInvoiceHeaderCommand, MoadianInvoiceHeader>()
            .IgnoreAllNonExisting();
    }
}

public class DeleteMoadianInvoiceHeaderCommandHandler : IRequestHandler<DeleteMoadianInvoiceHeaderCommand, ServiceResult<List<MoadianInvoiceHeaderDetailedModel>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public DeleteMoadianInvoiceHeaderCommandHandler(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork= unitOfWork;
    }
    public async Task<ServiceResult<List<MoadianInvoiceHeaderDetailedModel>>> Handle(DeleteMoadianInvoiceHeaderCommand request, CancellationToken cancellationToken)
    {
        var entities = await _unitOfWork.MoadianInvoiceHeaders
            .GetListAsync(x => x.IsSandbox == true & (x.TaxId == null || x.TaxId.StartsWith(MoadianConstants.SandboxProtectorId)),
            y => y.Include(a => a.MoadianInvoiceDetails));


        foreach (var entity in entities)
        {
            _unitOfWork.MoadianInvoiceHeaders.Delete(entity);
            foreach (var detail in entity.MoadianInvoiceDetails)
            {
                _unitOfWork.MoadianInvoiceDetails.Delete(detail);
            }
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success(_mapper.Map<List<MoadianInvoiceHeaderDetailedModel>>(entities));
    }
}