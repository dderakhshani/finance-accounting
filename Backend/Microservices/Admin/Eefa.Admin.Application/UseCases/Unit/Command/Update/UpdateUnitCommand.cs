using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Eefa.Admin.Application.CommandQueries.Unit.Model;
using Library.Common;
using Library.Interfaces;
using Library.Mappings;
using Library.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Eefa.Admin.Application.CommandQueries.Unit.Command.Update
{
    public class UpdateUnitCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<Data.Databases.Entities.Unit>, ICommand
    {
        public int Id { get; set; }


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
            profile.CreateMap<UpdateUnitCommand, Data.Databases.Entities.Unit>()
                .IgnoreAllNonExisting();
        }
    }


    public class UpdateUnitCommandHandler : IRequestHandler<UpdateUnitCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public UpdateUnitCommandHandler(IRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(UpdateUnitCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Data.Databases.Entities.Unit>(c =>
            c.ObjectId(request.Id)).Include(x => x.UnitPositions)
            .FirstOrDefaultAsync(cancellationToken);

            entity.ParentId = request.ParentId;
            entity.Title = request.Title;
            entity.BranchId = request.BranchId;

            var updateEntity = _repository.Update(entity);


            foreach (var removedPositions in entity.UnitPositions.Select(x => x.PositionId)
                .Except(request.PositionIds))
            {
                var deletingEntity = await _repository
                    .Find<Data.Databases.Entities.UnitPosition>(c =>
                        c.ConditionExpression(x => x.PositionId == removedPositions && x.UnitId == entity.Id))
                    .FirstOrDefaultAsync(cancellationToken);
                
                _repository.Delete<Data.Databases.Entities.UnitPosition>(deletingEntity);
            }


            foreach (var addedPositions in request.PositionIds.Except(entity.UnitPositions.Select(x => x.PositionId)))
            {
                if (await _repository.GetQuery<Data.Databases.Entities.UnitPosition>().AnyAsync(x =>
                    x.UnitId == entity.Id && x.PositionId == addedPositions &&
                    x.IsDeleted != true, cancellationToken: cancellationToken))
                {
                    continue;
                }
                _repository.Insert<Data.Databases.Entities.UnitPosition>(new Data.Databases.Entities.UnitPosition()
                {
                    PositionId = addedPositions,
                    UnitId = entity.Id
                });

            }


            if (await _repository.SaveChangesAsync(request.MenueId,cancellationToken) > 0)
            {
                return ServiceResult.Success(await _repository
                    .Find<Data.Databases.Entities.Unit>(c
                        => c.ObjectId(request.Id))
                    .ProjectTo<UnitModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(cancellationToken));
            }
            return ServiceResult.Failure();
        }
    }
}
