using System.Threading.Tasks;
using Eefa.WorkflowAdmin.WebApi.Application;
using Library.Common;
using Microsoft.AspNetCore.Mvc;

namespace Eefa.WorkflowAdmin.WebApi.Controllers
{
    public class HomeController : WorkflowAdminBaseController
    {
        private readonly ICorrectionRequestService _correctionRequestService;

        public HomeController(ICorrectionRequestService correctionRequestService)
        {
            _correctionRequestService = correctionRequestService;
        }

        [HttpGet]
        public async Task<IActionResult> GetMyTasks(int id)
        {
            return Ok(await _correctionRequestService.GetUserTasks(id));
        }

        [HttpGet]
        public async Task<IActionResult> UpdateTask(int correctionRequestId, short status, string description)
        {
             await _correctionRequestService.UpdateCorrectionRequest(correctionRequestId, status, description);
            return Ok();
        }
    }
}