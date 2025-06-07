using Eefa.Common;
using Eefa.Common.Data;
using Eefa.Purchase.Domain;
using Eefa.Purchase.Domain.Aggregates.InvoiceAggregate;
using Eefa.Purchase.Domain.Entities;
using Eefa.Purchase.Domain.Entities.SqlView;
using Eefa.Purchase.Infrastructure.Context.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;

namespace Eefa.Purchase.Infrastructure.Context
{
    public partial class PurchaseContext : AuditableDbContext
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
        public virtual DbSet<Invoice> DocumentHeads { get; set; } = default!;
       
        public virtual DbSet<DocumentItem> DocumentItems { get; set; } = default!;
        public virtual DbSet<DocumentNumberingFormat> DocumentNumberingFormats { get; set; } = default!;
        public virtual DbSet<DocumentPayment> DocumentPayments { get; set; } = default!;
        public virtual DbSet<DocumentHeadExtend> DocumentHeadExtend { get; set; } = default!;
        public virtual DbSet<Document> Document { get; set; } = default!;
        public virtual DbSet<Driver> Drivers { get; set; } = default!;
        public virtual DbSet<Freight> Freights { get; set; } = default!;
        public virtual DbSet<MeasureUnit> MeasureUnits { get; set; } = default!;
        public virtual DbSet<MeasureUnitConversion> MeasureUnitConversions { get; set; } = default!;
        public virtual DbSet<Stock> Stocks { get; set; } = default!;
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
        public virtual DbSet<Permissions> Permissions { get; set; } = default!;

        public virtual DbSet<DocumentAttachment> DocumentAttachment { get; set; } = default!;
        public virtual DbSet<Attachment> Attachment { get; set; } = default!;

        //-------------------------------View-----------------------------------------
        public virtual DbSet<InvoiceView> InvoiceView { get; set; } = default!;
        public virtual DbSet<AccountReferenceView> AccountReferenceView { get; set; } = default!;
        public virtual DbSet<ReceiptInvoiceView> ReceiptInvoiceView { get; set; } = default!;
        public virtual DbSet<ViewCommodity> ViewCommodity { get; set; } = default!;
        public virtual DbSet<ReceiptItemsView> ReceiptItemsView { get; set; } = default!;
        public virtual DbSet<AccountReferenceEmployeeView> AccountReferenceEmployeeView { get; set; } = default!;

        public PurchaseContext(DbContextOptions<PurchaseContext> options, ICurrentUserAccessor currentUserAccessor)
           : base(options, currentUserAccessor)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");
            //--------------------------Table---------------------------------------
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
            modelBuilder.ApplyConfiguration(new StockConfiguration());
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
            modelBuilder.ApplyConfiguration(new PermissionsConfiguration());
            modelBuilder.ApplyConfiguration(new DocumentAttachmentConfiguration());
            modelBuilder.ApplyConfiguration(new AttachmentConfiguration());

            //--------------------View------------------------------------------------
            modelBuilder.ApplyConfiguration(new InvoiceViewConfiguration());
           
            modelBuilder.ApplyConfiguration(new AccountReferenceViewConfiguration());
            modelBuilder.ApplyConfiguration(new ReceiptInvoiceViewConfiguration());
            modelBuilder.ApplyConfiguration(new ViewCommodityConfiguration());
            modelBuilder.ApplyConfiguration(new ReceiptItemsViewConfiguration());
            modelBuilder.ApplyConfiguration(new AccountReferenceEmployeeViewConfiguration());

            
            //-------------------------------------------------------------------------
            //--------------------------Convert Date To UTC-----------------------------
            var dateTimeConverter = new ValueConverter<DateTime, DateTime>(v => v.ToUniversalTime(), v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
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
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
