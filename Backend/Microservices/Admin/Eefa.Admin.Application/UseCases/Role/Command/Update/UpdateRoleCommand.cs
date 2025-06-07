using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Admin.Application.CommandQueries.Role.Model;
using Library.Common;
using Library.Interfaces;
using Library.Mappings;
using Library.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Eefa.Admin.Application.CommandQueries.Role.Command.Update
{
    public class UpdateRoleCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<Data.Databases.Entities.Role>, ICommand
    {
        public int Id { get; set; }


        /// <summary>
        /// عنوان
        /// </summary>
        public string Title { get; set; } = default!;

        /// <summary>
        /// نام اختصاصی
        /// </summary>
        public string? UniqueName { get; set; }

        /// <summary>
        /// توضیحات
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// کد والد
        /// </summary>
        public int? ParentId { get; set; }


        public IList<int> PermissionsId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateRoleCommand, Data.Databases.Entities.Role>()
                .IgnoreAllNonExisting();
        }
    }


    public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public UpdateRoleCommandHandler(IRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Data.Databases.Entities.Role>(c =>
            c.ObjectId(request.Id)).Include(x=>x.RolePermissionRoles)
            .FirstOrDefaultAsync(cancellationToken);

            entity.ParentId = request.ParentId;
            entity.UniqueName = request.UniqueName;
            entity.Title = request.Title;
            entity.Description = request.Description;

            foreach (var removedPermission in entity.RolePermissionRoles.Select(x => x.PermissionId).Except(request.PermissionsId))
            {
                var deletingEntity = await _repository
                    .Find<Data.Databases.Entities.RolePermission>(c =>
                        c.ConditionExpression(x=>x.PermissionId == removedPermission && x.RoleId == entity.Id))
                    .FirstOrDefaultAsync(cancellationToken);

                _repository.Delete<Data.Databases.Entities.RolePermission>(deletingEntity);
            }

            foreach (var addedPermission in request.PermissionsId.Except(entity.RolePermissionRoles.Select(x => x.PermissionId)))
            {
                if (await _repository.GetQuery<Data.Databases.Entities.RolePermission>().AnyAsync(x =>
                    x.RoleId == entity.Id && x.PermissionId == addedPermission &&
                    x.IsDeleted != true, cancellationToken: cancellationToken))
                {
                    continue;
                }


                _repository.Insert<Data.Databases.Entities.RolePermission>(new Data.Databases.Entities.RolePermission()
                {
                    RoleId = entity.Id,
                    PermissionId = addedPermission
                });

            }


            var updateEntity = _repository.Update(entity);

            return await request.Save<Data.Databases.Entities.Role,RoleModel>(_repository,_mapper, updateEntity.Entity, cancellationToken);

        }
    }
}
