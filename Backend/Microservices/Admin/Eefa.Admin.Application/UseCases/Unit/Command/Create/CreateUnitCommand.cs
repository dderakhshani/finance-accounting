using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Admin.Application.CommandQueries.Unit.Model;
using Library.Common;
using Library.Interfaces;
using Library.Mappings;
using Library.Models;
using MediatR;


namespace Eefa.Admin.Application.CommandQueries.Unit.Command.Create
{
    public class CreateUnitCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<CreateUnitCommand>, ICommand
    {

        /// <summary>
        /// عنوان
        /// </summary>
        public string Title { get; set; } = default!;

        /// <summary>
        /// کد والد
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        /// کد شعبه
        /// </summary>
        public int BranchId { get; set; }
        public IList<int> PositionIds { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateUnitCommand, Data.Databases.Entities.Unit>()
                .IgnoreAllNonExisting();
        }
    }

    public class CreateUnitCommandHandler : IRequestHandler<CreateUnitCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public CreateUnitCommandHandler(IRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }


        public async Task<ServiceResult> Handle(CreateUnitCommand request, CancellationToken cancellationToken)
        {
            var entity = _repository.Insert(_mapper.Map<Data.Databases.Entities.Unit>(request));

            foreach (var possition in request.PositionIds)
            {
                _repository.Insert<Data.Databases.Entities.UnitPosition>(new Data.Databases.Entities.UnitPosition()
                    { PositionId = possition, Unit = entity.Entity });
            }
            var result= await request.Save<Data.Databases.Entities.Unit, UnitModel>(_repository, _mapper, entity.Entity, cancellationToken);
            return result;

        }
    }
}
