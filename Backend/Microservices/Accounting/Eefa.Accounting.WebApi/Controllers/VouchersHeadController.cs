using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Eefa.Accounting.Application.UseCases.VouchersHead.Command.Create;
using Eefa.Accounting.Application.UseCases.VouchersHead.Command.Delete;
using Eefa.Accounting.Application.UseCases.VouchersHead.Command.Update;
using Eefa.Accounting.Application.UseCases.VouchersHead.Query.Get;
using Eefa.Accounting.Application.UseCases.VouchersHead.Query.GetAll;
using Eefa.Accounting.Application.UseCases.VouchersHead.Query.GetAllBySpecificCondition;
using Eefa.Accounting.Application.UseCases.VouchersHead.Command.SubmitVoucherHeadCorrectionRequest;
using Eefa.Accounting.Application.UseCases.VouchersHead.Command.BulkVoucherStatusUpdate;
using Eefa.Accounting.Application.UseCases.VouchersHead.Command.Combine;
using Eefa.Accounting.Application.UseCases.VouchersHead.Command.MoveVoucherDetails;
using Eefa.Accounting.Application.UseCases.VouchersHead.Command.CreateAutoVoucher;
using Eefa.Accounting.Application.UseCases.VouchersDetail.Command.Delete;
using Eefa.Accounting.Application.UseCases.VouchersHead.Command.EndYearOperations.InsertBetween;
using Eefa.Accounting.Application.UseCases.VouchersHead.Command.EndYearOperations.Adjustment;
using Eefa.Accounting.Application.UseCases.VouchersHead.Command.EndYearOperations.StartVoucher;
using Eefa.Accounting.Application.UseCases.VouchersHead.Command.EndYearOperations.End;
using Eefa.Accounting.Application.UseCases.VouchersHead.Command.EndYearOperations.Close;
using Eefa.Accounting.Application.UseCases.VouchersHead.Command.OldOperations.VoucherNoRenumber;
using Eefa.Accounting.Application.UseCases.VouchersHead.Command.AutoVoucher;
using Eefa.Accounting.Application.UseCases.VouchersHead.Command.ConvertRaahKaranSalaryExcelToVoucherDetails;
using Microsoft.AspNetCore.Http;
using Eefa.Accounting.Application.UseCases.VouchersHead.Command.OldOperations.End;

namespace Eefa.Accounting.WebApi.Controllers
{
    public class VouchersHeadController : AccountingBaseController
    {


        [HttpPost]
        //[Authorize(Roles = "VouchersHead-*,VouchersHead-Add")]
        public async Task<IActionResult> Add([FromBody] CreateVouchersHeadCommand model) => Ok(await Mediator.Send(model));

        [HttpPut]
        //[Authorize(Roles = "VouchersHead-*,VouchersHead-Update")]
        public async Task<IActionResult> Update([FromBody] UpdateVouchersHeadCommand model) => Ok(await Mediator.Send(model));

        [HttpDelete]
        //[Authorize(Roles = "VouchersHead-*,VouchersHead-Delete")]
        public async Task<IActionResult> Delete([FromQuery] int id) => Ok(await Mediator.Send(new DeleteVouchersHeadCommand { Id = id }));

        [HttpDelete]
        //[Authorize(Roles = "VoucherDetail-*,VoucherDetail-Delete")]
        public async Task<IActionResult> DeleteByDocumentId([FromBody] DeleteVouchersDetailsByDocumentIdsCommand request) => Ok(await Mediator.Send(request));

        [HttpGet]
        //[Authorize(Roles = "VouchersHead-*,VouchersHead-Get")]
        public async Task<IActionResult> Get([FromQuery] GetVouchersHeadQuery model) => Ok(await Mediator.Send(model));

        [HttpPost]
        //[Authorize(Roles = "VouchersHead-*,VouchersHead-GetAll")]
        public async Task<IActionResult> GetAll([FromBody] GetAllVouchersHeadQuery model) => Ok(await Mediator.Send(model));

        [HttpPost]
        //[Authorize(Roles = "VouchersHead-*,VouchersHead-GetAllBySpecific")]
        public async Task<IActionResult> GetAllBySpecific([FromBody] GetAllBySpecificQuery model) => Ok(await Mediator.Send(model));

        [HttpPost]
        //[Authorize(Roles = "VouchersHead-*,VouchersHead-AddAutoVoucher")]
        public async Task<IActionResult> AutoVoucher([FromBody] AutoVoucherRefactoredCommand model) => Ok(await Mediator.Send(model));

        [HttpPost]
        //[Authorize(Roles = "VouchersHead-*,VouchersHead-AddAutoVoucher")]
        public async Task<IActionResult> AutoVoucherRefactored([FromBody] AutoVoucherRefactoredCommand model) => Ok(await Mediator.Send(model));

        [HttpPost]
        //[Authorize(Roles = "VouchersHead-*,VouchersHead-Renumber")]
        public async Task<IActionResult> Renumber([FromBody] CreateVoucherNoRenumberCommand model) => Ok(await Mediator.Send(model));

        [HttpPost]
        //[Authorize(Roles = "VouchersHead-*,VouchersHead-Combine")]
        public async Task<IActionResult> Combine([FromBody] CombineVoucherHeadsCommand model) => Ok(await Mediator.Send(model));

        [HttpPost]
        //[Authorize(Roles = "VouchersHead-*,VouchersHead-Combine")]
        public async Task<IActionResult> UpdateTaxpayerFlag([FromBody] UpdateTaxpayerFlagCommand model) => Ok(await Mediator.Send(model));

        [HttpPost]
        //[Authorize(Roles = "VouchersHead-*,VouchersHead-Combine")]
        public async Task<IActionResult> MoveVoucherDetails([FromBody] MoveVoucherDetailsCommand model) => Ok(await Mediator.Send(model));
        [HttpPost]
        //[Authorize(Roles = "VouchersHead-*,VouchersHead-Lock")]
        public async Task<IActionResult> BulkStatusUpdate([FromBody] BulkVoucherStatusUpdateCommand model) => Ok(await Mediator.Send(model));

        [HttpPost]
        //[Authorize(Roles = "VouchersHead-*,VouchersHead-InsertBetween")]
        public async Task<IActionResult> InsertBetween([FromBody] CreateInsertBetweenCommand model) => Ok(await Mediator.Send(model));

        [HttpPost]
        public async Task<IActionResult> SubmitVoucherHeadCorrectionRequest([FromBody] SubmitVoucherHeadCorrectionRequestCommand model) => Ok(await Mediator.Send(model));

        [HttpPost]
        public async Task<IActionResult> ConvertRaahKaranSalaryExcelToVoucherDetails([FromForm] ConvertRaahKaranSalaryExcelToVoucherDetailsCommand request) => Ok(await Mediator.Send(request));

        [HttpPost]
        public async Task<IActionResult> CloseVouchers([FromBody] CreateCloseVoucherCommand request) => Ok(await Mediator.Send(request));
       
        [HttpPost]
        public async Task<IActionResult> EndVouchersHead([FromBody] EndVoucherCommand request) => Ok(await Mediator.Send(request));

        [HttpPost]
        public async Task<IActionResult> StartVoucherHead([FromBody] CreateStartVoucherHeadCommand request) => Ok(await Mediator.Send(request));


    }
}
