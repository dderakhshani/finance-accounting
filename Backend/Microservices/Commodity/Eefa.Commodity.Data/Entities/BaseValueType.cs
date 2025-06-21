using System;
using System.Collections.Generic;
using Eefa.Common;
using Eefa.Common.Data;
namespace Eefa.Commodity.Data.Entities;

/// <summary>
/// نوع اطلاعات پایه 
/// </summary>
public class BaseValueType : BaseEntity , IHierarchical
{
    /// <summary>
    /// کد والد
    /// </summary>
  public int? ParentId { get; set; }
    /// <summary>
    /// کد سطح
    /// </summary>
  public string LevelCode { get; set; } = null!;
    /// <summary>
    /// عنوان
    /// </summary>
  public string Title { get; set; } = null!;
    /// <summary>
    /// نام اختصاصی
    /// </summary>
  public string UniqueName { get; set; } = null!;
    /// <summary>
    /// نام گروه
    /// </summary>
  public string? GroupName { get; set; }
    /// <summary>
    /// آیا فقط قابل خواندن است؟
    /// </summary>
  public bool IsReadOnly { get; set; }
    /// <summary>
    /// زیر سیستم
    /// </summary>
  public string? SubSystem { get; set; }
    public virtual ICollection<BaseValueType> InverseParent { get; set; } = new List<BaseValueType>();
    public virtual BaseValueType? Parent { get; set; }
}
