using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Eefa.Common;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;
using Eefa.Common.Web;
using Eefa.Purchase.Application.Commands.Invoice.ArchiveCotract;
using Eefa.Purchase.Application.Commands.Invoice.ArchiveFactory;
using Eefa.Purchase.Application.Commands.Invoice.Create;
using Eefa.Purchase.Application.Commands.Invoice.CreateMultiple;
using Eefa.Purchase.Application.Commands.Invoice.Update;
using Eefa.Purchase.Application.Models;
using Eefa.Purchase.Application.Models.Receipt;
using Eefa.Purchase.Application.Queries.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace Eefa.Purchase.WebApi.Controllers.Invoice
{
    public class InvoiceController : ApiControllerBase
    {
        IInvoiceQueries _InvoiceQueries;


        public InvoiceController(IInvoiceQueries InvoiceQueries
            )
        {
            _InvoiceQueries = InvoiceQueries ?? throw new ArgumentNullException(nameof(InvoiceQueries));



        }

        [HttpGet]
        //[Authorize(Roles = "Invoice-*,Invoice-Get")]
        public async Task<IActionResult> Get(int id)
        {

            var result = await _InvoiceQueries.GetById(id);
            return Ok(ServiceResult<InvoiceQueryModel>.Success(result));
        }
        [HttpPost]
        public async Task<IActionResult> GetByListId(Rootobject model)
        {

            var result = await _InvoiceQueries.GetByListId(model.ListId);
            return Ok(ServiceResult<InvoiceQueryModel>.Success(result));
        }
        

        [HttpPost]

        public async Task<IActionResult> GetAll(
            int codeVoucherGroupId,
            bool? IsImportPurchase,
            DateTime? FromDate,
            DateTime? ToDate,
            PaginatedQueryModel paginatedQuery)
        {
            var result = await _InvoiceQueries.GetAll(codeVoucherGroupId, IsImportPurchase, FromDate, ToDate, paginatedQuery);

            return Ok(ServiceResult<PagedList<InvoiceQueryModel>>.Success(result));
        }
        [HttpPost]

        public async Task<IActionResult> GetInvoiceActiveRequestNo(
            string FromDate,
            string ToDate,
            int RequestNo,
            int ReferenceId=default
            )
        {
            var result = await _InvoiceQueries.GetInvoiceActiveRequestNo(RequestNo, ReferenceId,FromDate, ToDate);

            return Ok(ServiceResult<PagedList<InvoiceQueryModel>>.Success(result));
        }
        [HttpPost]

        public async Task<IActionResult> GetInvoiceActiveCommodityId(
            string FromDate,
            string ToDate,
            int CommodityId,
            int ReferenceId = default
            )
        {
            var result = await _InvoiceQueries.GetInvoiceActiveCommodityId(CommodityId, ReferenceId, FromDate, ToDate);

            return Ok(ServiceResult<PagedList<InvoiceQueryModel>>.Success(result));
        }

        [HttpPost]
        public async Task<IActionResult> GetInvoiceALLStatus()
        {
            var result = await _InvoiceQueries.GetInvoiceALLVoucherGroup();
            return Ok(ServiceResult<PagedList<InvoiceALLStatusModel>>.Success(result));

        }

        /// <summary>
        /// ثبت چندتایی 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddInvoiceMultiple([FromBody] CreateInvoiceMultipleCommand model) => Ok(await Mediator.Send(model));

        /// <summary>
        /// ثبت یکی
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateCommand model) => Ok(await Mediator.Send(model));


        [HttpDelete]
        public async Task<IActionResult> ArchiveContract(int id)
        {

            return Ok(await Mediator.Send(new ArchiveContractCommand() { Id = id }));
        }
        [HttpDelete]
        public async Task<IActionResult> ArchiveFactor(int id)
        {

            return Ok(await Mediator.Send(new ArchiveFactoryCommand() { Id = id }));
        }
        [HttpPost]
        public async Task<IActionResult> update([FromBody] UpdateInvoiceCommand model) => Ok(await Mediator.Send(model));

        public class Rootobject
        {
            public int state { get; set; }
            public string requestId { get; set; }
            public int menueId { get; set; }
            public List<int> ListId { get; set; }
        }
        

    }
}