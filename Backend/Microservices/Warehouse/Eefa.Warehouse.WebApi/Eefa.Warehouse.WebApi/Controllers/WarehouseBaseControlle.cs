using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Eefa.Common.Web;

namespace Eefa.Warehouse.WebApi.Controllers
{
    [Route("api/warehouse/[controller]/[action]")]
    [ApiController]
    public class WarehouseBaseControlle : ApiControllerBase
    {
    }
}
