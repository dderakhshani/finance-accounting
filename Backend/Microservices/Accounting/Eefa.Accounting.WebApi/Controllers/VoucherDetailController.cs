using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Eefa.Accounting.Application.UseCases.VouchersDetail.Command.Create;
using Eefa.Accounting.Application.UseCases.VouchersDetail.Command.Delete;
using Eefa.Accounting.Application.UseCases.VouchersDetail.Command.Update;
using Eefa.Accounting.Application.UseCases.VouchersDetail.Query.Get;
using Eefa.Accounting.Application.UseCases.VouchersDetail.Query.GetAll;

namespace Eefa.Accounting.WebApi.Controllers
{
    public class VoucherDetailController : AccountingBaseController
    {
    //    [HttpGet]
    //    //[Authorize(Roles = "VoucherDetail-*,VoucherDetail-Get")]
    //    public async Task<IActionResult> Get([FromQuery] GetVouchersDetailQuery model) => Ok(await Mediator.Send(model));

        [HttpPost]
        //[Authorize(Roles = "VoucherDetail-*,VoucherDetail-GetAll")]
        public async Task<IActionResult> GetAll([FromBody] GetAllVouchersDetailQuery model) => Ok(await Mediator.Send(model));

        //[HttpPost]
        ////[Authorize(Roles = "VoucherDetail-*,VoucherDetail-Add")]
        //public async Task<IActionResult> Add([FromBody] CreateVouchersDetailCommand model) => Ok(await Mediator.Send(model));

        //[HttpPut]
        ////[Authorize(Roles = "VoucherDetail-*,VoucherDetail-Update")]
        //public async Task<IActionResult> Update([FromBody] UpdateVouchersDetailCommand model) => Ok(await Mediator.Send(model));

        //[HttpDelete]
        ////[Authorize(Roles = "VoucherDetail-*,VoucherDetail-Delete")]
        //public async Task<IActionResult> Delete([FromQuery] int id) => Ok(await Mediator.Send(new DeleteVouchersDetailCommand{Id = id}));


    }
}
