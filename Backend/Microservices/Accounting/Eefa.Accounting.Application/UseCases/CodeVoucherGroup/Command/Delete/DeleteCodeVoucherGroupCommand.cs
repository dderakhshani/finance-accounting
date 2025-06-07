using AutoMapper;
using Eefa.Accounting.Application.UseCases.CodeVoucherGroup.Model;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Library.Common;
using Library.Interfaces;
using Library.Models;
using Microsoft.EntityFrameworkCore;
 

namespace Eefa.Accounting.Application.UseCases.CodeVoucherGroup.Command.Delete
{
    public class DeleteCodeVoucherGroupCommand : CommandBase, IRequest<ServiceResult>, ICommand
    {
        public int Id { get; set; }
    }

    public class DeleteCodeVoucherGroupCommandHandler : IRequestHandler<DeleteCodeVoucherGroupCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public DeleteCodeVoucherGroupCommandHandler(IRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(DeleteCodeVoucherGroupCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Data.Entities.CodeVoucherGroup>(c =>
             c.ObjectId(request.Id))
             .FirstOrDefaultAsync(cancellationToken);

            _repository.Delete(entity);
            if (await _repository.SaveChangesAsync(request.MenueId,cancellationToken) > 0)
            {
                return ServiceResult.Success(_mapper.Map<CodeVoucherGroupModel>(entity));
            }
            return ServiceResult.Failure();
        }
    }
}
