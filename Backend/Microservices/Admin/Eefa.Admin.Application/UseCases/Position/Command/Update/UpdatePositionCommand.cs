using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Admin.Application.CommandQueries.Position.Model;
using Library.Common;
using Library.Interfaces;
using Library.Mappings;
using Library.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
 

namespace Eefa.Admin.Application.CommandQueries.Position.Command.Update
{
    public class UpdatePositionCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<Data.Databases.Entities.Position>, ICommand
    {
        public int Id { get; set; }
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
            profile.CreateMap<UpdatePositionCommand, Data.Databases.Entities.Position>()
                .IgnoreAllNonExisting();
        }
    }


    public class UpdatePositionCommandHandler : IRequestHandler<UpdatePositionCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public UpdatePositionCommandHandler(IRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(UpdatePositionCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Data.Databases.Entities.Position>(c =>
            c.ObjectId(request.Id))
            .FirstOrDefaultAsync(cancellationToken);

            entity.Title = request.Title;
            entity.ParentId = request.ParentId;

            var updateEntity = _repository.Update(entity);

            return await request.Save<Data.Databases.Entities.Position,PositionModel>(_repository, _mapper,updateEntity.Entity, cancellationToken);

        }
    }
}
