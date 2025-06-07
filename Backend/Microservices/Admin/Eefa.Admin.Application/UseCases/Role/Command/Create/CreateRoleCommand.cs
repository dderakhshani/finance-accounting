using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Admin.Application.CommandQueries.Role.Model;
using Library.Common;
using Library.Interfaces;
using Library.Mappings;
using Library.Models;
using MediatR;


namespace Eefa.Admin.Application.CommandQueries.Role.Command.Create
{
    public class CreateRoleCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<CreateRoleCommand>, ICommand
    {

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
            profile.CreateMap<CreateRoleCommand, Data.Databases.Entities.Role>()
                .IgnoreAllNonExisting();
        }
    }

    public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public CreateRoleCommandHandler(IRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }


        public async Task<ServiceResult> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            var role =  _repository.Insert(_mapper.Map<Data.Databases.Entities.Role>(request));
            foreach (var permission in request.PermissionsId)
            {
                 _repository.Insert(new Data.Databases.Entities.RolePermission() { PermissionId = permission, Role = role.Entity });
            }


            return await request.Save<Data.Databases.Entities.Role,RoleModel>(_repository, _mapper,role.Entity, cancellationToken);

        }
    }
}
