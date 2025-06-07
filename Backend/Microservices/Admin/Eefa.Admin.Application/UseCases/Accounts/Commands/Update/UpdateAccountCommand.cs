using AutoMapper;
using Eefa.Admin.Application.CommandQueries.Accounts.Models;
using Eefa.Admin.Application.CommandQueries.Person.Command.Update;
using Eefa.Admin.Data.Databases.Entities;
using Library.Common;
using Library.ConfigurationAccessor.Mappings;
using Library.Interfaces;
using Library.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Admin.Application.CommandQueries.Accounts.Commands.Update
{
    public class UpdateAccountCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<UpdateAccountCommand>, ICommand
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Code { get; set; }
        public int Type { get; set; }
        public bool IsActive { get; set; }

        public UpdatePersonCommand Person { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateAccountCommand, Data.Databases.Entities.AccountReference>()
                .IgnoreAllNonExisting();
        }
    }

    public class UpdateAccountCommandHandler : IRequestHandler<UpdateAccountCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public UpdateAccountCommandHandler(IRepository repository, IMapper mapper, IMediator mediator)
        {
            _mapper = mapper;
            _mediator = mediator;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
        {
            var account = await _repository
                 .Find<AccountReference>(c =>
             c.ObjectId(request.Id))
                 .Include(x => x.Person)
             .FirstOrDefaultAsync(cancellationToken);

            account.Code = request.Code;
            account.Title = request.Title;
            account.IsActive = request.IsActive;

            if (account.Person == null)
            {

                var newPerson = _mapper.Map<Data.Databases.Entities.Person>(request.Person);

            }
            else
            {

            }

            await _repository.SaveChangesAsync();

            return ServiceResult.Success(_mapper.Map<AccountModel>(account));
        }
    }
}
    
