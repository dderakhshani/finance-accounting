using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Library.Attributes;
using Library.Common;
using Library.Interfaces;
using Library.Mappings;
using Library.Models;
using MediatR;
using Newtonsoft.Json;

namespace Eefa.Admin.Application.CommandQueries.PersonAddress.Command.Create
{
    public class CreatePersonAddressCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<CreatePersonAddressCommand>, ICommand
    {
        [SwaggerExclude]
        public Data.Databases.Entities.Person Person { get; set; }
        /// <summary>
        /// کد والد
        /// </summary>
        public int PersonId { get; set; } = default!;

        public int TypeBaseId { get; set; }
        /// <summary>
        /// آدرس
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// کد شهرستان
        /// </summary>
        public int? CountryDivisionId { get; set; }

        /// <summary>
        /// تلفن
        /// </summary>

        public ICollection<PhoneNumber> Mobiles { get; set; }

        /// <summary>
        /// کد پستی
        /// </summary>
        public string? PostalCode { get; set; }


        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreatePersonAddressCommand, Data.Databases.Entities.PersonAddress>()
                .IgnoreAllNonExisting();
        }
    }

    public class CreatePersonAddressCommandHandler : IRequestHandler<CreatePersonAddressCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public CreatePersonAddressCommandHandler(IRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }


        public async Task<ServiceResult> Handle(CreatePersonAddressCommand request, CancellationToken cancellationToken)
        {
            var personAddress = _mapper.Map<Data.Databases.Entities.PersonAddress>(request);
            if(request.Mobiles != null)
                personAddress.TelephoneJson = JsonConvert.SerializeObject(request.Mobiles);
            var entity = _repository.Insert(personAddress);

            return await request.Save(_repository, entity.Entity, cancellationToken);

        }
    }
}
