using System.Threading.Tasks;
using Eefa.Admin.Application.CommandQueries.User.Command.Create;
using Eefa.Admin.Application.CommandQueries.User.Command.Delete;
using Eefa.Admin.Application.CommandQueries.User.Command.Update;
using Eefa.Admin.Application.CommandQueries.User.Query.Get;
using Eefa.Admin.Application.CommandQueries.User.Query.GetAll;
using Eefa.Admin.Application.CommandQueries.User.Query.GetAllByRoleId;
using Eefa.Admin.Application.CommandQueries.User.Query.GetUsers;
using Microsoft.AspNetCore.Mvc;

namespace Eefa.Admin.WebApi.Controllers
{
    public class UserController : AdminBaseController
    {
        [HttpGet]
        //[Authorize(Roles = "Users-*,Users-Get")]
        public async Task<IActionResult> Get([FromQuery] GetUserQuery model) => Ok(await Mediator.Send(model));

        [HttpPost]
        //[Authorize(Roles = "Users-*,Users-GetAll")]
        public async Task<IActionResult> GetAll([FromBody] GetAllUserQuery model) => Ok(await Mediator.Send(model)); 
        
        [HttpGet]
        //[Authorize(Roles = "Users-*,Users-GetAllByRoleId")]
        public async Task<IActionResult> GetAllByRoleId([FromQuery] GetAllUserByRoleQuery model) => Ok(await Mediator.Send(model));

        [HttpPost]
        //[Authorize(Roles = "Users-*,Users-Get")]
        public async Task<IActionResult> Add([FromBody] CreateUserCommand model) => Ok(await Mediator.Send(model));

        [HttpPut]
        //[Authorize(Roles = "Users-*,Users-Update")]
        public async Task<IActionResult> Update([FromBody] UpdateUserCommand model) => Ok(await Mediator.Send(model));

        [HttpDelete]
        //[Authorize(Roles = "Users-*,Users-Delete")]
        public async Task<IActionResult> Delete([FromQuery] int id) => Ok(await Mediator.Send(new DeleteUserCommand{Id = id}));

        [HttpPost]
        public async Task<Library.Models.ServiceResult> GetUsersByIds([FromBody] GetUsersByIdsQuery model)
        {
            return await Mediator.Send(model);
        }
    }
}
