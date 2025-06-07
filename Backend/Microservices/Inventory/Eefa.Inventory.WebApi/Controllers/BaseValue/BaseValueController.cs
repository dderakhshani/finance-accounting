using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Eefa.Common;
using Eefa.Common.Data;
using Eefa.Common.Web;
using Eefa.Inventory.Application;
using Microsoft.AspNetCore.Mvc;

namespace Eefa.Inventory.WebApi.Controllers
{
    public class BaseValueController : ApiControllerBase
    {
        IBaseValueQueries _baseValueQueries;
       

        public BaseValueController(IBaseValueQueries baseValueQueries)
        {
            _baseValueQueries = baseValueQueries ?? throw new ArgumentNullException(nameof(baseValueQueries));

        }
        [HttpPost]
        public async Task<IActionResult> GetCurrencyBaseValue()
        {

            var result = await _baseValueQueries.GetCurrencyBaseValue();

            return Ok(ServiceResult<PagedList<BaseValueModel>>.Success(result));
        }
        [HttpGet]
        public async Task<IActionResult> GetDocumentTagBaseValue()
        {

            var result = await _baseValueQueries.GetDocumentTagBaseValue();

            return Ok(ServiceResult<List<string>>.Success(result));
        }

        [HttpPost]
        public async Task<IActionResult> GetReceiptALLBaseValue()
        {
            var result = await _baseValueQueries.GetReceiptALLBaseValue();
            return Ok(ServiceResult<PagedList<BaseValueModel>>.Success(result));

        }
        [HttpPost]
        public async Task<IActionResult> GetDepreciationTypeBaseValue()
        {
            var result = await _baseValueQueries.GetDepreciationTypeBaseValue();
            return Ok(ServiceResult<PagedList<BaseValueModel>>.Success(result));

        }
        [HttpPost]
        public async Task<IActionResult> GetCommodityGroupBaseValue()
        {
            var result = await _baseValueQueries.GetCommodityGroupBaseValue();
            return Ok(ServiceResult<PagedList<BaseValueModel>>.Success(result));

        }


    }
}