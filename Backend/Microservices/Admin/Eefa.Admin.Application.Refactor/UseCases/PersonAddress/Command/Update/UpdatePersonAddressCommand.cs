using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Newtonsoft.Json;

public class UpdatePersonAddressCommand : IRequest<ServiceResult<PersonAddressModel>>, IMapFrom<PersonAddress>
{
    public int Id { get; set; }
    public int PersonId { get; set; } = default!;
    public int TypeBaseId { get; set; }
    public string? Address { get; set; }
    public int? CountryDivisionId { get; set; }
    public IList<PhoneNumber> Mobiles { get; set; }
    public string? PostalCode { get; set; }


    public void Mapping(Profile profile)
    {
        profile.CreateMap<UpdatePersonAddressCommand, PersonAddress>()
            .IgnoreAllNonExisting();
    }
}


public class UpdatePersonAddressCommandHandler : IRequestHandler<UpdatePersonAddressCommand, ServiceResult<PersonAddressModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdatePersonAddressCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<PersonAddressModel>> Handle(UpdatePersonAddressCommand request, CancellationToken cancellationToken)
    {
        PersonAddress entity = await _unitOfWork.PersonsAddress.GetByIdAsync(request.Id);

        entity.Address = request.Address;
        entity.TypeBaseId = request.TypeBaseId;
        entity.CountryDivisionId = request.CountryDivisionId;
        entity.PostalCode = request.PostalCode;
        if (request.Mobiles != null)
            entity.TelephoneJson = JsonConvert.SerializeObject(request.Mobiles);

        _unitOfWork.PersonsAddress.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success(_mapper.Map<PersonAddressModel>(entity));
    }
}