using AutoMapper;
using Eefa.Bursary.Application.UseCases.Payables.Documents.Commands.Add;
using Eefa.Bursary.Application.UseCases.Payables.Documents.Models;
using Eefa.Bursary.Domain.Entities;
using Eefa.Bursary.Domain.Entities.Payables;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using MediatR;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using System.Threading;
using Eefa.Bursary.Infrastructure.Interfaces;
using Eefa.Common.Exceptions;
using System.Linq.Dynamic.Core;
using System.Security.Cryptography;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Eefa.Bursary.Application.UseCases.Payables.Documents.Commands.Update
{
    public class UpdateDocumentCommand: CommandBase, IRequest<ServiceResult<Payables_Documents>>, IMapFrom<CreateDocumentCommand>, ICommand
    {
        public int Id { get; set; }
        public int PayTypeId { get; set; }
        public int? ChequeTypeId { get; set; }
        public int? ChequeBookSheetId { get; set; }
        public int MonetarySystemId { get; set; }
        public int? CreditAccountHeadId { get; set; }
        public int? CreditReferenceId { get; set; }
        public int? CreditReferenceGroupId { get; set; }

        public int? BankAccountId { get; set; }
        public int? CurrencyTypeBaseId { get; set; }
        public DateTime DocumentDate { get; set; }
        public DateTime? DueDate { get; set; }
        public int SubjectId { get; set; }
        public long CurrencyRate { get; set; }
        public long CurrencyAmount { get; set; }
        public long Amount { get; set; }
        public string Descp { get; set; }
        public string DocumentNo { get; set; }
        public bool? ShowDebit { get; set; } = false;
        public bool? ShowCredit { get; set; } = false;
        public string? ReferenceNumber { get; set; }
        public string? TrackingNumber { get; set; }
        public DateTime? DraftDate { get; set; }

        public List<DocumentAccountsModel> Accounts { get; set; }
        public List<DocumentPayOrdersModel> PayOrders { get; set; }
        public void Mapping(Profile profile)
        {

            profile.CreateMap<UpdateDocumentCommand, Payables_Documents>().IgnoreAllNonExisting();
            profile.CreateMap<DocumentAccountsModel, Payables_DocumentsAccounts>().IgnoreAllNonExisting();
        }
    }

    public class UpdateDocumentCommandHandler : IRequestHandler<UpdateDocumentCommand, ServiceResult<Payables_Documents>>
    {
        private readonly IMapper _mapper;
        private readonly IBursaryUnitOfWork _uow;

        public UpdateDocumentCommandHandler(IMapper mapper, IBursaryUnitOfWork uow)
        {
            _mapper = mapper;
            _uow = uow;
        }

        public async Task<ServiceResult<Payables_Documents>> Handle(UpdateDocumentCommand request, CancellationToken cancellationToken)
        {
            var document = _mapper.Map<Payables_Documents>(request);

            if (document == null)
            {
                throw new ValidationError("اطلاعات ورودی ارسال نگردیده است");
            }

            foreach (var i in request.Accounts)
            {
                var ent = new Payables_DocumentsAccounts();
                _mapper.Map(i, ent);
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

            var acnts = await _uow.Payables_DocumentsAccounts.AsQueryable().Where(w=>w.DocumentId == request.Id).ToListAsync();
            _uow.Payables_DocumentsAccounts.RemoveRange(acnts);

            var ords = await _uow.Payables_DocumentsPayOrders.AsQueryable().Where(w => w.DocumentId == request.Id).ToListAsync();
            if(ords != null) _uow.Payables_DocumentsPayOrders.RemoveRange(ords);

            _uow.Payables_Documents.Update(document);
            var value = await _uow.SaveChangesAsync(cancellationToken);
            if (value <= 0)
            {
                throw new Exception("بروز خطا در ثبت اطلاعات سند پرداختی");
            }
            return ServiceResult<Payables_Documents>.Success(document);
        }
    }
}
