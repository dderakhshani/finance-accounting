using System;
using System.Collections.Generic;
using Eefa.Common;
using Eefa.Common.Data;
namespace Eefa.Commodity.Data.Entities;

/// <summary>
/// آیتم های ویژگی گروه کالا
/// </summary>
public class CommodityCategoryPropertyItem : BaseEntity , IHierarchical
{
    /// <summary>
    /// کد ویژگی گروه
    /// </summary>
  public int CategoryPropertyId { get; set; }
    /// <summary>
    /// کد والد
    /// </summary>
  public int? ParentId { get; set; }
    /// <summary>
    /// عنوان
    /// </summary>
  public string Title { get; set; } = null!;
    /// <summary>
    /// نام اختصاصی
    /// </summary>
  public string UniqueName { get; set; } = null!;
    /// <summary>
    /// کد
    /// </summary>
  public string? Code { get; set; }
    /// <summary>
    /// ترتیب نمایش
    /// </summary>
  public int OrderIndex { get; set; }
    /// <summary>
    /// فعال است؟
    /// </summary>
  public bool IsActive { get; set; }
    public virtual CommodityCategoryProperty CategoryProperty { get; set; } = null!;
    public virtual ICollection<CategoryPropertyMapping> CategoryPropertyMappingCommodityCategoryPropertyItems1Navigation { get; set; } = new List<CategoryPropertyMapping>();
    public virtual ICollection<CategoryPropertyMapping> CategoryPropertyMappingCommodityCategoryPropertyItems2Navigation { get; set; } = new List<CategoryPropertyMapping>();
    public virtual ICollection<CommodityPropertyValue> CommodityPropertyValues { get; set; } = new List<CommodityPropertyValue>();
    public virtual ICollection<CommodityCategoryPropertyItem> InverseParent { get; set; } = new List<CommodityCategoryPropertyItem>();
    public virtual CommodityCategoryPropertyItem? Parent { get; set; }
    public string LevelCode { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
}
