using AutoMapper;
using Eefa.Admin.Application.CommandQueries.PersonPhones.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Library.Common;
using Library.Interfaces;
using Library.Mappings;
using Library.Models;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Admin.Application.CommandQueries.PersonPhones.Commands.Update
{
    public class UpdatePersonPhoneCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<Data.Databases.Entities.PersonPhone>, ICommand
    {
        public int Id { get; set; }
        public int PersonId { get; set; } = default!;
        public int PhoneTypeBaseId { get; set; } = default!;
        public string PhoneNumber { get; set; }
        public string? Description { get; set; }
        public bool IsDefault { get; set; } = default!;
        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdatePersonPhoneCommand, Data.Databases.Entities.PersonPhone>()
                .IgnoreAllNonExisting();
        }
    }

    public class UpdatePersonPhoneCommandHandler : IRequestHandler<UpdatePersonPhoneCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public UpdatePersonPhoneCommandHandler(IRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<ServiceResult> Handle(UpdatePersonPhoneCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Data.Databases.Entities.PersonPhone>(c =>
                    c.ObjectId(request.Id))
                .FirstOrDefaultAsync(cancellationToken);


            _mapper.Map(request, entity);
            var updateEntity = _repository.Update(entity);
            return await request.Save<Data.Databases.Entities.PersonPhone, PersonPhoneModel>(_repository, _mapper, updateEntity.Entity, cancellationToken);

        }
    }
}
