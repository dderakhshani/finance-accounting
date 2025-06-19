using System;
using System.Collections.Generic;
using Eefa.Common;
using Eefa.Common.Data;
namespace Eefa.Commodity.Data.Entities;

/// <summary>
/// انبارها
/// </summary>
public class Warehous : BaseEntity , IHierarchical
{
    /// <summary>
    /// کد والد
    /// </summary>
  public int? ParentId { get; set; }
    /// <summary>
    /// کد سطح
    /// </summary>
  public string? LevelCode { get; set; }
    /// <summary>
    /// سرفصل حساب 
    /// </summary>
  public int AccountHeadId { get; set; }
  public int? AccountRererenceGroupId { get; set; }
    /// <summary>
    /// تفصیل شناور 
    /// </summary>
  public int? AccountReferenceId { get; set; }
    /// <summary>
    /// عنوان
    /// </summary>
  public string Title { get; set; } = null!;
    /// <summary>
    /// مجوز دسترسی به انبار
    /// </summary>
  public string? AccessPermission { get; set; }
    /// <summary>
    /// فعال
    /// </summary>
  public bool IsActive { get; set; }
    /// <summary>
    /// کد گروه کالا
    /// </summary>
  public int? CommodityCategoryId { get; set; }
    /// <summary>
    /// ترتیب نمایش
    /// </summary>
  public int? Sort { get; set; }
    /// <summary>
    /// کد تدبیر
    /// </summary>
  public int? TadbirCode { get; set; }
    /// <summary>
    /// قابل شمارش 
    /// </summary>
  public bool? Countable { get; set; }
  public byte[]? RowVersion { get; set; }
    public virtual ICollection<Warehous> InverseParent { get; set; } = new List<Warehous>();
    public virtual Warehous? Parent { get; set; }
    public virtual ICollection<WarehouseLayout> WarehouseLayouts { get; set; } = new List<WarehouseLayout>();
}
