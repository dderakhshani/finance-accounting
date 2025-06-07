using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Newtonsoft.Json;

public class CreatePersonAddressCommand : IRequest<ServiceResult<PersonAddressModel>>, IMapFrom<CreatePersonAddressCommand>
{
    [SwaggerExclude]
    public Person Person { get; set; }
    public int PersonId { get; set; } = default!;
    public int TypeBaseId { get; set; }
    public string? Address { get; set; }
    public int? CountryDivisionId { get; set; }
    public ICollection<PhoneNumber> Mobiles { get; set; }
    public string? PostalCode { get; set; }


    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreatePersonAddressCommand, PersonAddress>()
            .IgnoreAllNonExisting();
    }
}

public class CreatePersonAddressCommandHandler : IRequestHandler<CreatePersonAddressCommand, ServiceResult<PersonAddressModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreatePersonAddressCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<PersonAddressModel>> Handle(CreatePersonAddressCommand request, CancellationToken cancellationToken)
    {
        PersonAddress personAddress = _mapper.Map<PersonAddress>(request);

        if (request.Mobiles != null)
            personAddress.TelephoneJson = JsonConvert.SerializeObject(request.Mobiles);

        _unitOfWork.PersonsAddress.Add(personAddress);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success(_mapper.Map<PersonAddressModel>(personAddress));
    }
}