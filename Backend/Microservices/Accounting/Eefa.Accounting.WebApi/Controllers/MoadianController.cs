using Eefa.Accounting.Application.UseCases.Moadian.MoadianInvoiceHeader.Commands.Create;
using Eefa.Accounting.Application.UseCases.Moadian.MoadianInvoiceHeader.Commands.ImportByExcel;
using Eefa.Accounting.Application.UseCases.Moadian.MoadianInvoiceHeader.Commands.InquiryInvoices;
using Eefa.Accounting.Application.UseCases.Moadian.MoadianInvoiceHeader.Commands.Submit;
using Eefa.Accounting.Application.UseCases.Moadian.MoadianInvoiceHeader.Commands.Update;
using Eefa.Accounting.Application.UseCases.Moadian.MoadianInvoiceHeader.Commands.UpdateInvoicesStatusByIds;
using Eefa.Accounting.Application.UseCases.Moadian.MoadianInvoiceHeader.Queries.Get;
using Eefa.Accounting.Application.UseCases.Moadian.MoadianInvoiceHeader.Queries.GetAll;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Eefa.Accounting.WebApi.Controllers
{
    public class MoadianController : AccountingBaseController
    {


        [HttpGet]
        //[Authorize(Roles = "VouchersHead-*,VouchersHead-Get")]
        public async Task<IActionResult> GetInvoice([FromQuery] GetMoadianInvoiceHeaderQuery model) => Ok(await Mediator.Send(model));

        [HttpPost]
        //[Authorize(Roles = "VouchersHead-*,VouchersHead-GetAll")]
        public async Task<IActionResult> GetAllInvoices([FromBody] GetAllMoadianInvoiceHeadersQuery model) => Ok(await Mediator.Send(model));

        [HttpPost]
        //[Authorize(Roles = "VouchersHead-*,VouchersHead-Add")]
        public async Task<IActionResult> AddInvoice([FromBody] CreateMoadianInvoiceHeaderCommand model) => Ok(await Mediator.Send(model));

        [HttpPut]
        //[Authorize(Roles = "VouchersHead-*,VouchersHead-Update")]
        public async Task<IActionResult> UpdateInvoice([FromBody] UpdateMoadianInvoiceHeaderCommand model) => Ok(await Mediator.Send(model));
        [HttpPut]
        public async Task<IActionResult> UpdateInvoicesStatusByIds([FromBody] UpdateInvoicesStatusByIdsCommand model) => Ok(await Mediator.Send(model));

        [HttpPost]
        //[Authorize(Roles = "VouchersHead-*,VouchersHead-Add")]
        public async Task<IActionResult> ImportInvoicesByExcel([FromForm] ImportByExcelMoadianInvoiceHeaderCommand model) => Ok(await Mediator.Send(model));

        [HttpPost]
        //[Authorize(Roles = "VouchersHead-*,VouchersHead-Add")]
        public async Task<IActionResult> SubmitInvoices([FromBody] SubmitMoadianInvoiceHeaderCommand model) => Ok(await Mediator.Send(model));

        [HttpPost]
        //[Authorize(Roles = "VouchersHead-*,VouchersHead-Add")]
        public async Task<IActionResult> InquiryInvoices([FromBody] InquiryInvoicesCommand model) => Ok(await Mediator.Send(model));

    }
}
