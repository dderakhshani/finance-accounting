using Eefa.Admin.Application.CommandQueries.PersonPhones.Commands.Create;
using Eefa.Admin.Application.CommandQueries.PersonPhones.Commands.Delete;
using Eefa.Admin.Application.CommandQueries.PersonPhones.Commands.Update;
using Eefa.Admin.Application.CommandQueries.PersonPhones.Queries.Get;
using Eefa.Admin.Application.CommandQueries.PersonPhones.Queries.GetAll;

using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Eefa.Admin.WebApi.Controllers
{
    public class PersonPhonesController : AdminBaseController
    {
        [HttpGet]
        //[Authorize(Roles = "PersonPhones-*,PersonPhones-Get")]
        public async Task<IActionResult> Get([FromQuery] GetPersonPhoneQuery model) => Ok(await Mediator.Send(model));

        [HttpPost]
        //[Authorize(Roles = "PersonPhones-*,PersonPhones-GetAll")]
        public async Task<IActionResult> GetAll([FromBody] GetAllPersonPhonesQuery model) => Ok(await Mediator.Send(model));

        [HttpPost]
        //[Authorize(Roles = "PersonPhones-*,PersonPhones-Add")]
        public async Task<IActionResult> Add([FromBody] CreatePersonPhoneCommand model) => Ok(await Mediator.Send(model));

        [HttpPut]
        //[Authorize(Roles = "PersonPhones-*,PersonPhones-Update")]
        public async Task<IActionResult> Update([FromBody] UpdatePersonPhoneCommand model) => Ok(await Mediator.Send(model));

        [HttpDelete]
        //[Authorize(Roles = "PersonPhones-*,PersonPhones-Delete")]
        public async Task<IActionResult> Delete([FromQuery] int id) => Ok(await Mediator.Send(new DeletePersonPhoneCommand { Id = id }));

    }
}
