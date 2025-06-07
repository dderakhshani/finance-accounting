using AutoMapper;
using Library.Common;
using Library.Interfaces;
using Library.Mappings;
using Library.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Admin.Application.UseCases.Help.Command.Create
{
    public class CreateHelpCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<CreateHelpCommand>, ICommand
    {
        public int MenuItemId { get; set; }
        public string Contents { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateHelpCommand, Data.Databases.Entities.Help>()
                .ForMember(x => x.MenuId, opt => opt.MapFrom(x => x.MenuItemId));
        }
    }

    public class CreateHelpCommandHandler : IRequestHandler<CreateHelpCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public CreateHelpCommandHandler(IRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ServiceResult> Handle(CreateHelpCommand request, CancellationToken cancellationToken)
        {
            var entity = _repository.Insert(_mapper.Map<Data.Databases.Entities.Help>(request));
            return await request.Save(_repository, entity.Entity, cancellationToken);
        }
    }
}
