using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Eefa.Admin.Application.CommandQueries.Year.Command.Create;
using Eefa.Admin.Application.CommandQueries.Year.Command.Delete;
using Eefa.Admin.Application.CommandQueries.Year.Command.Update;
using Eefa.Admin.Application.CommandQueries.Year.Query.Get;
using Eefa.Admin.Application.CommandQueries.Year.Query.GetAll;

namespace Eefa.Admin.WebApi.Controllers
{
    public class YearController : AdminBaseController
    {
        [HttpGet]
        //[Authorize(Roles = "Years-*,Years-Get")]
        public async Task<IActionResult> Get([FromQuery] GetYearQuery model) => Ok(await Mediator.Send(model));

        [HttpPost]
        //[Authorize(Roles = "Years-*,Years-GetAll")]
        public async Task<IActionResult> GetAll([FromBody] GetAllYearQuery model) => Ok(await Mediator.Send(model));

        [HttpPost]
        //[Authorize(Roles = "Years-*,Years-Add")]
        public async Task<IActionResult> Add([FromBody] CreateYearCommand model) => Ok(await Mediator.Send(model));

        [HttpPut]
        //[Authorize(Roles = "Years-*,Years-Update")]
        public async Task<IActionResult> Update([FromBody] UpdateYearCommand model) => Ok(await Mediator.Send(model));

        [HttpDelete]
        //[Authorize(Roles = "Years-*,Years-Delete")]
        public async Task<IActionResult> Delete([FromQuery] int id) => Ok(await Mediator.Send(new DeleteYearCommand{Id = id}));
    }
}
