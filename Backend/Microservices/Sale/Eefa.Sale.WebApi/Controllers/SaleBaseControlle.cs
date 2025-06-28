using Eefa.Common.Web;
using Eefa.Sale.Application.Commands;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Eefa.Sale.WebApi.Controllers
{
    [Route("api/sale/[controller]/[action]")]
    [ApiController]
    public class SaleBaseControlle : ApiControllerBase
    {
    }
}
