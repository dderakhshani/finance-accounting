using AutoMapper;
using Dapper;
using Eefa.Bursary.Application.UseCases.AutoVouchers.GeneralAV.Commands.Add;
using Eefa.Bursary.Application.UseCases.AutoVouchers.PayableDocs.Commands.Add;
using Eefa.Bursary.Application.UseCases.Payables.Documents.Models;
using Eefa.Bursary.Domain.Entities;
using Eefa.Bursary.Domain.Entities.Payables;
using Eefa.Bursary.Infrastructure.Interfaces;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using Eefa.Common.Exceptions;
using Eefa.Common.Web;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application.UseCases.Payables.Documents.Commands.Add
{
    public class CreateDocumentCommand : CommandBase, IRequest<ServiceResult<Payables_Documents>>, IMapFrom<CreateDocumentCommand>, ICommand
    {
        public int PayTypeId { get; set; }
        public int? ChequeTypeId { get; set; }
        public int? ChequeBookSheetId { get; set; } = null;
        public int? BankAccountId { get; set; } = null;
        public int MonetarySystemId { get; set; }
        public int? CreditAccountHeadId { get; set; }
        public int? CreditReferenceId { get; set; }
        public int? CreditReferenceGroupId { get; set; }
        public int? CurrencyTypeBaseId { get; set; } = null;
        public DateTime DocumentDate { get; set; }
        public DateTime? DueDate { get; set; }
        public int SubjectId { get; set; }
        public long CurrencyRate { get; set; }
        public long CurrencyAmount { get; set; }
        public long Amount { get; set; }
        public string Descp { get; set; }
        public bool? ShowDebit { get; set; } = false;
        public bool? ShowCredit { get; set; } = false;
        public string? ReferenceNumber { get; set; }
        public string? TrackingNumber { get; set; }
        public DateTime? DraftDate { get; set; }
        public int StatusId { get; set; } = 29358;
        public List<DocumentAccountsModel> Accounts { get; set; }
        public List<DocumentPayOrdersModel> PayOrders { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateDocumentCommand, Payables_Documents>();
            profile.CreateMap<DocumentAccountsModel, Payables_DocumentsAccounts>().IgnoreAllNonExisting();
        }
    }

    public class CreateDocumentCommandHandler : IRequestHandler<CreateDocumentCommand, ServiceResult<Payables_Documents>>
    {
        private readonly IMapper _mapper;
        private readonly IBursaryUnitOfWork _uow;
        private readonly IConfigurationAccessor _config;
        private readonly IMediator _mediator;
        protected readonly ICurrentUserAccessor _currentUserAccessor;

        public CreateDocumentCommandHandler(IMapper mapper, IBursaryUnitOfWork uow, IConfigurationAccessor config, IMediator mediator, ICurrentUserAccessor currentUserAccessor = null)
        {
            _mapper = mapper;
            _uow = uow;
            _config = config;
            _mediator = mediator;
            _currentUserAccessor = currentUserAccessor;
        }

        public async Task<ServiceResult<Payables_Documents>> Handle(CreateDocumentCommand request, CancellationToken cancellationToken)
        {
            var document = _mapper.Map<Payables_Documents>(request);

            if (document == null)
            {
                throw new ValidationError("اطلاعات ورودی ارسال نگردیده است");
            }

            if (document.BankAccountId != null && document.BankAccountId == 0) document.BankAccountId = null;
            if (document.ChequeBookSheetId != null && document.ChequeBookSheetId == 0) document.ChequeBookSheetId = null;
            if (document.CurrencyTypeBaseId != null && document.CurrencyTypeBaseId == 0) document.CurrencyTypeBaseId = null;
            if (document.MonetarySystemId == 29341) document.Amount = document.CurrencyRate * document.CurrencyAmount;
            if (document.DocumentDate == DateTime.MinValue) document.DocumentDate = DateTime.Now;
            var dno = await _uow.Payables_DocumentsOperations.Where(w => w.YearId == _currentUserAccessor.GetYearId()).MaxAsync(w => w.YearId);
            if (dno == null || dno <= 0) dno = 0;
            dno += 1;
            document.DocumentNo = dno;
            foreach (var i in request.Accounts)
            {
                var ent = new Payables_DocumentsAccounts();
                _mapper.Map(i, ent);
                if (ent.AccountHeadId == 0) ent.AccountHeadId = null;
                if (ent.RexpId == 0) ent.RexpId = null;
                if (ent.ReferenceGroupId == 0) ent.ReferenceGroupId = null;

                document.Payables_DocumentsAccounts.Add(ent);
            }

            if (request.PayOrders != null && request.PayOrders.Count > 0)
            {
                foreach (var i in request.PayOrders)
                {
                    var ent = new Payables_DocumentsPayOrders();
                    ent.PayOrderId = i.PayOrderId;
                    document.Payables_DocumentsPayOrders.Add(ent);
                }
            }

            var opr = new Payables_DocumentsOperations();
            opr.Descp = "ثبت سند پرداختی";
            opr.OperationId = 28778;
            opr.OperationDate = DateTime.Now;
            opr.YearId = _currentUserAccessor.GetYearId();
            document.Payables_DocumentsOperations.Add(opr);

            _uow.Payables_Documents.Add(document);
            var value = await _uow.SaveChangesAsync(cancellationToken);
            if (value <= 0)
            {
                throw new Exception("بروز خطا در ثبت اطلاعات سند پرداختی");
            }

            //-------- autovoucher
            var vchcmd = new AddPayableDocsAutovoucher()
            {
                DocumentId = document.Id,
                OperationId = 29358
            };

            var resvch = await _mediator.Send(vchcmd, cancellationToken);
            if (resvch == null || !resvch.Succeed)
            {
                document.StatusId = 29359;
                _uow.Payables_Documents.Update(document);
                value = await _uow.SaveChangesAsync(cancellationToken);
            }
            else if (resvch.Succeed == true)
            {
                document.StatusId = 29358;

                _uow.Payables_Documents.Update(document);
                value = await _uow.SaveChangesAsync(cancellationToken);

            }
            //=============
            return ServiceResult<Payables_Documents>.Success(document);

        }
    }


}
