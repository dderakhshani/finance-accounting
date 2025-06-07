using Eefa.Admin.Application.CommandQueries.CorrectionRequest.Commands.Create;
using Eefa.Admin.Application.CommandQueries.CorrectionRequest.Commands.Submit;
using Eefa.Admin.Application.CommandQueries.CorrectionRequest.Queries.GetAll;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Eefa.Admin.WebApi.Controllers
{
    public class CorrectionRequestController : AdminBaseController
    {
        [HttpPost]
        //[Authorize(Roles = "BaseValueTypes-*,BaseValueTypes-GetAll")]
        public async Task<IActionResult> GetAll([FromBody] GetAllCorrectionRequestsQuery model) => Ok(await Mediator.Send(model));

        [HttpPost]
        //[Authorize(Roles = "BaseValueTypes-*,BaseValueTypes-Add")]
        public async Task<IActionResult> Add([FromBody] CreateCorrectionRequestCommand model) => Ok(await Mediator.Send(model));

        [HttpPut]
        //[Authorize(Roles = "BaseValueTypes-*,BaseValueTypes-Update")]
        public async Task<IActionResult> Submit([FromBody] SubmitCorrectionRequestCommand model) => Ok(await Mediator.Send(model));
    }
}
