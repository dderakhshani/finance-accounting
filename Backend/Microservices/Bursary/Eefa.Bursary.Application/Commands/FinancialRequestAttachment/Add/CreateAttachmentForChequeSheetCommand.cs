using AutoMapper;
using Eefa.Bursary.Domain.Entities;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using Eefa.Common.Data;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application.Commands.FinancialRequestAttachment.Add
{
    public class CreateAttachmentForChequeSheetCommand : Common.CommandQuery.CommandBase, IRequest<ServiceResult>, IMapFrom<CreateAttachmentForChequeSheetCommand>, ICommand
    {
        public List<FinancialAttachmentModel> Attachments { get; set; }

  

        public class CreateAttachmentForChequeSheetCommandHandler : IRequestHandler<CreateAttachmentForChequeSheetCommand, ServiceResult>
        {
            private readonly IMapper _autoMaper;
            private readonly IRepository<Domain.Entities.ChequeSheets> _chequeSheetRepository;
            private readonly IRepository<FinancialRequestAttachments> _financialRequestAttachmentRepository;


            public CreateAttachmentForChequeSheetCommandHandler(IMapper autoMaper, IRepository<Domain.Entities.ChequeSheets> chequeSheetRepository, IRepository<FinancialRequestAttachments> financialRequestAttachmentRepository)
            {
                _autoMaper = autoMaper;
                _chequeSheetRepository = chequeSheetRepository;
                _financialRequestAttachmentRepository = financialRequestAttachmentRepository;
            }

            public async Task<ServiceResult> Handle(CreateAttachmentForChequeSheetCommand request, CancellationToken cancellationToken)
            {

                foreach(var attachment in request.Attachments)
                {
                    var financialAttachment = new FinancialRequestAttachments()
                    {
                        AttachmentId = (int)attachment.AttachmentId, // AttachmentId come from front 
                        FinancialRequestId = (int)attachment.FinancialRequestId,
                        IsVerified = false,
                        IsDeleted = false,
                        ChequeSheetId = attachment.ChequeSheetId
                    };
                    _financialRequestAttachmentRepository.Insert(financialAttachment);
                }

                await _financialRequestAttachmentRepository.SaveChangesAsync(cancellationToken);

                return ServiceResult.Success();
            }
        }
    }
}
