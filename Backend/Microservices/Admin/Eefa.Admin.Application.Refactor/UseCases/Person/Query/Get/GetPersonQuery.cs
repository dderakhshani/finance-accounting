using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class GetPersonQuery : IRequest<ServiceResult<PersonModel>>
{
    public int Id { get; set; }
}

public class GetPersonQueryHandler : IRequestHandler<GetPersonQuery, ServiceResult<PersonModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetPersonQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<PersonModel>> Handle(GetPersonQuery request, CancellationToken cancellationToken)
    {
        Specification<Person> specification = new Specification<Person>();
        specification.ApplicationConditions.Add(x => x.Id == request.Id);
        specification.Includes = x => x.Include(y => y.PersonAddresses)
                                       .Include(y => y.PersonFingerprints)
                                       .Include(y => y.PersonBankAccounts)
                                       .Include(y => y.PersonPhones)
                                       .Include(y => y.Customer);

        return ServiceResult.Success(await _unitOfWork.Persons
                            .GetProjectedByIdAsync<PersonModel>(specification));
    }
}