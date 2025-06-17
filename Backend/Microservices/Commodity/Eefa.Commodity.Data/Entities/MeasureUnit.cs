using System;
using System.Collections.Generic;
using Eefa.Common;
using Eefa.Common.Data;
namespace Eefa.Commodity.Data.Entities;

/// <summary>
/// واحد های اندازه گیری
/// </summary>
public class MeasureUnit : BaseEntity , IHierarchical
{
  public int? ParentId { get; set; }
    /// <summary>
    /// عنوان
    /// </summary>
  public string Title { get; set; } = null!;
    /// <summary>
    /// نام اختصاصی
    /// </summary>
  public string? UniqueName { get; set; }
    public virtual ICollection<CommodityCategory> CommodityCategories { get; set; } = new List<CommodityCategory>();
    public virtual ICollection<CommodityCategoryProperty> CommodityCategoryProperties { get; set; } = new List<CommodityCategoryProperty>();
    public virtual ICollection<DocumentItem> DocumentItems { get; set; } = new List<DocumentItem>();
    public virtual ICollection<MeasureUnit> InverseParent { get; set; } = new List<MeasureUnit>();
    public virtual ICollection<MeasureUnitConversion> MeasureUnitConversionDestinationMeasureUnits { get; set; } = new List<MeasureUnitConversion>();
    public virtual ICollection<MeasureUnitConversion> MeasureUnitConversionSourceMeasureUnits { get; set; } = new List<MeasureUnitConversion>();
    public virtual MeasureUnit? Parent { get; set; }
}
