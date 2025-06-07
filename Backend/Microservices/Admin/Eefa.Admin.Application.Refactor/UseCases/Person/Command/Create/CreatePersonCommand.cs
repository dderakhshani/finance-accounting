using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;

public class CreatePersonCommand : IRequest<ServiceResult<PersonModel>>, IMapFrom<CreatePersonCommand>
{
    public string FirstName { get; set; } = ""!;
    public string LastName { get; set; } = default!;
    public string? FatherName { get; set; } = default!;
    public string NationalNumber { get; set; } = default!;
    public string EconomicCode { get; set; } = default!;

    public string? IdentityNumber { get; set; }
    public string? InsuranceNumber { get; set; }
    public string? Email { get; set; }
    public int? AccountReferenceId { get; set; }
    public string AccountReferenceCode { get; set; }
    public DateTime? BirthDate { get; set; }
    public int? BirthPlaceCountryDivisionId { get; set; }
    public int GenderBaseId { get; set; } = default!;
    public int? LegalBaseId { get; set; }
    public int? GovernmentalBaseId { get; set; }
    public string ProfileImageReletiveAddress { get; set; }
    public bool TaxIncluded { get; set; } = default!;
    public int AccountReferenceGroupId { get; set; }
    public string? AccountReferenceTitle { get; set; }


    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreatePersonCommand, Person>()
            .IgnoreAllNonExisting();
    }
}

public class CreatePersonCommandHandler : IRequestHandler<CreatePersonCommand, ServiceResult<PersonModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public CreatePersonCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<PersonModel>> Handle(CreatePersonCommand request, CancellationToken cancellationToken)
    {
        Person person = _mapper.Map<Person>(request);

        _unitOfWork.Persons.Add(person);

        //var accountReferenceGroup = await _unitOfWork.GetQuery<AccountReferencesGroup>()
        //    .FirstOrDefaultAsync(x => x.Id == request.AccountReferenceGroupId, cancellationToken: cancellationToken);

        var accountReference = new AccountReference()
        {
            Person = person,
            Title = request.AccountReferenceTitle ?? ((person.FirstName ?? "") + " " + person.LastName).Trim(),
            IsActive = true
        };

        if (string.IsNullOrEmpty(request.AccountReferenceCode))
        {
            var accountReferenceGroupCode = await _unitOfWork.AccountReferencesGroups.GetProjectedByIdAsync(request.AccountReferenceGroupId, x => x.Code);

            var spesification = new Specification<AccountReference>();
            spesification.ApplicationConditions.Add(x => x.Code.Length == 6 && x.Code.StartsWith(accountReferenceGroupCode));
            spesification.OrderBy = x => x.OrderBy(y => y.Code);
            var lastAccountReference = await _unitOfWork.AccountReferences.GetAsync(spesification);

            if (lastAccountReference != null && lastAccountReference.Code.EndsWith("9999")) throw new Exception("Maximum code limit has been reached for this accountReference Type");
            if (lastAccountReference == null) lastAccountReference = new AccountReference { Code = accountReferenceGroupCode + "0000" };

            accountReference.Code = (Convert.ToInt32(lastAccountReference.Code) + 1).ToString();
        }
        else
        {
            accountReference.Code = request.AccountReferenceCode;
        }

        _unitOfWork.AccountReferences.Add(accountReference);

        _unitOfWork.AccountReferencesRelReferencesGroups.Add(new AccountReferencesRelReferencesGroup()
        {
            ReferenceGroupId = request.AccountReferenceGroupId,
            Reference = accountReference
        });

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success(_mapper.Map<PersonModel>(person));
    }
}