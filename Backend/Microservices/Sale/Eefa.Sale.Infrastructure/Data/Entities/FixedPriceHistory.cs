using System;
using System.Collections.Generic;
using Eefa.Common;
using Eefa.Common.Data;
namespace Eefa.Sale.Infrastructure.Data.Entities;

/// <summary>
/// قیمت تمام شده
/// </summary>
public class FixedPriceHistory : BaseEntity 
{
    /// <summary>
    /// کد کالا
    /// </summary>
  public int CommodityId { get; set; }
    /// <summary>
    /// قیمت
    /// </summary>
  public long? Price { get; set; }
  public double? DollarPrice { get; set; }
    /// <summary>
    /// تاریخ شروع
    /// </summary>
  public DateTime StartDate { get; set; }
}
