using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Accounting.Application.UseCases.AccountReferencesGroup.Model;
using Library.Common;
using Library.Interfaces;
using Library.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Accounting.Application.UseCases.AccountReferencesGroup.Command.Delete
{
    public class DeleteAccountReferencesGroupCommand : CommandBase, IRequest<ServiceResult>, ICommand
    {
        public int Id { get; set; }
    }

    public class DeleteReferencesGroupCommandHandler : IRequestHandler<DeleteAccountReferencesGroupCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public DeleteReferencesGroupCommandHandler(IRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(DeleteAccountReferencesGroupCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Eefa.Accounting.Data.Entities.AccountReferencesGroup>(c =>
             c.ObjectId(request.Id))
             .FirstOrDefaultAsync(cancellationToken);

            _repository.Delete(entity);


            if (await _repository.SaveChangesAsync(request.MenueId,cancellationToken) > 0)
            {
                return ServiceResult.Success(_mapper.Map<AccountReferencesGroupModel>(entity));
            }
            return ServiceResult.Failure();
        }
    }
}
