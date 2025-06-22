using Eefa.Sale.Application.Commands;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Eefa.Sale.WebApi.Controllers
{
    public class PriceListController : SaleBaseControlle
    {

        [HttpPost("Pricelist")]
        public async Task<IActionResult> Add([FromBody] CreatePriceListCommand model) => Ok(await Mediator.Send(model));
    }
}
