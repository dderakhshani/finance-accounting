using System;
using System.Collections.Generic;
using Eefa.Common;
using Eefa.Common.Data;
namespace Eefa.Commodity.Data.Entities;

/// <summary>
/// مقادیر فرمول ساخت
/// </summary>
public class BomValue : BaseEntity 
{
    /// <summary>
    /// کد سند فرمول ساخت
    /// </summary>
  public int BomValueHeaderId { get; set; }
  public int BomWarehouseId { get; set; }
    /// <summary>
    /// کد کالای مصرفی
    /// </summary>
  public int UsedCommodityId { get; set; }
    /// <summary>
    /// مقدار
    /// </summary>
  public double Value { get; set; }
    public virtual BomValueHeader BomValueHeader { get; set; } = null!;
    public virtual Commodity UsedCommodity { get; set; } = null!;
}
