using System;
using System.Collections.Generic;
using Eefa.Common;
using Eefa.Common.Data;
namespace Eefa.Commodity.Data.Entities;

/// <summary>
/// مقادیر ویژگی های کالا
/// </summary>
public class CommodityPropertyValue : BaseEntity 
{
    /// <summary>
    /// کد کالا
    /// </summary>
  public int CommodityId { get; set; }
    /// <summary>
    /// کد ویژگی گروه
    /// </summary>
  public int CategoryPropertyId { get; set; }
    /// <summary>
    /// کد آیتم ویژگی مقدار 
    /// </summary>
  public int? ValuePropertyItemId { get; set; }
    /// <summary>
    /// مقدار
    /// </summary>
  public string? Value { get; set; }
    public virtual CommodityCategoryProperty CategoryProperty { get; set; } = null!;
    public virtual Commodity Commodity { get; set; } = null!;
    public virtual CommodityCategoryPropertyItem? ValuePropertyItem { get; set; }
}
