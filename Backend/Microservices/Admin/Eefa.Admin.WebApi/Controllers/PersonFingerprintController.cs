using System.Threading.Tasks;
using Eefa.Admin.Application.CommandQueries.PersonFingerprint.Command.Create;
using Eefa.Admin.Application.CommandQueries.PersonFingerprint.Command.Delete;
using Eefa.Admin.Application.CommandQueries.PersonFingerprint.Command.Update;
using Eefa.Admin.Application.CommandQueries.PersonFingerprint.Query.Get;
using Eefa.Admin.Application.CommandQueries.PersonFingerprint.Query.GetAll;
using Microsoft.AspNetCore.Mvc;

namespace Eefa.Admin.WebApi.Controllers
{
    public class PersonFingerprintController : AdminBaseController
    {
        [HttpGet]
        //[Authorize(Roles = "PersonFingerprints-*,PersonFingerprints-Get")]
        public async Task<IActionResult> Get([FromQuery] GetPersonFingerprintQuery model) => Ok(await Mediator.Send(model));

        [HttpPost]
        //[Authorize(Roles = "PersonFingerprints-*,PersonFingerprints-GetAll")]
        public async Task<IActionResult> GetAll([FromBody] GetAllPersonFingerprintQuery model) => Ok(await Mediator.Send(model));

        [HttpPost]
        //[Authorize(Roles = "PersonFingerprints-*,PersonFingerprints-Add")]
        public async Task<IActionResult> Add([FromBody] CreatePersonFingerprintCommand model) => Ok(await Mediator.Send(model));

        [HttpPut]
        //[Authorize(Roles = "PersonFingerprints-*,PersonFingerprints-Update")]
        public async Task<IActionResult> Update([FromBody] UpdatePersonFingerprintCommand model) => Ok(await Mediator.Send(model));

        [HttpDelete]
        //[Authorize(Roles = "PersonFingerprints-*,PersonFingerprints-Delete")]
        public async Task<IActionResult> Delete([FromQuery] int id) => Ok(await Mediator.Send(new DeletePersonFingerprintCommand{Id = id }));


    }
}
