using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Bursary.Domain.Entities.Definitions;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using Eefa.Common.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Bursary.Application.Commands.ChequeSheet
{
    public class CreateChequeSheetCommand : Common.CommandQuery.CommandBase, IRequest<ServiceResult>, IMapFrom<CreateChequeSheetCommand>, ICommand
    {
        public int Id { get; set; }
        public int? PayChequeId { get; set; }
        public int SheetSeqNumber { get; set; }
        public string SheetUniqueNumber { get; set; }
        public int SheetSeriNumber { get; set; }
        public decimal TotalCost { get; set; }
        public int? AccountReferenceId { get; set; }
        public int BankBranchId { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ReceiptDate { get; set; }
        public int IssuerEmployeeId { get; set; }
        public string Description { get; set; }
        public string AccountNumber { get; set; }
        public string BranchName { get; set; }
        public int? OwnerChequeReferenceId { get; set; }
        public int? OwnerChequeReferenceGroupId { get; set; }
        public int? ReceiveChequeReferenceId { get; set; }
        public int? ReceiveChequeReferenceGroupId { get; set; }
        public bool? ApproveReceivedChequeSheet { get; set; }
        public int BankId { get; set; }
        public string BankCode { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsUsed { get; set; } 

        public int ChequeTypeBaseId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateChequeSheetCommand, Domain.Entities.ChequeSheets>().IgnoreAllNonExisting();
        }

    }

    public class CreateChequeSheetCommandHandler : IRequestHandler<CreateChequeSheetCommand, ServiceResult>
    {
        private readonly IMapper _autoMaper;
        private readonly IRepository<Domain.Entities.ChequeSheets> _chequeSheetRepository;
        private readonly IRepository<Banks> _bankRepository;
        private readonly IApplicationLogs _applicationLogs;
        public CreateChequeSheetCommandHandler(IMapper autoMaper, IRepository<Domain.Entities.ChequeSheets> chequeSheetRepository, IRepository<Banks> bankRepository, IApplicationLogs applicationLogs)
        {
            _autoMaper = autoMaper;
            _chequeSheetRepository = chequeSheetRepository;
            _bankRepository = bankRepository;
            _applicationLogs = applicationLogs;
        }

        public async Task<ServiceResult> Handle(CreateChequeSheetCommand request, CancellationToken cancellationToken)
        {
            await _applicationLogs.CommitLog(request);

            var bankId = await _bankRepository.GetAll().Where(x => x.Code == request.BankCode).Select(x=>x.Id).FirstAsync();
            request.BankId = bankId;
            var chequeSheet = _autoMaper.Map<Domain.Entities.ChequeSheets>(request);

            var entity = _chequeSheetRepository.Insert(chequeSheet);
            entity.ChequeDocumentState = 0;

            await _chequeSheetRepository.SaveChangesAsync(cancellationToken);

            return ServiceResult.Success();
        }

    }

}
