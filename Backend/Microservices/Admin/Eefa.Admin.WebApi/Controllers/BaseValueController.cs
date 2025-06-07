using System.Threading.Tasks;
using Eefa.Admin.Application.CommandQueries.BaseValue.Command.Create;
using Eefa.Admin.Application.CommandQueries.BaseValue.Command.Delete;
using Eefa.Admin.Application.CommandQueries.BaseValue.Command.Update;
using Eefa.Admin.Application.CommandQueries.BaseValue.Query.Get;
using Eefa.Admin.Application.CommandQueries.BaseValue.Query.GetAll;
using Eefa.Admin.Application.CommandQueries.BaseValue.Query.GetAllByCategoryUniqueName;
using Microsoft.AspNetCore.Mvc;

namespace Eefa.Admin.WebApi.Controllers
{
    public class BaseValueController : AdminBaseController
    {
        [HttpGet]
        //[Authorize(Roles = "BaseValues-*,BaseValue-Get")]
        public async Task<IActionResult> Get([FromQuery] GetBaseValueQuery model) => Ok(await Mediator.Send(model));

        [HttpPost]
        //[Authorize(Roles = "BaseValues-*,BaseValue-GetAll")]
        public async Task<IActionResult> GetAll([FromBody] GetAllBaseValueQuery model) => Ok(await Mediator.Send(model));

        [HttpGet]
        //[Authorize(Roles = "BaseValues-*,BaseValue-GetAllByCategoryUniqueName")]
        public async Task<IActionResult> GetAllByCategoryUniqueName([FromQuery] GetAllByCategoryUniqueNameQuery model) => Ok(await Mediator.Send(model));

        [HttpPost]
        //[Authorize(Roles = "BaseValues-*,BaseValue-Add")]
        public async Task<IActionResult> Add([FromBody] CreateBaseValueCommand model) => Ok(await Mediator.Send(model));

        [HttpPut]
        //[Authorize(Roles = "BaseValues-*,BaseValue-Update")]
        public async Task<IActionResult> Update([FromBody] UpdateBaseValueCommand model) => Ok(await Mediator.Send(model));

        [HttpDelete]
        //[Authorize(Roles = "BaseValues-*,BaseValue-Delete")]
        public async Task<IActionResult> Delete([FromQuery] int id) => Ok(await Mediator.Send(new DeleteBaseValueCommand{Id = id}));

    }
}
