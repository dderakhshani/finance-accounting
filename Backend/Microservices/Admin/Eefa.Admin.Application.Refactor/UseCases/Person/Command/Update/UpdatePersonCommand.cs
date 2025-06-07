using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class UpdatePersonCommand : IRequest<ServiceResult<PersonModel>>, IMapFrom<Person>
{
    public int Id { get; set; }
    public string FirstName { get; set; } = ""!;
    public string LastName { get; set; } = default!;
    public string? FatherName { get; set; } = default!;
    public string NationalNumber { get; set; } = default!;
    public string EconomicCode { get; set; } = default!;
    public string? IdentityNumber { get; set; }
    public string? InsuranceNumber { get; set; }
    //  public IList<PhoneNumber> Mobiles { get; set; }
    public string? Email { get; set; }
    //public int? AccountReferenceId { get; set; }
    public DateTime? BirthDate { get; set; }
    public int? BirthPlaceCountryDivisionId { get; set; }
    public int GenderBaseId { get; set; } = default!;
    public int? LegalBaseId { get; set; }
    public int? GovernmentalBaseId { get; set; }
    public bool TaxIncluded { get; set; } = default!;
    public string ProfileImageReletiveAddress { get; set; }
    public string SignatureImageReletiveAddress { get; set; }


    public void Mapping(Profile profile)
    {
        profile.CreateMap<UpdatePersonCommand, Person>()
            .IgnoreAllNonExisting();
    }
}

public class UpdatePersonCommandHandler : IRequestHandler<UpdatePersonCommand, ServiceResult<PersonModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public UpdatePersonCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<PersonModel>> Handle(UpdatePersonCommand request, CancellationToken cancellationToken)
    {
        Person entity = await _unitOfWork.Persons.GetByIdAsync(request.Id,
                                                  x => x.Include(y => y.AccountReference));

        _mapper.Map(request, entity);
        entity.AccountReference.Title = ((entity.FirstName ?? "") + " " + entity.LastName).Trim();

        _unitOfWork.Persons.Update(entity);
        _unitOfWork.AccountReferences.Update(entity.AccountReference);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success(_mapper.Map<PersonModel>(entity));
    }
}