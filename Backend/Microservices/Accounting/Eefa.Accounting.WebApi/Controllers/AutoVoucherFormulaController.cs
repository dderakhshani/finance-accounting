using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Eefa.Accounting.Application.UseCases.AutoVoucherFormula.Command.Create;
using Eefa.Accounting.Application.UseCases.AutoVoucherFormula.Command.Delete;
using Eefa.Accounting.Application.UseCases.AutoVoucherFormula.Command.Update;
using Eefa.Accounting.Application.UseCases.AutoVoucherFormula.Query.Get;
using Eefa.Accounting.Application.UseCases.AutoVoucherFormula.Query.GetAll;

namespace Eefa.Accounting.WebApi.Controllers
{
    public class AutoVoucherFormulaController : AccountingBaseController
    {
        [HttpGet]
        //[Authorize(Roles = "AutoVoucherFormula-*,AutoVoucherFormula-Get")]
        public async Task<IActionResult> Get([FromQuery] GetAutoVoucherFormulaQuery model) => Ok(await Mediator.Send(model));

        [HttpPost]
        //[Authorize(Roles = "AutoVoucherFormula-*,AutoVoucherFormula-GetAll")]
        public async Task<IActionResult> GetAll([FromBody] GetAllAutoVoucherFormulaQuery model) => Ok(await Mediator.Send(model));

        [HttpPost]
        //[Authorize(Roles = "AutoVoucherFormula-*,AutoVoucherFormula-Add")]
        public async Task<IActionResult> Add([FromBody] CreateAutoVoucherFormulaCommand model) => Ok(await Mediator.Send(model));

        [HttpPut]
        //[Authorize(Roles = "AutoVoucherFormula-*,AutoVoucherFormula-Update")]
        public async Task<IActionResult> Update([FromBody] UpdateAutoVoucherFormulaCommand model) => Ok(await Mediator.Send(model));

        [HttpDelete]
        //[Authorize(Roles = "AutoVoucherFormula-*,AutoVoucherFormula-Delete")]
        public async Task<IActionResult> Delete([FromQuery] int id) => Ok(await Mediator.Send(new DeleteAutoVoucherFormulaCommand{Id = id}));


    }
}
