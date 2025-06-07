using System.Threading.Tasks;
using Eefa.Admin.Application.CommandQueries.BaseValueType.Command.Create;
using Eefa.Admin.Application.CommandQueries.BaseValueType.Command.Delete;
using Eefa.Admin.Application.CommandQueries.BaseValueType.Command.Update;
using Eefa.Admin.Application.CommandQueries.BaseValueType.Query.Get;
using Eefa.Admin.Application.CommandQueries.BaseValueType.Query.GetAll;
using Microsoft.AspNetCore.Mvc;

namespace Eefa.Admin.WebApi.Controllers
{
    public class BaseValueTypeController : AdminBaseController
    {
        [HttpGet]
        //[Authorize(Roles = "BaseValueTypes-*,BaseValueTypes-Get")]
        public async Task<IActionResult> Get([FromQuery] GetBaseValueTypeQuery model) => Ok(await Mediator.Send(model));

        [HttpPost]
        //[Authorize(Roles = "BaseValueTypes-*,BaseValueTypes-GetAll")]
        public async Task<IActionResult> GetAll([FromBody] GetAllBaseValueTypeQuery model) => Ok(await Mediator.Send(model));

        [HttpPost]
        //[Authorize(Roles = "BaseValueTypes-*,BaseValueTypes-Add")]
        public async Task<IActionResult> Add([FromBody] CreateBaseValueTypeCommand model) => Ok(await Mediator.Send(model));

        [HttpPut]
        //[Authorize(Roles = "BaseValueTypes-*,BaseValueTypes-Update")]
        public async Task<IActionResult> Update([FromBody] UpdateBaseValueTypeCommand model) => Ok(await Mediator.Send(model));

        [HttpDelete]
        //[Authorize(Roles = "BaseValueTypes-*,BaseValueTypes-Delete")]
        public async Task<IActionResult> Delete([FromQuery] int id) => Ok(await Mediator.Send(new DeleteBaseValueTypeCommand{Id = id}));

    }
}
