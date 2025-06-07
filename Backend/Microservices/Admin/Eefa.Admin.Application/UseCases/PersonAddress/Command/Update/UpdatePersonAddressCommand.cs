using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Admin.Application.CommandQueries.PersonAddress.Model;
using Library.Common;
using Library.Interfaces;
using Library.Mappings;
using Library.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;


namespace Eefa.Admin.Application.CommandQueries.PersonAddress.Command.Update
{
    public class UpdatePersonAddressCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<Data.Databases.Entities.PersonAddress>, ICommand
    {
        public int Id { get; set; }
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
        public IList<PhoneNumber> Mobiles { get; set; }


        /// <summary>
        /// کد پستی
        /// </summary>
        public string? PostalCode { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdatePersonAddressCommand, Data.Databases.Entities.PersonAddress>()
                .IgnoreAllNonExisting();
        }
    }


    public class UpdatePersonAddressCommandHandler : IRequestHandler<UpdatePersonAddressCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public UpdatePersonAddressCommandHandler(IRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(UpdatePersonAddressCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Data.Databases.Entities.PersonAddress>(c =>
                    c.ObjectId(request.Id))
                .FirstOrDefaultAsync(cancellationToken);

            entity.Address = request.Address;
            entity.TypeBaseId = request.TypeBaseId;
            entity.CountryDivisionId = request.CountryDivisionId;
            entity.PostalCode = request.PostalCode;
            if (request.Mobiles != null)
                entity.TelephoneJson = JsonConvert.SerializeObject(request.Mobiles);

            var updateEntity = _repository.Update(entity);

            return await request.Save<Data.Databases.Entities.PersonAddress,PersonAddressModel>(_repository,_mapper, updateEntity.Entity, cancellationToken);
        }
    }
}
