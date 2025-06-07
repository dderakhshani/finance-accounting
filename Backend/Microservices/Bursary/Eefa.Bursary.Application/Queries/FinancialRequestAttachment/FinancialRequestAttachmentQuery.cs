using Eefa.Bursary.Domain.Entities;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application.Queries.FinancialRequestAttachment
{
    public class FinancialRequestAttachmentQuery : IFinancialRequestAttachmentQuery
    {

        private readonly IRepository<FinancialRequestAttachments> _financialAttachmentRepository;
        private readonly IRepository<Attachment> _attachmentRepository;

        public FinancialRequestAttachmentQuery(IRepository<FinancialRequestAttachments> financialAttachmentRepository, IRepository<Attachment> attachmentRepository)
        {
            _financialAttachmentRepository = financialAttachmentRepository;
            _attachmentRepository = attachmentRepository;
        }

        public async Task<PagedList<FinancialAttachmentModel>> GetAll(PaginatedQueryModel query)
        {
            var attachemtns =await (from fa in _financialAttachmentRepository.GetAll()
                               join att in _attachmentRepository.GetAll() on fa.AttachmentId equals att.Id
                               select new FinancialAttachmentModel
                               {
                                   Id = fa.Id,
                                   AddressUrl = att.Url,
                                   AttachmentId = fa.AttachmentId,
                                   FinancialRequestId = fa.FinancialRequestId,
                                   isVerified = fa.IsVerified,
                                   ChequeSheetId = fa.ChequeSheetId
                                   
                               }).FilterQuery(query.Conditions).OrderByMultipleColumns(query.OrderByProperty).ToListAsync();

            return new PagedList<FinancialAttachmentModel>()
            {
                Data =  attachemtns,
                TotalCount = attachemtns.Count,
                TotalSum = 0
            };
        }
    }
}
