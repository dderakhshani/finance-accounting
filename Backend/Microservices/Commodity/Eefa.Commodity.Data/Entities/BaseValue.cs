using System;
using System.Collections.Generic;
using Eefa.Common;
using Eefa.Common.Data;
namespace Eefa.Commodity.Data.Entities;

/// <summary>
/// اطلاعات پایه 
/// </summary>
public class BaseValue : BaseEntity , IHierarchical
{
    /// <summary>
    /// کد نوع مقدار
    /// </summary>
  public int BaseValueTypeId { get; set; }
    /// <summary>
    /// کد والد
    /// </summary>
  public int? ParentId { get; set; }
    /// <summary>
    /// کد
    /// </summary>
  public string Code { get; set; } = null!;
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
    /// مقدار
    /// </summary>
  public string Value { get; set; } = null!;
    /// <summary>
    /// ترتیب نمایش 
    /// </summary>
  public int OrderIndex { get; set; }
    /// <summary>
    /// آیا فقط قابل خواندن است؟
    /// </summary>
  public bool IsReadOnly { get; set; }
    public virtual ICollection<CommodityCategoryProperty> CommodityCategoryProperties { get; set; } = new List<CommodityCategoryProperty>();
}
