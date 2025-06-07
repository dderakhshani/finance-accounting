using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Library.Common;
using Library.Interfaces;
using Library.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Accounting.Application.UseCases.AccountReference.Command.RemovereferencesFromGroup
{
    public class RemoveReferencesFromGroupCommand : CommandBase, IRequest<ServiceResult>, ICommand
    {
        public int GroupId { get; set; }
        public ICollection<int> AccountReferenceIds { get; set; }
    }

    public class RemoveReferencesFromGroupCommandHandler : IRequestHandler<RemoveReferencesFromGroupCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public RemoveReferencesFromGroupCommandHandler(IRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(RemoveReferencesFromGroupCommand request, CancellationToken cancellationToken)
        {
            await _repository.GetQuery<Data.Entities.AccountReferencesRelReferencesGroup>()
                .Where(x => x.ReferenceGroupId == request.GroupId &&
                            request.AccountReferenceIds.Contains(x.ReferenceId))
                .ForEachAsync(x => _repository.Delete(x), cancellationToken: cancellationToken);

            if (await _repository.SaveChangesAsync(request.MenueId,cancellationToken) > 0)
            {
                return ServiceResult.Success();
            }
            return ServiceResult.Failure();
        }
    }
}
