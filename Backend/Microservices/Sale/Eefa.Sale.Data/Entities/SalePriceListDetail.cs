using System;
using System.Collections.Generic;
using Eefa.Common;
using Eefa.Common.Data;
namespace Eefa.Sale.Data.Entities;

/// <summary>
/// لیست قیمت ریز محصولات
/// </summary>
public class SalePriceListDetail : BaseEntity 
{
    /// <summary>
    /// لیست قیمت فروش
    /// </summary>
  public int SalePriceListId { get; set; }
    /// <summary>
    /// کد کالا
    /// </summary>
  public int CommodityId { get; set; }
    /// <summary>
    /// نوع مشتری
    /// </summary>
  public int? _ChildAccountReferenceGroupId { get; set; }
    public virtual SalePriceList SalePriceList { get; set; } = null!;
}
