using System;
using System.Collections.Generic;
using Eefa.Common;
using Eefa.Common.Data;
namespace Eefa.Commodity.Data.Entities;

/// <summary>
/// مواد فرمول ساخت
/// </summary>
public class BomValueHeader : BaseEntity 
{
    /// <summary>
    /// کد فرمول ساخت
    /// </summary>
  public int BomId { get; set; }
  public string? Name { get; set; }
    /// <summary>
    /// کد کالا
    /// </summary>
  public int CommodityId { get; set; }
    /// <summary>
    /// تاریخ فرمول ساخت
    /// </summary>
  public DateTime BomDate { get; set; }
    public virtual Bom Bom { get; set; } = null!;
    public virtual ICollection<BomValue> BomValues { get; set; } = new List<BomValue>();
}
