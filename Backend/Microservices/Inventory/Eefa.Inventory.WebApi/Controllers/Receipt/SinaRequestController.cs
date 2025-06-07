using System;
using System.Threading.Tasks;
using Eefa.Common;
using Eefa.Common.Web;
using Eefa.Inventory.Application;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Eefa.Inventory.Domain;

namespace Eefa.Inventory.WebApi.Controllers.Receipt
{
    public class SinaRequestController : ApiControllerBase
    {
        ISinaRequestCommandQueries _receiptRepository;
        private readonly IReceiptCommandsService _receiptCommandsService;
        public SinaRequestController(ISinaRequestCommandQueries receiptQueries, IReceiptCommandsService receiptCommandsService)
        {
            _receiptRepository = receiptQueries;
            _receiptCommandsService = receiptCommandsService;


        }


        [HttpGet]
        public async Task<IActionResult> GetProductInputToWarehouse(string Date)
        {

            var result = await _receiptRepository.GetInputProductToWarehouse(Date.Trim());
           
            return Ok(ServiceResult<ReceiptQueryModel>.Success(result));
        }
        [HttpGet]
        public async Task UpdateProductProperty(string Date)
        {

            await _receiptRepository.UpdateProductProperty(Date.Trim());
            
        }
        [HttpGet]
        public async Task<IActionResult> GetProductLeaveWarehouse(string Date)
        {

            var result = await _receiptRepository.GetProductLeaveWarehouse(Date.Trim());
            return Ok(ServiceResult<ReceiptQueryModel>.Success(result));
        }
        [HttpGet]
        public async Task<IActionResult> GetProductList()
        {

            var result = await _receiptRepository.GetProductByMaxSinaIdList();
            return Ok(ServiceResult<ICollection<SinaProduct>>.Success(result));
        }
        [HttpGet]
        public async Task<IActionResult> GetAllProductNeedUpdate()
        {

            var result = await _receiptRepository.GetAllProductNeedUpdate();
            return Ok(ServiceResult<ICollection<SinaProduct>>.Success(result));
        }


        [HttpGet]
        public async Task<IActionResult> GetFilesByPaymentNumber(string financialOperationNumber)
        {

            var result = await _receiptRepository.GetFilesByPaymentNumber(financialOperationNumber.Trim());
            return Ok(ServiceResult<ICollection<FilesByPaymentNumber>>.Success(result));
        }
        [HttpPost]
        public async Task<IActionResult> WarehouseIOCommodity([FromBody] WarehouseIOCommodity model) {

            WarehouseIOCommodity warehouseIOCommodity = new WarehouseIOCommodity();
            warehouseIOCommodity.CommodityList = model.CommodityList;
            warehouseIOCommodity.WarehouseId = 1;
            warehouseIOCommodity.yearId = 2;
            warehouseIOCommodity.DocumentDate= model.DocumentDate;
            warehouseIOCommodity.Mode = 1;
            warehouseIOCommodity.DocumentStauseBaseValue = 33;

            InsertDocumentHeadsCommand UpdateQuantityCommand = new InsertDocumentHeadsCommand() { warehouseIOCommodity = warehouseIOCommodity };
           return Ok(Mediator.Send(UpdateQuantityCommand));
                
        }


    }
}