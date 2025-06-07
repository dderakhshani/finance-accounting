using System.Threading.Tasks;
using Eefa.Admin.Application.CommandQueries.PersonAddress.Command.Create;
using Eefa.Admin.Application.CommandQueries.PersonAddress.Command.Delete;
using Eefa.Admin.Application.CommandQueries.PersonAddress.Command.Update;
using Eefa.Admin.Application.CommandQueries.PersonAddress.Query.Get;
using Eefa.Admin.Application.CommandQueries.PersonAddress.Query.GetAll;
using Microsoft.AspNetCore.Mvc;

namespace Eefa.Admin.WebApi.Controllers
{
    public class PersonAddressController : AdminBaseController
    {
        [HttpGet]
        //[Authorize(Roles = "PersonAddresses-*,PersonAddresses-Get")]
        public async Task<IActionResult> Get([FromQuery] GetPersonAddressQuery model) => Ok(await Mediator.Send(model));

        [HttpPost]
        //[Authorize(Roles = "PersonAddresses-*,PersonAddresses-GetAll")]
        public async Task<IActionResult> GetAll([FromBody] GetAllPersonAddressQuery model) => Ok(await Mediator.Send(model));

        [HttpPost]
        //[Authorize(Roles = "PersonAddresses-*,PersonAddresses-Add")]
        public async Task<IActionResult> Add([FromBody] CreatePersonAddressCommand model) => Ok(await Mediator.Send(model));

        [HttpPut]
        //[Authorize(Roles = "PersonAddresses-*,PersonAddresses-Update")]
        public async Task<IActionResult> Update([FromBody] UpdatePersonAddressCommand model) => Ok(await Mediator.Send(model));

        [HttpDelete]
        //[Authorize(Roles = "PersonAddresses-*,PersonAddresses-Delete")]
        public async Task<IActionResult> Delete([FromQuery] int id) => Ok(await Mediator.Send(new DeletePersonAddressCommand{Id = id }));

    }
}
