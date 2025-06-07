using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Admin.Application.CommandQueries.Employee.Model;
using Library.Common;
using Library.Interfaces;
using Library.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
 

namespace Eefa.Admin.Application.CommandQueries.Employee.Command.Delete
{
    public class DeleteEmployeeCommand : CommandBase, IRequest<ServiceResult>, ICommand
    {
        public int Id { get; set; }
    }

    public class DeleteEmployeeCommandHandler : IRequestHandler<DeleteEmployeeCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public DeleteEmployeeCommandHandler(IRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Data.Databases.Entities.Employee>(c =>
             c.ObjectId(request.Id))
             .FirstOrDefaultAsync(cancellationToken);

            var deletedEntity = _repository.Delete(entity);
            return await request.Save<Data.Databases.Entities.Employee, EmployeeModel>(_repository,_mapper, deletedEntity.Entity, cancellationToken);
        }
    }
}
