using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Eefa.Admin.Application.CommandQueries.CountryDivision.Query.GetAll;
using Eefa.Admin.Application.CommandQueries.CountryDivision.Query.GetAllCitiesQuery;
using Eefa.Admin.Application.CommandQueries.CountryDivision.Query.GetAllStatesQuery;

namespace Eefa.Admin.WebApi.Controllers
{
    public class CountryDivisionController : AdminBaseController
    {
        [HttpGet]
        //[Authorize(Roles = "CountryDivisions-*,CountryDivisions-GetAllStates")]
        public async Task<IActionResult> GetAllStates([FromQuery] GetAllStatesQuery model) => Ok(await Mediator.Send(model));

        [HttpGet]
        //[Authorize(Roles = "CountryDivisions-*,CountryDivisions-GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] GetAllQuery model) => Ok(await Mediator.Send(model));

        [HttpGet]
        //[Authorize(Roles = "CountryDivisions-*,CountryDivisions-GetAllCities")]
        public async Task<IActionResult> GetAllCities([FromQuery] GetAllCitiesQuery model) => Ok(await Mediator.Send(model));
    }
}
