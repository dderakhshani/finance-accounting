using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

public class GetMoadianInvoiceHeaderQuery : IRequest<ServiceResult<MoadianInvoiceHeaderDetailedModel>>
{
    public int Id { get; set; }
}
public class GetMoadianInvoiceHeaderQueryHandler : IRequestHandler<GetMoadianInvoiceHeaderQuery, ServiceResult<MoadianInvoiceHeaderDetailedModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetMoadianInvoiceHeaderQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork= unitOfWork;
    }

    public async Task<ServiceResult<MoadianInvoiceHeaderDetailedModel>> Handle(GetMoadianInvoiceHeaderQuery request, CancellationToken cancellationToken)
    {
        Specification<MoadianInvoiceHeader> specification = new Specification<MoadianInvoiceHeader>();
        specification.ApplicationConditions.Add(x => x.Id == request.Id);
        specification.Includes = y =>
            y.Include(x => x.Person)
             .Include(x => x.AccountReference)
             .Include(x => x.Customer);

        return ServiceResult.Success(await _unitOfWork.MoadianInvoiceHeaders
            .GetProjectedAsync<MoadianInvoiceHeaderDetailedModel>(specification));
    }
}