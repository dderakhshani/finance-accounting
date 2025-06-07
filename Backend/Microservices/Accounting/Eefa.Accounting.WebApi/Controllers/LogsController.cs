using Eefa.Accounting.Application.Services.EventManager;
using Eefa.Accounting.Application.Services.Logs;
using Eefa.Accounting.Application.UseCases.AccountHead.Query.Get;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Eefa.Accounting.WebApi.Controllers
{
    public class LogsController : AccountingBaseController
    {
        private readonly IApplicationRequestLogManager logManager;

        public LogsController(IApplicationRequestLogManager logManager)
        {
            this.logManager = logManager;
        }
        [HttpPost]
        public async Task<IActionResult> GetAll([FromBody] GetLogsQuery query) => Ok(await logManager.GetAll(query));
    }
}
