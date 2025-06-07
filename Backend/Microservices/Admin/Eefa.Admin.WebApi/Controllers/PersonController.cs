using System.Threading.Tasks;
using Eefa.Admin.Application.CommandQueries.Person.Command.Create;
using Eefa.Admin.Application.CommandQueries.Person.Command.Delete;

using Eefa.Admin.Application.CommandQueries.Person.Command.Update;
using Eefa.Admin.Application.CommandQueries.Person.Query.Get;
using Eefa.Admin.Application.CommandQueries.Person.Query.GetAll;
using Eefa.Admin.Application.UseCases.Person.Command.Update;
using Eefa.Admin.Application.UseCases.Person.Query.GetCentralBankReport;
using Microsoft.AspNetCore.Mvc;

namespace Eefa.Admin.WebApi.Controllers
{
    public class PersonController : AdminBaseController
    {
        [HttpGet]
        //[Authorize(Roles = "Persons-*,Persons-Get")]
        public async Task<IActionResult> Get([FromQuery] GetPersonQuery model) => Ok(await Mediator.Send(model));

        [HttpPost]
        //[Authorize(Roles = "Persons-*,Persons-GetAll")]
        public async Task<IActionResult> GetAll([FromBody] GetAllPersonQuery model) => Ok(await Mediator.Send(model));

        //[HttpPost]
        ////[Authorize(Roles = "Persons-*,Persons-GetAll")]
        //public async Task<IActionResult> GetCentralBankReport([FromBody] GetCentralBankReportQuery model) => Ok(await Mediator.Send(model));

        [HttpPost]
        //[Authorize(Roles = "Persons-*,Persons-Add")]
        public async Task<IActionResult> Add([FromBody] CreatePersonCommand model) => Ok(await Mediator.Send(model));

        [HttpPut]
        //[Authorize(Roles = "Persons-*,Persons-Update")]
        public async Task<IActionResult> Update([FromBody] UpdatePersonCommand model) => Ok(await Mediator.Send(model));

        [HttpPut]
        //[Authorize(Roles = "Persons-*,Persons-Update")]
        public async Task<IActionResult> SetDepositIdForPeopleByReferenceCode([FromBody] UpdateDepositPersonCommand model) => Ok(await Mediator.Send(model));

        [HttpDelete]
        //[Authorize(Roles = "Persons-*,Persons-Delete")]
        public async Task<IActionResult> Delete([FromQuery] int id) => Ok(await Mediator.Send(new DeletePersonCommand { Id = id }));

    }
}
