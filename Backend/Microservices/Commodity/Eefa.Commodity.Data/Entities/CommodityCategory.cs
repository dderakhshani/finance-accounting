using System;
using System.Collections.Generic;
using Eefa.Common;
using Eefa.Common.Data;
namespace Eefa.Commodity.Data.Entities;

/// <summary>
/// گروه های کالا
/// </summary>
public class CommodityCategory : BaseEntity , IHierarchical
{
    /// <summary>
    /// کد والد
    /// </summary>
  public int? ParentId { get; set; }
    /// <summary>
    /// کد سطح
    /// </summary>
  public string LevelCode { get; set; } = null!;
  public string Code { get; set; } = null!;
    /// <summary>
    /// 1 کد سازی برا اساس گروه
    /// 2 کد سازی بر اساس مشخصات
    /// </summary>
  public int CodingMode { get; set; }
    /// <summary>
    /// عنوان
    /// </summary>
  public string? Title { get; set; }
    /// <summary>
    /// کدواحد اصلی کالا
    /// </summary>
  public int? MeasureId { get; set; }
    /// <summary>
    /// ترتیب نمایش
    /// </summary>
  public int OrderIndex { get; set; }
    /// <summary>
    /// this.Parent().Commodities
    /// </summary>
  public bool? RequireParentProduct { get; set; }
    /// <summary>
    /// آیا فقط قابل خواندن است؟
    /// </summary>
  public bool IsReadOnly { get; set; }
  public byte[]? RowVersion { get; set; }
    public virtual ICollection<CommodityCategoryProperty> CommodityCategoryProperties { get; set; } = new List<CommodityCategoryProperty>();
    public virtual ICollection<CommodityCategory> InverseParent { get; set; } = new List<CommodityCategory>();
    public virtual MeasureUnit? Measure { get; set; }
    public virtual CommodityCategory? Parent { get; set; }
}
