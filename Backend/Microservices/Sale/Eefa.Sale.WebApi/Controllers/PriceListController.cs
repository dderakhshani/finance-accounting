using Eefa.Sale.Application.Commands;
using Eefa.Sale.Application.Commands.PriceList.Copy;
using Eefa.Sale.Application.Queries.PriceList;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Eefa.Sale.WebApi.Controllers
{
    public class PriceListController : SaleBaseControlle
    {
        IPriceListQueries _priceListQueries;


        public PriceListController(IPriceListQueries priceListQueries)
        {
            _priceListQueries = priceListQueries;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreatePriceListCommand model) => Ok(await Mediator.Send(model));

        [HttpPost]
        public async Task<IActionResult> Copy([FromBody] CopyPriceListCommand model) => Ok(await Mediator.Send(model));

        [HttpGet]
        public async Task<IActionResult> GetPriceList() => Ok(await _priceListQueries.GetAll());
    }
}
