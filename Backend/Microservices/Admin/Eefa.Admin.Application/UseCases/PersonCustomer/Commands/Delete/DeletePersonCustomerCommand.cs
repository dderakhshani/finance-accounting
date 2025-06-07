using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Library.Common;
using Library.Interfaces;
using Library.Mappings;
using Library.Models;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Admin.Application.CommandQueries.PersonCustomer.Commands.Delete
{

    public class DeletePersonCustomerCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<Data.Databases.Entities.Customer>, ICommand
    {
        public int Id { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<DeletePersonCustomerCommand, Data.Databases.Entities.Customer>();
        }
    }

    public class DeletePersonCustomerCommandHandler : IRequestHandler<DeletePersonCustomerCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public DeletePersonCustomerCommandHandler(IRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<ServiceResult> Handle(DeletePersonCustomerCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Data.Databases.Entities.Customer>(c =>
                    c.ObjectId(request.Id))
                .FirstOrDefaultAsync(cancellationToken);

            var deletedEntity = _repository.Delete(entity);
            return await request.Save(_repository, cancellationToken);
        }
    }
}
