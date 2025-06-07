using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Library.Common;
using Library.Interfaces;
using Library.Mappings;
using Library.Models;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Admin.Application.CommandQueries.PersonBankAccounts.Commands.Delete
{
    public class DeletePersonBankAccountCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<Data.Databases.Entities.PersonBankAccount>, ICommand
    {
        public int Id { get; set; }
    
        public void Mapping(Profile profile)
        {
            profile.CreateMap<DeletePersonBankAccountCommand, Data.Databases.Entities.PersonBankAccount>();
        }
    }

    public class DeletePersonBankAccountCommandHandler : IRequestHandler<DeletePersonBankAccountCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public DeletePersonBankAccountCommandHandler(IRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<ServiceResult> Handle(DeletePersonBankAccountCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Data.Databases.Entities.PersonBankAccount>(c =>
                    c.ObjectId(request.Id))
                .FirstOrDefaultAsync(cancellationToken);

            var deletedEntity = _repository.Delete(entity);
            return await request.Save(_repository, cancellationToken);
        }
    }
}
