
using Eefa.Commodity.Data.Entities;
using Eefa.Common.Data;
using Microsoft.EntityFrameworkCore;


namespace Eefa.Commodity.Data.Context
{
    public interface ICommodityUnitOfWork : IUnitOfWork
    {
        public  DbSet<BaseValue> BaseValues { get; set; }

        public  DbSet<BaseValueType> BaseValueTypes { get; set; }

        public  DbSet<Bom> Boms { get; set; }

        public DbSet<BomItem> BomItems { get; set; }

        public  DbSet<BomValue> BomValues { get; set; }

        public  DbSet<BomValueHeader> BomValueHeaders { get; set; }

        public  DbSet<BomsView> BomsViews { get; set; }

        public DbSet<CategoryPropertyMapping> CategoryPropertyMappings { get; set; }

        public DbSet<Entities.Commodity> Commodities { get; set; }

        public DbSet<CommodityCategory> CommodityCategories { get; set; }

        public DbSet<CommodityCategoryProperty> CommodityCategoryProperties { get; set; }

        public DbSet<CommodityCategoryPropertyItem> CommodityCategoryPropertyItems { get; set; }

        public DbSet<CommodityPropertyValue> CommodityPropertyValues { get; set; }

        public DbSet<DocumentItem> DocumentItems { get; set; }

        public DbSet<MeasureUnit> MeasureUnits { get; set; }

        public  DbSet<MeasureUnitConversion> MeasureUnitConversions { get; set; }
        DbSet<CommoditeisView> CommoditeisView { get; set; }
        DbSet<BomsView> BomsView { get; set; }


    }

}