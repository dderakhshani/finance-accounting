using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Library.Common;
using Library.Interfaces;
using Library.Mappings;
using Library.Models;


namespace Eefa.Accounting.Application.UseCases.CodeRowDescription.Command.Create
{
    public class CreateCodeRowDescriptionCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<CreateCodeRowDescriptionCommand>, ICommand
    {

        public string Title { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateCodeRowDescriptionCommand, Data.Entities.CodeRowDescription>()
                .IgnoreAllNonExisting();
        }
    }

    public class CreateCodeRowDescriptionCommandHandler : IRequestHandler<CreateCodeRowDescriptionCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        public CreateCodeRowDescriptionCommandHandler(IRepository repository, IMapper mapper, ICurrentUserAccessor currentUserAccessor)
        {
            _mapper = mapper;
            _currentUserAccessor = currentUserAccessor;
            _repository = repository;
        }


        public async Task<ServiceResult> Handle(CreateCodeRowDescriptionCommand request, CancellationToken cancellationToken)
        {
            var input = _mapper.Map<Data.Entities.CodeRowDescription>(request);
            input.CompanyId = _currentUserAccessor.GetCompanyId();
            var entity =  _repository.Insert(input);

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
