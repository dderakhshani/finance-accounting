using System;
using System.Collections.Generic;
using Eefa.Common;
using Eefa.Common.Data;
namespace Eefa.Commodity.Data.Entities;

/// <summary>
/// ویژگی های گروه کالا
/// </summary>
public class CommodityCategoryProperty : BaseEntity , IHierarchical
{
    /// <summary>
    /// کد والد
    /// </summary>
  public int? ParentId { get; set; }
    /// <summary>
    /// کد گروه
    /// </summary>
  public int? CategoryId { get; set; }
    /// <summary>
    /// کد سطح
    /// </summary>
  public string LevelCode { get; set; } = null!;
    /// <summary>
    /// نام اختصاصی
    /// </summary>
  public string UniqueName { get; set; } = null!;
    /// <summary>
    /// عنوان
    /// </summary>
  public string Title { get; set; } = null!;
    /// <summary>
    /// کد واحد اندازه گیری
    /// </summary>
  public int? MeasureId { get; set; }
    /// <summary>
    /// قوانین حاکم بر مولفه
    /// </summary>
  public int? PropertyTypeBaseId { get; set; }
    /// <summary>
    /// ترتیب نمایش
    /// </summary>
  public int OrderIndex { get; set; }
    public virtual CommodityCategory? Category { get; set; }
    public virtual ICollection<CommodityCategoryPropertyItem> CommodityCategoryPropertyItems { get; set; } = new List<CommodityCategoryPropertyItem>();
    public virtual ICollection<CommodityPropertyValue> CommodityPropertyValues { get; set; } = new List<CommodityPropertyValue>();
    public virtual ICollection<CommodityCategoryProperty> InverseParent { get; set; } = new List<CommodityCategoryProperty>();
    public virtual MeasureUnit? Measure { get; set; }
    public virtual CommodityCategoryProperty? Parent { get; set; }
    public virtual BaseValue? PropertyTypeBase { get; set; }
}
