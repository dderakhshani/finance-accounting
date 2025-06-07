using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using Eefa.Purchase.Application.Models;
using Eefa.Purchase.Infrastructure.Context;

namespace Eefa.Purchase.Application
{
    public class ProcedureCallService : IProcedureCallService
    {

        private readonly IMapper _mapper;
        private readonly PurchaseContext _context;
        private readonly IRepository<Purchase.Domain.Attachment> _repositoryAttachment;
        private readonly IRepository<Domain.Aggregates.InvoiceAggregate.DocumentAttachment> _repositoryDocumentAttachments;
        public ProcedureCallService(
              IMapper mapper
            , PurchaseContext context
            , IRepository<Purchase.Domain.Attachment> repositoryAttachment
            , IRepository<Domain.Aggregates.InvoiceAggregate.DocumentAttachment> repositoryDocumentAttachments


            )
        {
            _mapper = mapper;
            _context = context;
            _repositoryDocumentAttachments = repositoryDocumentAttachments;
            _repositoryAttachment = repositoryAttachment;

        }
        /// <summary>
        /// ترتیب پارامترهای ورودی برای پروسیجرها اهمیت دارد
        /// </summary>
        /// <param name="CommodityId"></param>
        /// <param name="DocumentItemsId"></param>
        /// <returns></returns>

        public async Task<SpDocumentItemsPriceBuy> GetPriceBuy(int CommodityId)
        {

            var model = new SpDocumentItemsPriceBuyParam() { CommodityId = CommodityId, DocumentStauseBaseValue = 0, DocumentItemsId = null };
            var parameters = model.EntityToSqlParameters();

            var PriceBuy = await _context.ExecuteSqlQueryAsync<SpDocumentItemsPriceBuy>($"EXEC [inventory].[spDocumentItemsPriceBuy] {QueryUtility.SqlParametersToQuey(parameters)}", parameters, new CancellationToken());
            return PriceBuy.FirstOrDefault();
        }
        public async Task ModifyDocumentAttachments(List<int> attachmentIds, int DocumentHeadId)
        {
            var DocumentId = _context.Document.Where(a => a.DocumentId == DocumentHeadId).Select(a => a.Id).FirstOrDefault();
            var AssetAttachments = await _repositoryDocumentAttachments.GetAll().Where(a => a.DocumentId == DocumentId).ToListAsync();
            // LIST OF DELETEDS
            var AssetAttachmentsDeleted = AssetAttachments.Where(a => attachmentIds.Contains(a.AttachmentId)).ToList();

            if (attachmentIds.Any())
            {

                foreach (var attachmentI in attachmentIds)
                {

                    var AssetAttachment = AssetAttachments.Where(a => a.AttachmentId == attachmentI).FirstOrDefault();

                    //UPDATE
                    var Attachment = await _repositoryAttachment.GetAll().Where(a => a.Id == attachmentI).FirstOrDefaultAsync();

                    //INSERT NEW
                    if (Attachment != null && AssetAttachment == null)
                    {
                        Attachment.IsUsed = true;
                        var attach = new Domain.Aggregates.InvoiceAggregate.DocumentAttachment()
                        {
                            AttachmentId = attachmentI,
                            DocumentId = DocumentId,

                        };
                        _repositoryDocumentAttachments.Insert(attach);
                    }

                }
                foreach (var Asset in AssetAttachmentsDeleted)
                {
                    _repositoryDocumentAttachments.Delete(Asset);
                }
                await _repositoryDocumentAttachments.SaveChangesAsync();

            }
        }

    }
}
