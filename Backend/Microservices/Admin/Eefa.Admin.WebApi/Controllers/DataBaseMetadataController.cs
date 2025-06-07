using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Eefa.Admin.Application.CommandQueries.DataBaseMetadata.Query;

namespace Eefa.Admin.WebApi.Controllers
{
    public class DataBaseMetadataController : AdminBaseController
    {
        [HttpPost]
        //[Authorize(Roles = "DataBaseMetadata-*,DataBaseMetadata-GetAll")]
        public async Task<IActionResult> GetAll([FromBody] GetAllDataBaseMetadataQuery model) => Ok(await Mediator.Send(model));

    }
}
