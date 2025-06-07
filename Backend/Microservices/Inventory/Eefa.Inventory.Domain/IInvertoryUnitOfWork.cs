using Eefa.Common.Data;
using Eefa.Inventory.Domain.Aggregates.WarehouseAggregate;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Eefa.Inventory.Domain
{
    public interface IInvertoryUnitOfWork : IUnitOfWork
    {
        public DbSet<BaseValue> BaseValues { get; set; }
        public DbSet<CodeVoucherGroup> CodeVoucherGroups { get; set; }
        public DbSet<BaseValueType> BaseValueTypes { get; set; }
        public DbSet<Bom> Boms { get; set; }
        public DbSet<BomItem> BomItems { get; set; }
        public DbSet<BomValue> BomValues { get; set; }
        public DbSet<BomValueHeader> BomValueHeaders { get; set; }
        public DbSet<AccountHead> AccountHead { get; set; }

        public DbSet<CategoryPropertyMapping> CategoryPropertyMappings { get; set; }
        public DbSet<Commodity> Commodities { get; set; }
        public DbSet<CommodityCategory> CommodityCategories { get; set; }
        public DbSet<CommodityCategoryProperty> CommodityCategoryProperties { get; set; }
        public DbSet<CommodityCategoryPropertyItem> CommodityCategoryPropertyItems { get; set; }
        public DbSet<CommodityPropertyValue> CommodityPropertyValues { get; set; }
        public DbSet<CommodityMeasures> CommodityMeasures { get; set; }
        public DbSet<Receipt> DocumentHeads { get; set; }
        public DbSet<DocumentItem> DocumentItems { get; set; }
        public DbSet<DocumentNumberingFormat> DocumentNumberingFormats { get; set; }
        public DbSet<DocumentPayment> DocumentPayments { get; set; }
        public DbSet<DocumentHeadExtend> DocumentHeadExtend { get; set; }
        public DbSet<Document> Document { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Freight> Freights { get; set; }
        public DbSet<MeasureUnit> MeasureUnits { get; set; }
        public DbSet<MeasureUnitConversion> MeasureUnitConversions { get; set; }
        public DbSet<WarehouseStocks> WarehouseStocks { get; set; }
        public DbSet<Turn> Turns { get; set; }
        public DbSet<TurnDocument> TurnDocuments { get; set; }
        public DbSet<UserCompany> UserCompanies { get; set; }
        public DbSet<UserYear> UserYears { get; set; }
        public DbSet<WarehouseLayoutCategories> WarehouseLayoutCategories { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<WarehouseHistory> WarehouseHistories { get; set; }
        public DbSet<WarehouseLayout> WarehouseLayouts { get; set; }
        public DbSet<WarehouseLayoutProperty> WarehouseLayoutProperties { get; set; }
        public DbSet<WarehouseLayoutQuantity> WarehouseLayoutQuantities { get; set; }
        public DbSet<Year> Years { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<AccountReference> AccountReferences { get; set; }
        public DbSet<AccountReferencesGroup> AccountReferencesGroups { get; set; }
        public DbSet<AccountReferencesRelReferencesGroup> AccountReferencesRelReferencesGroups { get; set; }
        public DbSet<WarehouseRequestExit> WarehouseRequestExit { get; set; }
        public DbSet<Assets> Assets { get; set; }
        public DbSet<Employees> Employees { get; set; }
        public DbSet<UnitCommodityQuota> UnitCommodityQuota { get; set; }
        public DbSet<Units> Units { get; set; }
        public DbSet<WarehousesCodeVoucherGroups> WarehousesCodeVoucherGroups { get; set; }
        public DbSet<WarehousesCategories> WarehousesCategories { get; set; }
        public DbSet<PersonsDebitedCommodities> PersonsDebitedCommodities { get; set; }
        public DbSet<QuotaGroup> QuotaGroups { get; set; }
        public DbSet<DocumentItemsBom> DocumentItemsBom { get; set; }
        public DbSet<AssetAttachments> AssetAttachments { get; set; }
        public DbSet<Attachment> Attachment { get; set; }

        public DbSet<DocumentAttachment> DocumentAttachment { get; set; }
        public DbSet<CorrectionRequest> CorrectionRequest { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<AccessToWarehouse> AccessToWarehouse { get; set; }
        public DbSet<inventory_StockFromTadbir> inventory_StockFromTadbir { get; set; }

        public DbSet<DocumentHeadExtraCost> DocumentHeadExtraCost { get; set; }

        public DbSet<VouchersDetail> VouchersDetail { get; set; }
        public DbSet<VouchersHead> VouchersHead { get; set; }
        public DbSet<RolePermission> RolePermission { get; set; }
        public DbSet<UserRole> UserRole { get; set; }
        public DbSet<WarehouseCountFormHead> WarehouseCountFormHead { get; set; }
        public DbSet<WarehouseCountFormDetail> WarehouseCountFormDetail { get; set; }
        public DbSet<AccountHeadRelReferenceGroup> AccountHeadRelReferenceGroup { get; set; }

        public DbSet<Permissions> Permissions { get; set; }

        
        //-------------------------------View-----------------------------------------
        public DbSet<ReceiptView> ReceiptView { get; set; }
        public DbSet<WarehouseLayoutsCommoditiesView> WarehouseLayoutsCommoditiesView { get; set; }
        public DbSet<WarehouseLayoutsCommoditiesQuantityView> WarehouseLayoutsCommoditiesQuantityView { get; set; }
        public DbSet<AccountReferenceView> AccountReferenceView { get; set; }
        public DbSet<ReceiptInvoiceView> ReceiptInvoiceView { get; set; }
        public DbSet<WarehousesLastLevelView> WarehousesLastLevelView { get; set; }
        public DbSet<ViewCommodity> ViewCommodity { get; set; }
        public DbSet<BomValueView> BomValueView { get; set; }
        public DbSet<ReceiptItemsView> ReceiptItemsView { get; set; }
        public DbSet<UnitCommodityQuotaView> UnitCommodityQuotaView { get; set; }
        public DbSet<AssetsNotAssignedView> AssetsNotAssignedView { get; set; }
        public DbSet<EmployeesUnitsView> EmployeesUnitsView { get; set; }
        public DbSet<AssetsView> AssetsView { get; set; }
        public DbSet<PersonsDebitedCommoditiesView> PersonsDebitedCommoditiesView { get; set; }

        public DbSet<WarehouseHistoriesDocumentView> WarehouseHistoriesDocumentView { get; set; }
        public DbSet<AccountReferenceEmployeeView> AccountReferenceEmployeeView { get; set; }
        public DbSet<ReceiptGroupbyInvoiceView> ReceiptGroupbyInvoiceView { get; set; }
        public DbSet<DocumentHeadGetIdView> DocumentHeadGetIdView { get; set; }

        public DbSet<ReceiptWithCommodityView> ReceiptWithCommodityView { get; set; }

        public DbSet<WarehouseHistoriesDocumentItemView> WarehouseHistoriesDocumentItemView { get; set; }

        public DbSet<CommodityPropertyWithThicknessView> CommodityPropertyWithThicknessView { get; set; }

        public DbSet<View_Sina_FinancialOperationNumber> View_Sina_FinancialOperationNumber { get; set; }
        public DbSet<WarehouseRequestExitView> WarehouseRequestExitView { get; set; }

        public DbSet<WarehouseLayoutsCommoditiesViewArani> WarehouseLayoutsCommoditiesViewArani { get; set; }

        public DbSet<DocumentHeadView> DocumentHeadView { get; set; }
        public DbSet<WarehouseLayoutView> WarehouseLayoutView { get; set; }
        IQueryable<spWarehouseLayoutRecursive> GetWarehouseLayoutRecursive(int parentId);
        IQueryable<WarehouseCountFormReport> GetWarehouseCountReport(int warehouseCountFormHeadeId);

    }
}
