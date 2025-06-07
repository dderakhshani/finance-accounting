using Eefa.Common;
using Eefa.Common.Data;
using Eefa.Inventory.Domain;
using Eefa.Invertory.Infrastructure.Context.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Linq;
using Eefa.Inventory.Domain.Aggregates.WarehouseAggregate;

namespace Eefa.Invertory.Infrastructure.Context
{
    public partial class InvertoryContext : AuditableDbContext, IInvertoryUnitOfWork
    {
        public virtual DbSet<BaseValue> BaseValues { get; set; } = default!;
        public virtual DbSet<CodeVoucherGroup> CodeVoucherGroups { get; set; }
        public virtual DbSet<BaseValueType> BaseValueTypes { get; set; } = default!;
        public virtual DbSet<Bom> Boms { get; set; } = default!;
        public virtual DbSet<BomItem> BomItems { get; set; } = default!;
        public virtual DbSet<BomValue> BomValues { get; set; } = default!;
        public virtual DbSet<BomValueHeader> BomValueHeaders { get; set; } = default!;
        public virtual DbSet<CategoryPropertyMapping> CategoryPropertyMappings { get; set; } = default!;
        public virtual DbSet<Commodity> Commodities { get; set; } = default!;
        public virtual DbSet<CommodityCategory> CommodityCategories { get; set; } = default!;
        public virtual DbSet<CommodityCategoryProperty> CommodityCategoryProperties { get; set; } = default!;
        public virtual DbSet<CommodityCategoryPropertyItem> CommodityCategoryPropertyItems { get; set; } = default!;
        public virtual DbSet<CommodityPropertyValue> CommodityPropertyValues { get; set; } = default!;
        public virtual DbSet<CommodityMeasures> CommodityMeasures { get; set; } = default!;
        public virtual DbSet<Receipt> DocumentHeads { get; set; } = default!;
        public virtual DbSet<DocumentItem> DocumentItems { get; set; } = default!;
        public virtual DbSet<DocumentNumberingFormat> DocumentNumberingFormats { get; set; } = default!;
        public virtual DbSet<DocumentPayment> DocumentPayments { get; set; } = default!;
        public virtual DbSet<DocumentHeadExtend> DocumentHeadExtend { get; set; } = default!;
        public virtual DbSet<Document> Document { get; set; } = default!;
        public virtual DbSet<Driver> Drivers { get; set; } = default!;
        public virtual DbSet<Freight> Freights { get; set; } = default!;
        public virtual DbSet<MeasureUnit> MeasureUnits { get; set; } = default!;
        public virtual DbSet<MeasureUnitConversion> MeasureUnitConversions { get; set; } = default!;
        public virtual DbSet<WarehouseStocks> WarehouseStocks { get; set; } = default!;
        public virtual DbSet<Turn> Turns { get; set; } = default!;
        public virtual DbSet<TurnDocument> TurnDocuments { get; set; } = default!;
        public virtual DbSet<UserCompany> UserCompanies { get; set; } = default!;
        public virtual DbSet<UserYear> UserYears { get; set; } = default!;
        public virtual DbSet<WarehouseLayoutCategories> WarehouseLayoutCategories { get; set; } = default!;
        public virtual DbSet<Warehouse> Warehouses { get; set; } = default!;
        public virtual DbSet<WarehouseHistory> WarehouseHistories { get; set; } = default!;
        public virtual DbSet<WarehouseLayout> WarehouseLayouts { get; set; } = default!;
        public virtual DbSet<WarehouseLayoutProperty> WarehouseLayoutProperties { get; set; } = default!;
        public virtual DbSet<WarehouseLayoutQuantity> WarehouseLayoutQuantities { get; set; } = default!;
        public virtual DbSet<Year> Years { get; set; } = default!;
        public virtual DbSet<Person> Persons { get; set; } = default!;
        public virtual DbSet<AccountReference> AccountReferences { get; set; } = default!;
        public virtual DbSet<AccountReferencesGroup> AccountReferencesGroups { get; set; } = default!;
        public virtual DbSet<AccountReferencesRelReferencesGroup> AccountReferencesRelReferencesGroups { get; set; } = default!;
        public virtual DbSet<WarehouseRequestExit> WarehouseRequestExit { get; set; } = default!;
        public virtual DbSet<Assets> Assets { get; set; } = default!;
        public virtual DbSet<Employees> Employees { get; set; } = default!;
        public virtual DbSet<UnitCommodityQuota> UnitCommodityQuota { get; set; } = default!;
        public virtual DbSet<Units> Units { get; set; } = default!;

        public virtual DbSet<WarehousesCodeVoucherGroups> WarehousesCodeVoucherGroups { get; set; } = default!;
        public virtual DbSet<WarehousesCategories> WarehousesCategories { get; set; } = default!;
        public virtual DbSet<PersonsDebitedCommodities> PersonsDebitedCommodities { get; set; } = default!;

        public virtual DbSet<QuotaGroup> QuotaGroups { get; set; } = default!;
        public virtual DbSet<AccountHead> AccountHead { get; set; } = default!;
        public virtual DbSet<DocumentItemsBom> DocumentItemsBom { get; set; } = default!;
        public virtual DbSet<AssetAttachments> AssetAttachments { get; set; } = default!;
        public virtual DbSet<Attachment> Attachment { get; set; } = default!;
        public virtual DbSet<DocumentAttachment> DocumentAttachment { get; set; } = default!;
        public virtual DbSet<CorrectionRequest> CorrectionRequest { get; set; } = default!;
        public virtual DbSet<User> User { get; set; } = default!;
        public virtual DbSet<AccessToWarehouse> AccessToWarehouse { get; set; } = default!;
        public virtual DbSet<inventory_StockFromTadbir> inventory_StockFromTadbir { get; set; }

        public virtual DbSet<DocumentHeadExtraCost> DocumentHeadExtraCost { get; set; }
        public virtual DbSet<VouchersDetail> VouchersDetail { get; set; }

        public virtual DbSet<VouchersHead> VouchersHead { get; set; }
        public virtual DbSet<WarehouseCountFormHead> WarehouseCountFormHead { get; set; }
        public virtual DbSet<WarehouseCountFormDetail> WarehouseCountFormDetail { get; set; }

        public DbSet<RolePermission> RolePermission { get; set; }
        public DbSet<UserRole> UserRole { get; set; }
        public DbSet<AccountHeadRelReferenceGroup> AccountHeadRelReferenceGroup { get; set; }

        public virtual DbSet<Permissions> Permissions { get; set; }

        //-------------------------------View-----------------------------------------
        public virtual DbSet<ReceiptView> ReceiptView { get; set; } = default!;
        public virtual DbSet<WarehouseLayoutsCommoditiesView> WarehouseLayoutsCommoditiesView { get; set; } = default!;
        public virtual DbSet<WarehouseLayoutsCommoditiesViewArani> WarehouseLayoutsCommoditiesViewArani { get; set; } = default!;
        public virtual DbSet<WarehouseLayoutsCommoditiesQuantityView> WarehouseLayoutsCommoditiesQuantityView { get; set; } = default!;
        public virtual DbSet<AccountReferenceView> AccountReferenceView { get; set; } = default!;
        public virtual DbSet<ReceiptInvoiceView> ReceiptInvoiceView { get; set; } = default!;
        public virtual DbSet<WarehousesLastLevelView> WarehousesLastLevelView { get; set; } = default!;
        public virtual DbSet<ViewCommodity> ViewCommodity { get; set; } = default!;
        public virtual DbSet<BomValueView> BomValueView { get; set; } = default!;
        public virtual DbSet<ReceiptItemsView> ReceiptItemsView { get; set; } = default!;
        public virtual DbSet<UnitCommodityQuotaView> UnitCommodityQuotaView { get; set; } = default!;
        public virtual DbSet<AssetsNotAssignedView> AssetsNotAssignedView { get; set; } = default!;
        public virtual DbSet<EmployeesUnitsView> EmployeesUnitsView { get; set; } = default!;
        public virtual DbSet<AssetsView> AssetsView { get; set; } = default!;
        public virtual DbSet<PersonsDebitedCommoditiesView> PersonsDebitedCommoditiesView { get; set; } = default!;
        public virtual DbSet<WarehouseHistoriesDocumentView> WarehouseHistoriesDocumentView { get; set; } = default!;
        public virtual DbSet<AccountReferenceEmployeeView> AccountReferenceEmployeeView { get; set; } = default!;
        public virtual DbSet<ReceiptGroupbyInvoiceView> ReceiptGroupbyInvoiceView { get; set; }

        public virtual DbSet<DocumentHeadGetIdView> DocumentHeadGetIdView { get; set; }

        public virtual DbSet<ReceiptWithCommodityView> ReceiptWithCommodityView { get; set; }
        public DbSet<WarehouseHistoriesDocumentItemView> WarehouseHistoriesDocumentItemView { get; set; }

        public DbSet<CommodityPropertyWithThicknessView> CommodityPropertyWithThicknessView { get; set; }

        public DbSet<View_Sina_FinancialOperationNumber> View_Sina_FinancialOperationNumber { get; set; }
        public DbSet<DocumentHeadView> DocumentHeadView { get; set; }

        public DbSet<WarehouseRequestExitView> WarehouseRequestExitView { get; set; }
        public virtual DbSet<WarehouseLayoutView> WarehouseLayoutView { get; set; }

        


        //-------------------------------sp-----------------------------------------
        //public DbSet<spWarehouseLayoutRecursive> spWarehouseLayoutRecursive { get; set; }

        public InvertoryContext(DbContextOptions<InvertoryContext> options, ICurrentUserAccessor currentUserAccessor)
           : base(options, currentUserAccessor)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");
            //--------------------------Table---------------------------------------
            modelBuilder.ApplyConfiguration(new DocumentAttachmentConfiguration());
            
            modelBuilder.ApplyConfiguration(new AttachmentConfiguration());
            modelBuilder.ApplyConfiguration(new AccountHeadConfiguration());
            modelBuilder.ApplyConfiguration(new BaseValueConfiguration());
            modelBuilder.ApplyConfiguration(new BaseValueTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CodeVoucherGroupConfiguration());
            modelBuilder.ApplyConfiguration(new BomConfiguration());
            modelBuilder.ApplyConfiguration(new BomItemConfiguration());
            modelBuilder.ApplyConfiguration(new BomValueConfiguration());
            modelBuilder.ApplyConfiguration(new BomValueHeaderConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryPropertyMappingConfiguration());
            modelBuilder.ApplyConfiguration(new CommodityConfiguration());
            modelBuilder.ApplyConfiguration(new CommodityCategoryConfiguration());
            modelBuilder.ApplyConfiguration(new CommodityCategoryPropertyConfiguration());
            modelBuilder.ApplyConfiguration(new CommodityCategoryPropertyItemConfiguration());
            modelBuilder.ApplyConfiguration(new CommodityPropertyValueConfiguration());
            modelBuilder.ApplyConfiguration(new ReceiptConfiguration());
            modelBuilder.ApplyConfiguration(new DocumentItemConfiguration());
            modelBuilder.ApplyConfiguration(new DocumentNumberingFormatConfiguration());
            modelBuilder.ApplyConfiguration(new DocumentPaymentConfiguration());
            modelBuilder.ApplyConfiguration(new DocumentConfiguration());
            modelBuilder.ApplyConfiguration(new DriverConfiguration());
            modelBuilder.ApplyConfiguration(new FreightConfiguration());
            modelBuilder.ApplyConfiguration(new MeasureUnitConfiguration());
            modelBuilder.ApplyConfiguration(new MeasureUnitConversionConfiguration());
            modelBuilder.ApplyConfiguration(new WarehouseStocksConfiguration());
            modelBuilder.ApplyConfiguration(new TurnConfiguration());
            modelBuilder.ApplyConfiguration(new TurnDocumentConfiguration());
            modelBuilder.ApplyConfiguration(new UserCompanyConfiguration());
            modelBuilder.ApplyConfiguration(new UserYearConfiguration());
            modelBuilder.ApplyConfiguration(new WarehousConfiguration());
            modelBuilder.ApplyConfiguration(new WarehouseHistoryConfiguration());
            modelBuilder.ApplyConfiguration(new WarehouseLayoutConfiguration());
            modelBuilder.ApplyConfiguration(new WarehouseLayoutPropertyConfiguration());
            modelBuilder.ApplyConfiguration(new WarehouseLayoutQuantityConfiguration());
            modelBuilder.ApplyConfiguration(new YearConfiguration());
            modelBuilder.ApplyConfiguration(new WarehouseLayoutCategoriesConfiguration());
            modelBuilder.ApplyConfiguration(new AccountReferenceConfiguration());
            modelBuilder.ApplyConfiguration(new AccountReferencesGroupConfiguration());
            modelBuilder.ApplyConfiguration(new AccountReferencesRelReferencesGroupConfiguration());
            modelBuilder.ApplyConfiguration(new DocumentHeadExtendConfiguration());
            modelBuilder.ApplyConfiguration(new PersonConfiguration());
            modelBuilder.ApplyConfiguration(new CommodityMeasuresConfiguration());
            modelBuilder.ApplyConfiguration(new WarehouseRequestExitConfiguration());
            modelBuilder.ApplyConfiguration(new AssetsConfiguration());
            modelBuilder.ApplyConfiguration(new EmployeesConfiguration());
            modelBuilder.ApplyConfiguration(new UnitCommodityQuotaConfiguration());
            modelBuilder.ApplyConfiguration(new UnitsConfiguration());
            modelBuilder.ApplyConfiguration(new WarehousesCodeVoucherGroupsConfiguration());
            modelBuilder.ApplyConfiguration(new WarehousesCategoriesGroupsConfiguration());
            modelBuilder.ApplyConfiguration(new PersonsDebitedCommoditiesConfiguration());
            modelBuilder.ApplyConfiguration(new QuotaGroupConfiguration());
            modelBuilder.ApplyConfiguration(new DocumentItemsBomConfiguration());
            modelBuilder.ApplyConfiguration(new AssetAttachmentsConfiguration());
            modelBuilder.ApplyConfiguration(new CorrectionRequestConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new AccessToWarehouseConfiguration());
            modelBuilder.ApplyConfiguration(new inventory_StockFromTadbirConfiguration());
            modelBuilder.ApplyConfiguration(new DocumentHeadExtraCostConfiguration());
            modelBuilder.ApplyConfiguration(new VouchersDetailConfiguration());
            modelBuilder.ApplyConfiguration(new VouchersHeadConfiguration());
            modelBuilder.ApplyConfiguration(new RolePermissionConfiguration());
            modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
            modelBuilder.ApplyConfiguration(new AccountHeadRelReferenceGroupConfiguration());
            modelBuilder.ApplyConfiguration(new PermissionsConfiguration());

            //--------------------View------------------------------------------------
            modelBuilder.ApplyConfiguration(new ReceiptViewConfiguration());
            modelBuilder.ApplyConfiguration(new WarehouseLayoutsCommoditiesViewConfiguration());
            modelBuilder.ApplyConfiguration(new WarehouseLayoutsCommoditiesViewAraniConfiguration());
            modelBuilder.ApplyConfiguration(new WarehouseLayoutsCommoditiesQuantityViewConfiguration());
            modelBuilder.ApplyConfiguration(new AccountReferenceViewConfiguration());
            modelBuilder.ApplyConfiguration(new ReceiptInvoiceViewConfiguration());
            modelBuilder.ApplyConfiguration(new WarehousesLastLevelViewConfiguration());
            modelBuilder.ApplyConfiguration(new ViewCommodityConfiguration());
            modelBuilder.ApplyConfiguration(new BomValueViewConfiguration());
            modelBuilder.ApplyConfiguration(new ReceiptItemsViewConfiguration());
            modelBuilder.ApplyConfiguration(new UnitCommodityQuotaViewConfiguration());
            modelBuilder.ApplyConfiguration(new AssetsNotAssignedViewConfiguration());
            modelBuilder.ApplyConfiguration(new EmployeesUnitsViewConfiguration());
            modelBuilder.ApplyConfiguration(new AssetsViewConfiguration());
            modelBuilder.ApplyConfiguration(new PersonsDebitedCommoditiesViewConfiguration());
            modelBuilder.ApplyConfiguration(new WarehouseHistoriesDocumentViewConfiguration());
            modelBuilder.ApplyConfiguration(new AccountReferenceEmployeeViewConfiguration());
            modelBuilder.ApplyConfiguration(new ReceiptGroupbyInvoiceViewConfiguration());
            modelBuilder.ApplyConfiguration(new DocumentHeadGetIdViewConfiguration());
            modelBuilder.ApplyConfiguration(new ReceiptWithCommodityViewConfiguration());
            modelBuilder.ApplyConfiguration(new WarehouseHistoriesDocumentItemViewConfiguration());
            modelBuilder.ApplyConfiguration(new CommodityPropertyWithThicknessViewConfiguration());
            modelBuilder.ApplyConfiguration(new View_Sina_FinancialOperationNumberConfiguration());
            modelBuilder.ApplyConfiguration(new WarehouseRequestExitViewConfiguration());
            modelBuilder.ApplyConfiguration(new WarehouseCountFormHeadConfiguration());
            modelBuilder.ApplyConfiguration(new WarehouseCountFormDetailConfiguration());

            modelBuilder.ApplyConfiguration(new DocumentHeadViewConfiguration());
            modelBuilder.ApplyConfiguration(new WarehouseLayoutViewConfiguration());
            

            //----------------------sp---------------------------------------------------


            //---------------------Sql Function-----------------------------------
            modelBuilder.HasDbFunction(() => GetWarehouseLayoutRecursive(default)).HasName("GetWarehouseLayoutRecursive");
            modelBuilder.Entity<spWarehouseLayoutRecursive>(e => e.HasKey(e => e.WarehouseLayoutId));
            modelBuilder.HasDbFunction(() => GetWarehouseCountReport(default)).HasName("GetWarehouseCountReport");       
            modelBuilder.Entity<WarehouseCountFormReport>(e => e.HasKey(e => e.WarehouseLayoutQuantitiesId));

            //--------------------------Convert Date To UTC-----------------------------
            var dateTimeConverter = new ValueConverter<DateTime, DateTime>( v => v.ToUniversalTime(),v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
            var nullableDateTimeConverter = new ValueConverter<DateTime?, DateTime?>(
                v => v.HasValue ? v.Value.ToUniversalTime() : v,
                v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (entityType.IsKeyless)
                {
                    continue;
                }

                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(DateTime))
                    {
                        property.SetValueConverter(dateTimeConverter);
                    }
                    else if (property.ClrType == typeof(DateTime?))
                    {
                        property.SetValueConverter(nullableDateTimeConverter);
                    }
                }
            }
            //------------------------------------------------------------------------
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
        public IQueryable<spWarehouseLayoutRecursive> GetWarehouseLayoutRecursive(int parentId)
        => FromExpression(() => GetWarehouseLayoutRecursive(parentId));

        public IQueryable<WarehouseCountFormReport> GetWarehouseCountReport(int warehouseCountFormHeadeId)
        => FromExpression(() => GetWarehouseCountReport(warehouseCountFormHeadeId));               
    }
}
