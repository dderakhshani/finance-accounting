using Eefa.Audit.Service.AuditMonitoring;
using Library.Common;
using Library.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Eefa.Audit.WebApi.Controllers
{
    [Route("api/admin/[controller]/[action]")]
    public class AuditBaseController : BaseController
    {
        private readonly IAuditMonitoringService _auditMonitoringService;

        public AuditBaseController(IAuditMonitoringService auditMonitoringService)
        {
            _auditMonitoringService = auditMonitoringService;
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult IsUp() => Ok(GetType().Name.Replace("Controller", "") + " Is Up! ;)");


        [HttpGet]
        public ActionResult Get()
        {
            return Ok(_auditMonitoringService.GetAll(new AuditMonitoringModel(),
                new AuditMonitoringProjectionModel() { Action = true },
                new Pagination() { PageIndex = 1, PageSize = 10 }));
        }

    }
}