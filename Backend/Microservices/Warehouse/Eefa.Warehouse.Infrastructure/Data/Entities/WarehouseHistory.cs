using System;
using System.Collections.Generic;
using Eefa.Common;
using Eefa.Common.Data;
namespace Eefa.Warehouse.Infrastructure.Data.Entities;

/// <summary>
/// تاریخچه انبار ها
/// </summary>
public class WarehouseHistory : BaseEntity 
{
    /// <summary>
    /// کد کالا
    /// </summary>
  public int Commodityld { get; set; }
  public int? WarehousesId { get; set; }
  public int? InputOutput { get; set; }
    /// <summary>
    /// نوع عملیات
    /// </summary>
  public int Mode { get; set; }
  public int? DocumentHeadId { get; set; }
    /// <summary>
    /// شماره آیتم در سند
    /// </summary>
  public int? DocumentItemId { get; set; }
    /// <summary>
    /// تعداد
    /// </summary>
  public double Quantity { get; set; }
}
