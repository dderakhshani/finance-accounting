using System;
using System.Collections.Generic;
using Eefa.Common;
using Eefa.Common.Data;
namespace Eefa.Commodity.Data.Entities;

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
    ///  0 NONE - 1 LIFO - 2 FIFO حالت ورود اطلاعات  
    /// </summary>
  public int EntryMode { get; set; }
    /// <summary>
    /// 0 آزاد 
    /// 1 فقط ورودی 
    /// 2 فقط خروجی 
    /// 3 قفل موقت 
    /// 4 قفل دائم 
    /// 
    /// </summary>
  public int Status { get; set; }
    /// <summary>
    /// شماره شروع
    /// </summary>
  public int StartIndex { get; set; }
    /// <summary>
    /// شماره پایان
    /// </summary>
  public int? EndIndex { get; set; }
    /// <summary>
    /// نوع واحد کالا
    /// </summary>
  public int? UnitBaseTypeId { get; set; }
    /// <summary>
    /// ترتیب نمایش
    /// </summary>
  public int OrderIndex { get; set; }
    /// <summary>
    /// موقعیت پیش فرض
    /// </summary>
  public bool? IsDefault { get; set; }
    /// <summary>
    /// زیر مجموعه بصورت سریال است
    /// </summary>
  public bool IsChildSequncial { get; set; }
    public virtual ICollection<WarehouseLayout> InverseParent { get; set; } = new List<WarehouseLayout>();
    public virtual WarehouseLayout? Parent { get; set; }
    public virtual Warehous? Warehouse { get; set; }
}
