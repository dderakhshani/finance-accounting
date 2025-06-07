using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Library.Common;
using Library.Interfaces;
using Library.Mappings;
using Library.Models;
using MediatR;

namespace Eefa.Admin.Application.CommandQueries.PersonBankAccounts.Commands.Create
{
    public class CreatePersonBankAccountCommand : CommandBase,IRequest<ServiceResult>, IMapFrom<Data.Databases.Entities.PersonBankAccount>, ICommand
    {
        public int PersonId { get; set; }
        public int? BankId { get; set; }
        public string? BankBranchName { get; set; }
        public int AccountTypeBaseId { get; set; }
        public string AccountNumber { get; set; }
        public string? Description { get; set; }
        public bool IsDefault { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreatePersonBankAccountCommand,Data.Databases.Entities.PersonBankAccount>();
        }
    }

    public class CreatePersonBankAccountCommandHandler : IRequestHandler<CreatePersonBankAccountCommand,ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public CreatePersonBankAccountCommandHandler(IRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<ServiceResult> Handle(CreatePersonBankAccountCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Data.Databases.Entities.PersonBankAccount>(request);

            _repository.Insert(entity);

            return await request.Save(_repository, entity, cancellationToken);
        }
    }
}
