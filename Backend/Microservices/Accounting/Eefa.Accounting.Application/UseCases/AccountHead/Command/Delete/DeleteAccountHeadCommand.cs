using AutoMapper;
using Eefa.Accounting.Application.UseCases.AccountHead.Model;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Library.Common;
using Library.Interfaces;
using Library.Models;
using Microsoft.EntityFrameworkCore;


namespace Eefa.Accounting.Application.UseCases.AccountHead.Command.Delete
{
    public class DeleteAccountHeadCommand : CommandBase, IRequest<ServiceResult>, ICommand
    {
        public int Id { get; set; }
        public bool ForceDeactive { get; set; } = false;

    }

    public class DeleteAccountHeadCommandHandler : IRequestHandler<DeleteAccountHeadCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        public DeleteAccountHeadCommandHandler(IRepository repository, IMapper mapper, ICurrentUserAccessor currentUserAccessor)
        {
            _mapper = mapper;
            _currentUserAccessor = currentUserAccessor;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(DeleteAccountHeadCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Data.Entities.AccountHead>(c =>
             c.ObjectId(request.Id))
             .FirstOrDefaultAsync(cancellationToken);


                _repository.Delete(entity);
                if (await _repository.SaveChangesAsync(request.MenueId,cancellationToken) > 0)
                {
                    return ServiceResult.Success(_mapper.Map<AccountHeadModel>(entity));
                }
            return ServiceResult.Failure();
        }
    }
}
