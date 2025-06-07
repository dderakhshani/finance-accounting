using AutoMapper;
using Eefa.Bursary.Domain.Aggregates.ChequeAggregate;
using Eefa.Common;
using Eefa.Common.CommandQuery;
 
using MediatR;
using System;
 
using System.Threading;
using System.Threading.Tasks;
using Eefa.Bursary.Domain.Entities;
using Eefa.Bursary.Infrastructure;


namespace Eefa.Bursary.Application.Commands.Cheque.Create
{
    public class CreateChequeCommand : Common.CommandQuery.CommandBase, IRequest<ServiceResult>, IMapFrom<CreateChequeCommand>, ICommand
    {
        public int Id { get; set; }
        public string Sheba { get; set; }
        public int BankBranchId { get; set; }
        public string AccountNumber { get; set; }
        public int SheetsCount { get; set; }
        public string? ChequeNumberIdentification { get; set; }
        public int? OwnerEmployeeId { get; set; }
        public DateTime? SetOwnerTime { get; set; }
        public bool? IsFinished { get; set; }
        public int? BankAccountId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateChequeCommand, PayCheque>().IgnoreAllNonExisting();
        }

    }

    public class CreateChequeCommandHandler : IRequestHandler<CreateChequeCommand, ServiceResult>
    {
        private readonly IMapper _autoMaper;
        private readonly IChequeRepository _chequeRepository;
        private readonly IApplicationLogs _applicationLogs;

        public CreateChequeCommandHandler(IMapper autoMaper, IChequeRepository chequeRepository, IApplicationLogs applicationLogs)
        {
            _autoMaper = autoMaper;
            _chequeRepository = chequeRepository;
            _applicationLogs = applicationLogs;
        }

        public async Task<ServiceResult> Handle(CreateChequeCommand request, CancellationToken cancellationToken)
        {
            await _applicationLogs.CommitLog(request);

            var cheque = _autoMaper.Map<PayCheque>(request);

            var entity = _chequeRepository.Insert(cheque);

            await _chequeRepository.SaveChangesAsync(cancellationToken);

            return ServiceResult.Success();
        }

    }
}
