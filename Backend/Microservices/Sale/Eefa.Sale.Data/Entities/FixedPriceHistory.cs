using System;
using System.Collections.Generic;
using Eefa.Common;
using Eefa.Common.Data;
namespace Eefa.Sale.Data.Entities;

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
  public long Price { get; set; }
    /// <summary>
    /// تاریخ شروع
    /// </summary>
  public DateTime StartDate { get; set; }
}
