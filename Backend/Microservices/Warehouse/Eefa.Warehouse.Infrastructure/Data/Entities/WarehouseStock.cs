using System;
using System.Collections.Generic;
using Eefa.Common;
using Eefa.Common.Data;
namespace Eefa.Warehouse.Infrastructure.Data.Entities;

public class WarehouseStock : BaseEntity 
{
    /// <summary>
    /// کد انبار
    /// </summary>
  public int WarehousId { get; set; }
    /// <summary>
    /// کد کالا
    /// </summary>
  public int CommodityId { get; set; }
  public double InitializeQuantity { get; set; }
    /// <summary>
    /// تعداد
    /// </summary>
  public double Quantity { get; set; }
  public int? YearId { get; set; }
    public virtual Warehous Warehous { get; set; } = null!;
}
