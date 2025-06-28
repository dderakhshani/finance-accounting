using System;
using System.Collections.Generic;
using Eefa.Common;
using Eefa.Common.Data;
namespace Eefa.Warehouse.Infrastructure.Data.Entities;

/// <summary>
/// موقعیت های انبار
/// </summary>
public class WarehouseLayout : BaseEntity , IHierarchical
{
    /// <summary>
    /// کد انبار
    /// </summary>
  public int? WarehouseId { get; set; }
    /// <summary>
    /// کد والد
    /// </summary>
  public int? ParentId { get; set; }
    /// <summary>
    /// کد سطح
    /// </summary>
  public string? LevelCode { get; set; }
    /// <summary>
    /// عنوان
    /// </summary>
  public string Title { get; set; } = null!;
    /// <summary>
    /// ظرفیت
    /// </summary>
  public int Capacity { get; set; }
    /// <summary>
    /// سطح آخر هست
    /// </summary>
  public bool LastLevel { get; set; }
    /// <summary>
    /// نوع واحد کالا
    /// </summary>
  public int? UnitBaseTypeId { get; set; }
    /// <summary>
    /// ترتیب نمایش
    /// </summary>
  public int OrderIndex { get; set; }
  public int? CommodityId { get; set; }
  public double? Quantity { get; set; }
    public virtual ICollection<WarehouseLayout> InverseParent { get; set; } = new List<WarehouseLayout>();
    public virtual WarehouseLayout? Parent { get; set; }
    public virtual Warehous? Warehouse { get; set; }
}
