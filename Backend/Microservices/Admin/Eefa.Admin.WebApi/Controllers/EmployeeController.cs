using System.Threading.Tasks;
using Eefa.Admin.Application.CommandQueries.Employee.Command.Create;
using Eefa.Admin.Application.CommandQueries.Employee.Command.Delete;
using Eefa.Admin.Application.CommandQueries.Employee.Command.Update;
using Eefa.Admin.Application.CommandQueries.Employee.Query.Get;
using Eefa.Admin.Application.CommandQueries.Employee.Query.GetAll;
using Eefa.Admin.Application.CommandQueries.Employee.Query.GetByPersonId;
using Eefa.Admin.Application.UseCases.Employee.Command.UpdateEmployeesFromSina;
using Microsoft.AspNetCore.Mvc;

namespace Eefa.Admin.WebApi.Controllers
{
    public class EmployeeController : AdminBaseController
    {

        [HttpGet]
        //[Authorize(Roles = "Employees-*,Employees-Get")]
        public async Task<IActionResult> Get([FromQuery] GetEmployeeQuery model) => Ok(await Mediator.Send(model));

        [HttpGet]
        //[Authorize(Roles = "Employees-*,Employees-GetByPersonId")]
        public async Task<IActionResult> GetByPersonId([FromQuery] GetByPersonIdQuery model) => Ok(await Mediator.Send(model));

        [HttpPost]
        //[Authorize(Roles = "Employees-*,Employees-GetAll")]
        public async Task<IActionResult> GetAll([FromBody] GetAllEmployeeQuery model) => Ok(await Mediator.Send(model));



        [HttpPost]
        //[Authorize(Roles = "Employees-*,Employees-Add")]
        public async Task<IActionResult> Add([FromBody] CreateEmployeeCommand model) => Ok(await Mediator.Send(model));

        [HttpPut]
        //[Authorize(Roles = "Employees-*,Employees-Update")]
        public async Task<IActionResult> Update([FromBody] UpdateEmployeeCommand model) => Ok(await Mediator.Send(model));

        [HttpDelete]
        //[Authorize(Roles = "Employees-*,Employees-Delete")]
        public async Task<IActionResult> Delete([FromQuery] int id) => Ok(await Mediator.Send(new DeleteEmployeeCommand { Id = id }));

        [HttpPost]
        //[Authorize(Roles = "Employees-*,Employees-Delete")]
        public async Task<IActionResult> UpdateEmployeesFromSina() => Ok(await Mediator.Send(new UpdateEmployeesFromSinaCommand()));
    }
}
