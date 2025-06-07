using Eefa.Bursary.Application.Commands.FinancialRequestAttachment.Add;
using Eefa.Bursary.Application.Queries.FinancialRequestAttachment;
using Eefa.Common.Data.Query;
using Eefa.Common.Web;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Eefa.Bursary.WebApi.Controllers
{
    public class FinancialRequestAttachmentController : ApiControllerBase
    {
        private readonly IFinancialRequestAttachmentQuery financialRequestAttachmentQuery;

        public FinancialRequestAttachmentController(IFinancialRequestAttachmentQuery financialRequestAttachmentQuery)
        {
            this.financialRequestAttachmentQuery = financialRequestAttachmentQuery;
        }

        [HttpPost]
        public async Task<IActionResult> GetAll([FromBody] PaginatedQueryModel paginatedQuery) => Ok(await financialRequestAttachmentQuery.GetAll(paginatedQuery));

        [HttpPost]
        public async Task<IActionResult> SetAttachmentForChequeSheet([FromBody] CreateAttachmentForChequeSheetCommand model) => Ok(await Mediator.Send(model));
    }
}
