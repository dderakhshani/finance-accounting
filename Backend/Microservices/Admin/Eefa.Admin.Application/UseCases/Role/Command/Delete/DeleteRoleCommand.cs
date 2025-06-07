using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Library.Common;
using Library.Interfaces;
using Library.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
 

namespace Eefa.Admin.Application.CommandQueries.Role.Command.Delete
{
    public class DeleteRoleCommand : CommandBase, IRequest<ServiceResult>, ICommand
    {
        public int Id { get; set; }
    }

    public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public DeleteRoleCommandHandler(IRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Data.Databases.Entities.Role>(c =>
             c.ObjectId(request.Id)).Include(x=>x.RolePermissionRoles)
             .FirstOrDefaultAsync(cancellationToken);

            foreach (var rolePermission in entity.RolePermissionRoles)
            {
                _repository.Delete<Data.Databases.Entities.RolePermission>(rolePermission);
            }

            var deletedEntity = _repository.Delete(entity);
            return await request.Save(_repository, cancellationToken);

        }
    }
}
