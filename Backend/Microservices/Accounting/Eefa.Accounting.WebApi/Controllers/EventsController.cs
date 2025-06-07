using Eefa.Accounting.Application.Services.EventManager;
using Eefa.Accounting.Application.UseCases.AccountHead.Query.Get;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Eefa.Accounting.WebApi.Controllers
{
    public class EventsController : AccountingBaseController
    {
        private readonly IApplicationEventsManager applicationEventsManager;

        public EventsController(IApplicationEventsManager applicationEventsManager)
        {
            this.applicationEventsManager = applicationEventsManager;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int entityId, [FromQuery] string entityType) => Ok(await applicationEventsManager.GetEvents(entityId,entityType));
    }
}
