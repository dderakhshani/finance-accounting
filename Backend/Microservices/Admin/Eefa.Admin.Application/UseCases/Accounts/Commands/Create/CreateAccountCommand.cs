using AutoMapper;
using Eefa.Admin.Application.CommandQueries.Accounts.Models;
using Eefa.Admin.Data.Databases.Entities;
using Library.Common;
using Library.ConfigurationAccessor.Mappings;
using Library.Interfaces;
using Library.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Admin.Application.CommandQueries.Accounts.Commands.Create
{
    public class CreateAccountCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<CreateAccountCommand>, ICommand
    {
        public string Title { get; set; }
        public string Code { get; set; }
        public int Type { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateAccountCommand, Data.Databases.Entities.AccountReference>()
                .IgnoreAllNonExisting();
        }
    }

    public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public CreateAccountCommandHandler(IRepository repository, IMapper mapper, IMediator mediator)
        {
            _mapper = mapper;
            _mediator = mediator;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            var account = new AccountReference
            {
                Title = request.Title,
                IsActive = true
            };

            if (!string.IsNullOrEmpty(request.Code)) account.Code = request.Code;
            else account.Code = await this.GenerateAccountCodeByType(request.Type);



            _repository.Insert(account);
            await _repository.SaveChangesAsync();

            return ServiceResult.Success(_mapper.Map<AccountModel>(account));
        }


        public async Task<string> GenerateAccountCodeByType(int type)
        {
            var accountReferencesType = await _repository.GetQuery<Eefa.Admin.Data.Databases.Entities.BaseValue>().FirstOrDefaultAsync(x => x.Id == type);

            var lastAccountReference = await _repository.GetQuery<AccountReference>()
                .Where(x => x.Code.Length == 6 && x.Code.StartsWith(accountReferencesType.Value))
                .OrderByDescending(x => x.Code)
                .FirstOrDefaultAsync();

            if (lastAccountReference.Code.EndsWith("9999")) throw new Exception("Maximum code limit has been reached for this accountReference Type");
            if (lastAccountReference == null) lastAccountReference = new AccountReference { Code = accountReferencesType.Value + "0000" };

            return (Convert.ToInt32(lastAccountReference.Code) + 1).ToString();
        } 
    }
}
    
