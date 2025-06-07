
using Eefa.Commodity.Data.Entities;
using Eefa.Common.Data;
using Microsoft.EntityFrameworkCore;


namespace Eefa.Commodity.Data.Context
{
    public interface ICommodityUnitOfWork : IUnitOfWork
    {
        DbSet<BaseValue> BaseValues { get; set; }
        DbSet<BaseValueType> BaseValueTypes { get; set; }
        DbSet<BomItem> BomItems { get; set; }
        DbSet<Bom> Boms { get; set; }
        DbSet<BomValue> BomValues { get; set; }
        DbSet<BomValueHeader> BomValueHeaders { get; set; }
        DbSet<CategoryPropertyMapping> CategoryPropertyMappings { get; set; }
        DbSet<Entities.Commodity> Commodities { get; set; }
        DbSet<CommodityCategory> CommodityCategories { get; set; }
        DbSet<CommodityCategoryProperty> CommodityCategoryProperties { get; set; }
        DbSet<CommodityCategoryPropertyItem> CommodityCategoryPropertyItems { get; set; }
        DbSet<CommodityPropertyValue> CommodityPropertyValues { get; set; }
        DbSet<MeasureUnit> MeasureUnits { get; set; }
        DbSet<MeasureUnitConversion> MeasureUnitConversions { get; set; }
        DbSet<BomsView> BomsView { get; set; }
        DbSet<DocumentItem> DocumentItems { get; set; }
       DbSet<CommoditeisView> CommoditeisView { get; set; }


    }


}