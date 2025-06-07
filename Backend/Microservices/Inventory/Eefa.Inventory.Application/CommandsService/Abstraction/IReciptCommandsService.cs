using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Eefa.Common.CommandQuery;
using Eefa.Inventory.Domain;
using static Eefa.Invertory.Infrastructure.Services.AdminApi.AdminApiService;

namespace Eefa.Inventory.Application
{
    public interface IReceiptCommandsService : IQuery
    {
        Task<int> InsertAndUpdateDocument(Domain.Receipt receipt);
        Task SerialFormula(Receipt receipt, string code, CancellationToken cancellationToken);
        Task<int> lastDocumentNo(Receipt receipt, CancellationToken cancellationToken);
        Task<CodeVoucherGroup> GetNewCodeVoucherGroup(Receipt receipt);
        Task InsertDocumentHeadExtend(int? RequesterReferenceId, int? FollowUpReferenceId, Domain.Receipt receipt);
        Task InsertAssets(AssetsModel Assets, int MainMeasureId, int CommodityId, Domain.Receipt receipt);
        Task<bool> IsDuplicateDocumentNo(Receipt receipt, CancellationToken cancellationToken);
        Task<int?> GetEmployee(string Code);
        Task<int> AddReceiptForExitReceiptArani(RequestResult Request, string DarkhastKonandehCode, int WarehouseId,string codeVoucherGroupType);
        Task<Domain.Commodity> AddCommodity(string Code, string Name, string CompactCode);
        Task<Domain.Commodity> AddProduct(SinaProduct model);
        Task<Domain.Commodity> UpdateProduct(SinaProduct model);
        Task<Person> AddNewPerson(string Name, string NationalNumber, string Mobile);
        Task<Employees> AddNewEmployees(int PersonId, string Code);
        Task<DocumentItemsBom> AddDocumentItemsBom(int WarehouseLayoutsId,int WarehouseId, int currency, double Quantity, int CommodityId, int documentItemId, BomValueView bomValueView);
        
        Task CalculateTotalItemPrice(Receipt receipt);
        Task ModifyAttachmentAssets(List<AttachmentAssetsRequest> attachmentAssets, int PersonsDebitedCommoditiesId);
        Task ModifyDocumentAttachments(List<int> attachmentIds, int DocumentHeadId);
        Task GetPriceBuyItems(int CommodityId, int WarehouseId, int? DocumentItemsId, double Quantity, DocumentItem documentItem);
        Task GetPriceBuyBom(int CommodityId, int WarehouseId, double Quantity, DocumentItemsBom documentItemsBom);
        Task GetPriceEstimateItems(int CommodityId, int WarehouseId, DocumentItem documentItem);
        Task<double> ComputeAvgPrice(int CommodityId, Domain.Receipt receipt);
        
        Task<int> UploadInventoryTadbir(Microsoft.AspNetCore.Http.IFormFile file, string warehouseId);
        Task<spUpdateWarehouseCommodityAvailabe2> UpdateWarehouseCommodityAvailable(int warehouseId, int yearId);
        void GenerateInvoiceNumber(string LeaveType, Receipt receipt, CodeVoucherGroup codeVoucherGroup);
        string GenerateInvoiceNumber(string LeaveType, CodeVoucherGroup codeVoucherGroup);
        Task<Receipt> UpdateImportPurchaseReceipt(Receipt receipt);
        Receipt ConvertTagArray(string Tag, Receipt a);
        string ConvertTagArray(string Tag);
        DateTime? SetExpireDate(DateTime? ExpireDate, DateTime LastDate);
        Task<ResultModel> UpdateVoucher(ConvertToRailsReceiptCommand model);
        void ReceiptBaseDataInsert(DateTime DocumentDate, Receipt receipt);
        Task<object> UpdateWarehouseCommodityPrice(int warehouseId, int yearId);

        Task<Domain.Commodity> UpdateProductProperty(int CommodityId, SinaProducingInputProduct inputProduct);
        string AppendNewTagToReceipt(string tag, Receipt receipt);
        string RemoveTagFromReceipt(string tag, Receipt receipt);

        Task<int> WarehouseCheckLoseData();
        Task<int> UpdateWarehouseLayout(int WarehouseId);
        Task<int> RemoveCommodityFromWarehouse(int WarehouseId);
        Task<int> UpdateAddNewCommodity();
        Task<int> ArchiveDocumentHeadsByDocumentDate(DateTime? FromDate, DateTime? ToDate, int WarehouseId, int DocumentStatuesBaseValue);
        Task<bool> IsValidAccountHeadRelationByReferenceGroup(int? AccountHeadId, int? ReferenceGroupId);
    }
}

