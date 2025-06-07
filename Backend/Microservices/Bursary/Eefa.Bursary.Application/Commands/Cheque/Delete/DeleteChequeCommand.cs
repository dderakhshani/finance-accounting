using AutoMapper;
using Eefa.Bursary.Domain.Aggregates.ChequeAggregate;
using Eefa.Common;
using Eefa.Common.CommandQuery;
 
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application.Commands.Cheque.Delete
{
   public class DeleteChequeCommand:CommandBase, IRequest<ServiceResult>, ICommand
    {
        public int Id { get; set; }
    }


    public class DeleteChequeCommandHandler : IRequestHandler<DeleteChequeCommand, ServiceResult>
    {

        private readonly IChequeRepository _chequeRepository;
        private readonly IMapper _mapper;
        private readonly IApplicationLogs _applicationLogs;

        public DeleteChequeCommandHandler(IChequeRepository chequeRepository, IMapper mapper, IApplicationLogs applicationLogs)
        {
            _chequeRepository = chequeRepository;
            _mapper = mapper;
            _applicationLogs = applicationLogs;
        }


        public async Task<ServiceResult> Handle(DeleteChequeCommand request, CancellationToken cancellationToken)
        {
            await _applicationLogs.CommitLog(request);
            var cheque = await _chequeRepository.Find(request.Id);

            var deletedEntity = _chequeRepository.Delete(cheque);
            if (await _chequeRepository.SaveChangesAsync(cancellationToken) > 0)
            {
                return ServiceResult.Success();
            }
            return ServiceResult.Failed();

        }
    }
}
