using AutoMapper;
using Eefa.Accounting.Application.UseCases.AccountReference.Model;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Library.Common;
using Library.Interfaces;
using Library.Models;
using Microsoft.EntityFrameworkCore;
 

namespace Eefa.Accounting.Application.UseCases.AccountReference.Command.Delete
{
    public class DeleteAccountReferenceCommand : CommandBase, IRequest<ServiceResult>, ICommand
    {
        public int Id { get; set; }
        public bool ForceDeactive { get; set; } = false;
    }

    public class DeleteAccountReferenceCommandHandler : IRequestHandler<DeleteAccountReferenceCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public DeleteAccountReferenceCommandHandler(IRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(DeleteAccountReferenceCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Data.Entities.AccountReference>(c =>
             c.ObjectId(request.Id))
             .FirstOrDefaultAsync(cancellationToken);

            _repository.Delete(entity);

            if (await _repository.SaveChangesAsync(request.MenueId,cancellationToken) > 0)
            {
                return ServiceResult.Success(_mapper.Map<AccountReferenceModel>(entity));
            }
            return ServiceResult.Failure();
        }
    }
}
