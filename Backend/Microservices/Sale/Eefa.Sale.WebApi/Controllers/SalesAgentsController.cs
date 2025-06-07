using Eefa.Common.Data.Query;
using Eefa.Common.Web;
//using Eefa.Sale.Application.Commands.SaleAgent.Create;
//using Eefa.Sale.Application.Commands.SaleAgent.Delete;
//using Eefa.Sale.Application.Commands.SaleAgent.Update;
using Eefa.Sale.Application.Queries.SaleAgents;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Eefa.Sale.WebApi.Controllers
{
    public class SalesAgentsController : ApiControllerBase
    {
        readonly ISalesAgentQueries _salesAgentsQueries;

        public SalesAgentsController(ISalesAgentQueries salesAgentsQueries)
        {
            this._salesAgentsQueries = salesAgentsQueries;
        }


        [HttpPost]
        public async Task<IActionResult> GetAll(PaginatedQueryModel paginatedQuery) => Ok(await _salesAgentsQueries.GetAll(paginatedQuery));

        [HttpGet]
        public async Task<IActionResult> Get(int id) => Ok(await _salesAgentsQueries.GetById(id));
        //[HttpPost]
        //public async Task<IActionResult> Add([FromBody] CreateSaleAgentCommand model) => Ok(await Mediator.Send(model));

        //[HttpPut]
        //public async Task<IActionResult> Update([FromBody] UpdateSaleAgentCommand model) => Ok(await Mediator.Send(model));

        //[HttpDelete]
        //public async Task<IActionResult> Delete([FromQuery] int id) => Ok(await Mediator.Send(new DeleteSaleAgentCommand { Id = id }));
    }
}
