using System;
using System.Collections.Generic;
using Eefa.Common;
using Eefa.Common.Data;
namespace Eefa.Commodity.Data.Entities;

/// <summary>
/// کالا ها
/// </summary>
public class Commodity : BaseEntity , IHierarchical
{
    /// <summary>
    /// شناسه ملی کالا 
    /// </summary>
  public string? CommodityNationalId { get; set; }
    /// <summary>
    /// عنوان شناسه ملی کالا
    /// </summary>
  public string? CommodityNationalTitle { get; set; }
    /// <summary>
    /// کد والد
    /// </summary>
  public int? ParentId { get; set; }
    /// <summary>
    /// کد گروه کالا
    /// </summary>
  public int? CommodityCategoryId { get; set; }
    /// <summary>
    /// کد سطح
    /// </summary>
  public string LevelCode { get; set; } = null!;
    /// <summary>
    /// کد محصول
    /// </summary>
  public string? Code { get; set; }
    /// <summary>
    /// کد محصول
    /// </summary>
  public string? SecondaryCode { get; set; }
  public string? ThirdCode { get; set; }
    /// <summary>
    /// عنوان
    /// </summary>
  public string? Title { get; set; }
    /// <summary>
    /// توضیحات
    /// </summary>
  public string? Descriptions { get; set; }
    /// <summary>
    /// کد واحد اندازه گیری
    /// </summary>
  public int? MeasureId { get; set; }
    /// <summary>
    /// کد سال
    /// </summary>
  public int YearId { get; set; }
    /// <summary>
    /// حداقل تعداد
    /// </summary>
  public double? MinimumQuantity { get; set; }
    /// <summary>
    /// حداکثر تعداد
    /// </summary>
  public double? MaximumQuantity { get; set; }
    /// <summary>
    /// نوع محاسبه قیمت
    /// </summary>
  public int? PricingTypeBaseId { get; set; }
  public int? CommodityTypeBaseId { get; set; }
  public int? InventoryType { get; set; }
    /// <summary>
    /// فعال است 
    /// </summary>
  public bool? IsActive { get; set; }
  public byte[]? RowVersion { get; set; }
    public virtual ICollection<BomItem> BomItems { get; set; } = new List<BomItem>();
    public virtual ICollection<BomValue> BomValues { get; set; } = new List<BomValue>();
    public virtual ICollection<CommodityPropertyValue> CommodityPropertyValues { get; set; } = new List<CommodityPropertyValue>();
}
