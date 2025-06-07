using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Eefa.Admin.Application.CommandQueries.PersonBankAccounts.Models;
using Library.Common;
using Library.Interfaces;
using Library.Mappings;
using Library.Models;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Admin.Application.CommandQueries.PersonBankAccounts.Commands.Update
{
    public class UpdatePersonBankAccountCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<Data.Databases.Entities.PersonBankAccount>, ICommand
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public int? BankId { get; set; }
        public string? BankBranchName { get; set; }
        public int AccountTypeBaseId { get; set; }
        public string AccountNumber { get; set; }
        public string? Description { get; set; }
        public bool IsDefault { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdatePersonBankAccountCommand, Data.Databases.Entities.PersonBankAccount>()
                .IgnoreAllNonExisting();
        }
    }

    public class UpdatePersonBankAccountCommandHandler : IRequestHandler<UpdatePersonBankAccountCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public UpdatePersonBankAccountCommandHandler(IRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<ServiceResult> Handle(UpdatePersonBankAccountCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Data.Databases.Entities.PersonBankAccount>(c =>
                    c.ObjectId(request.Id))
                .FirstOrDefaultAsync(cancellationToken);


            _mapper.Map(request, entity);
            var updateEntity = _repository.Update(entity);
            return await request.Save<Data.Databases.Entities.PersonBankAccount, PersonBankAccountModel>(_repository, _mapper, updateEntity.Entity, cancellationToken);

        }
    }
}
