using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Eefa.Admin.Application.CommandQueries.CompanyInformation.Command.Create;
using Eefa.Admin.Application.CommandQueries.CompanyInformation.Command.Delete;
using Eefa.Admin.Application.CommandQueries.CompanyInformation.Command.Update;
using Eefa.Admin.Application.CommandQueries.CompanyInformation.Query.Get;
using Eefa.Admin.Application.CommandQueries.CompanyInformation.Query.GetAll;

namespace Eefa.Admin.WebApi.Controllers
{
    public class CompanyInformationController : AdminBaseController
    {
        [HttpGet]
        //[Authorize(Roles = "CompanyInformations-*,CompanyInformations-Get")]
        public async Task<IActionResult> Get([FromQuery] GetCompanyInformationQuery model) => Ok(await Mediator.Send(model));

        [HttpPost]
        //[Authorize(Roles = "CompanyInformations-*,CompanyInformations-GetAll")]
        public async Task<IActionResult> GetAll([FromBody] GetAllowedUserCompanyByUserIdQuery model) => Ok(await Mediator.Send(model));

        [HttpPost]
        //[Authorize(Roles = "CompanyInformations-*,CompanyInformations-Add")]
        public async Task<IActionResult> Add([FromBody] CreateCompanyInformationCommand model) => Ok(await Mediator.Send(model));

        [HttpPut]
        //[Authorize(Roles = "CompanyInformations-*,CompanyInformations-Update")]
        public async Task<IActionResult> Update([FromBody] UpdateCompanyInformationCommand model) => Ok(await Mediator.Send(model));

        [HttpDelete]
        //[Authorize(Roles = "CompanyInformations-*,CompanyInformations-Delete")]
        public async Task<IActionResult> Delete([FromQuery] int id) => Ok(await Mediator.Send(new DeleteCompanyInformationCommand{Id = id }));


    }
}
