using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Eefa.Common;
using Eefa.Common.Data;
using Eefa.Common.Web;
using Eefa.Purchase.Application.Models;
using Eefa.Purchase.Application.Queries.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace Eefa.Purchase.WebApi.Controllers.BaseValue
{
    public class BaseValueController : ApiControllerBase
    {
        IBaseValueQueries _baseValueQueries;
       

        public BaseValueController(IBaseValueQueries baseValueQueries)
        {
            _baseValueQueries = baseValueQueries ?? throw new ArgumentNullException(nameof(baseValueQueries));

        }
        [HttpGet]
        public async Task<IActionResult> GetDocumentTagBaseValue()
        {

            var result = await _baseValueQueries.GetDocumentTagBaseValue();

            return Ok(ServiceResult<List<string>>.Success(result));
        }
        [HttpPost]
        public async Task<IActionResult> GetCurrencyBaseValue()
        {

            var result = await _baseValueQueries.GetCurrencyBaseValue();

            return Ok(ServiceResult<PagedList<BaseValueModel>>.Success(result));
        }
        [HttpPost]
        public async Task<IActionResult> GeVatDutiesTaxValue()
        {

            var result = await _baseValueQueries.GeVatDutiesTaxValue();

            return Ok(ServiceResult<string>.Success(result));
        }

        [HttpPost]
        public async Task<IActionResult> GetReceiptALLBaseValue()
        {
            var result = await _baseValueQueries.GetInvoiceALLBaseValue();
            return Ok(ServiceResult<PagedList<BaseValueModel>>.Success(result));

        }
    }
}