using AutoMapper;
using Eefa.Bursary.Domain.Entities.Definitions;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using Eefa.Common.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application.Commands.ChequeSheet.Update
{
    public class UpdateChequeSheetCommand : Common.CommandQuery.CommandBase, IRequest<ServiceResult>, IMapFrom<UpdateChequeSheetCommand>, ICommand
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
            profile.CreateMap<UpdateChequeSheetCommand, Domain.Entities.ChequeSheets>().IgnoreAllNonExisting();
        }
    }


    public class UpdateChequeSheetCommandHandler : IRequestHandler<UpdateChequeSheetCommand, ServiceResult>
    {
        private readonly IMapper _autoMaper;
        private readonly IRepository<Domain.Entities.ChequeSheets> _chequeSheetRepository;
        private readonly IRepository<Banks> _bankRepository;

        public UpdateChequeSheetCommandHandler(IMapper autoMaper, IRepository<Domain.Entities.ChequeSheets> chequeSheetRepository, IRepository<Banks> bankRepository)
        {
            _autoMaper = autoMaper;
            _chequeSheetRepository = chequeSheetRepository;
            _bankRepository = bankRepository;
        }

        public async Task<ServiceResult> Handle(UpdateChequeSheetCommand request, CancellationToken cancellationToken)
        {

           
            var bankId = await _bankRepository.GetAll().Where(x => x.Code == request.BankCode).Select(x => x.Id).FirstAsync();
            request.BankId = bankId;
            var chequeSheet = await _chequeSheetRepository.GetAll().Where(x => x.Id == request.Id).FirstAsync();

            if (chequeSheet.ChequeDocumentState != 0)
                throw new Exception("چک استفاده شده و قابل ویرایش نیست");

            chequeSheet.AccountNumber = request.AccountNumber;
            chequeSheet.SheetSeqNumber =  request.SheetSeqNumber.ToString();
            chequeSheet.SheetUniqueNumber = request.SheetUniqueNumber;
            chequeSheet.SheetSeriNumber = request.SheetSeriNumber.ToString();
            chequeSheet.TotalCost = request.TotalCost;
            chequeSheet.IssueDate = request.IssueDate;
            chequeSheet.ReceiptDate = request.ReceiptDate;
            chequeSheet.Description = request.Description;
            chequeSheet.BranchName = request.BranchName;
            chequeSheet.OwnerChequeReferenceId = request.OwnerChequeReferenceId;
            chequeSheet.BankId = request.BankId;
            chequeSheet.ChequeTypeBaseId = request.ChequeTypeBaseId;
             
            var entity = _chequeSheetRepository.Update(chequeSheet);
           

            await _chequeSheetRepository.SaveChangesAsync(cancellationToken);

            return ServiceResult.Success();
        }

    }

}
