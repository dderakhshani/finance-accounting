using AutoMapper;
using Eefa.Accounting.Application.UseCases.CodeRowDescription.Model;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Library.Common;
using Library.Interfaces;
using Library.Models;
using Microsoft.EntityFrameworkCore;
 

namespace Eefa.Accounting.Application.UseCases.CodeRowDescription.Command.Delete
{
    public class DeleteCodeRowDescriptionCommand : CommandBase, IRequest<ServiceResult>, ICommand
    {
        public int Id { get; set; }
    }

    public class DeleteCodeRowDescriptionCommandHandler : IRequestHandler<DeleteCodeRowDescriptionCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public DeleteCodeRowDescriptionCommandHandler(IRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(DeleteCodeRowDescriptionCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Data.Entities.CodeRowDescription>(c =>
             c.ObjectId(request.Id))
             .FirstOrDefaultAsync(cancellationToken);

            _repository.Delete(entity);
            if (await _repository.SaveChangesAsync(request.MenueId,cancellationToken) > 0)
            {
                return ServiceResult.Success(_mapper.Map<CodeRowDescriptionModel>(entity));
            }
            return ServiceResult.Failure();

        }
    }
}
