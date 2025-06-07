using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Library.Common;
using Library.Interfaces;
using Library.Mappings;
using Library.Models;
using MediatR;
 

namespace Eefa.Admin.Application.CommandQueries.Position.Command.Create
{
    public class CreatePositionCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<CreatePositionCommand>, ICommand
    {
        /// <summary>
        /// کد والد
        /// </summary>
        public int? ParentId { get; set; }


        /// <summary>
        /// عنوان
        /// </summary>
        public string Title { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreatePositionCommand, Data.Databases.Entities.Position>()
                .IgnoreAllNonExisting();
        }
    }

    public class CreatePositionCommandHandler : IRequestHandler<CreatePositionCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public CreatePositionCommandHandler(IRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }


        public async Task<ServiceResult> Handle(CreatePositionCommand request, CancellationToken cancellationToken)
        {
            var entity = _repository.Insert(_mapper.Map<Data.Databases.Entities.Position>(request));
            return await request.Save(_repository, entity.Entity, cancellationToken);

        }
    }
}
