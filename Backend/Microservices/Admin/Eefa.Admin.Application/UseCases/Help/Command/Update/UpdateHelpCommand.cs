using Library.Interfaces;
using AutoMapper;
using System.Threading.Tasks;
using MediatR;
using Library.Common;
using Library.Models;
using Library.Mappings;
using System.Threading;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Admin.Application.UseCases.Help.Command.Update
{
    public class UpdateHelpCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<Data.Databases.Entities.Help>, ICommand
    {
        public int Id { get; set; }
        public int MenuItemId { get; set; }
        public string Contents { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateHelpCommand, Data.Databases.Entities.Help>()
                .ForMember(x => x.MenuId, opt => opt.MapFrom(x => x.MenuItemId));
        }
    }

    public class UpdateHelpCommandHandler : IRequestHandler<UpdateHelpCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public UpdateHelpCommandHandler(IRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(UpdateHelpCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Data.Databases.Entities.Help>(c =>
                c.ObjectId(request.Id))
                .FirstOrDefaultAsync(cancellationToken);

            entity.Contents = request.Contents;
            entity.MenuId = request.MenuItemId;

            var updateEntity = _repository.Update(entity);
            return await request.Save<Data.Databases.Entities.Help, Model.HelpModel>(_repository, _mapper, updateEntity.Entity, cancellationToken);
        }
    }
}
