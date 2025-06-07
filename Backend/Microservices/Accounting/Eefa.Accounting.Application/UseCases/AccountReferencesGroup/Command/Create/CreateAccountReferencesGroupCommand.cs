using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Library.Common;
using Library.Interfaces;
using Library.Mappings;
using Library.Models;
using MediatR;

namespace Eefa.Accounting.Application.UseCases.AccountReferencesGroup.Command.Create
{
    public class CreateAccountReferencesGroupCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<CreateAccountReferencesGroupCommand>, ICommand
    {
        public int? ParentId { get; set; }
        public string Title { get; set; }
        public bool? IsEditable { get; set; }
        public string Code { get; set; }


        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateAccountReferencesGroupCommand, Eefa.Accounting.Data.Entities.AccountReferencesGroup>()
                .IgnoreAllNonExisting();
        }
    }

    public class CreateAccountReferencesGroupCommandHandler : IRequestHandler<CreateAccountReferencesGroupCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        public CreateAccountReferencesGroupCommandHandler(IRepository repository, IMapper mapper, ICurrentUserAccessor currentUserAccessor)
        {
            _mapper = mapper;
            _currentUserAccessor = currentUserAccessor;
            _repository = repository;
        }


        public async Task<ServiceResult> Handle(CreateAccountReferencesGroupCommand request, CancellationToken cancellationToken)
        {
            var input = _mapper.Map<Eefa.Accounting.Data.Entities.AccountReferencesGroup>(request);
            input.CompanyId = _currentUserAccessor.GetCompanyId();
            var entity = _repository.Insert(input);

            if (request.SaveChanges)
            {
                if (await _repository.SaveChangesAsync(request.MenueId,cancellationToken) > 0)
                {
                    return ServiceResult.Success(entity.Entity);
                }
            }
            else
            {
                return ServiceResult.Success(entity.Entity);
            }

            return ServiceResult.Failure();
        }
    }
}
