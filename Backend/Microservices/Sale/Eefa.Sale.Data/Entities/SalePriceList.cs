using System;
using System.Collections.Generic;
using Eefa.Common;
using Eefa.Common.Data;
namespace Eefa.Sale.Data.Entities;

/// <summary>
/// لیست قیمت محصولات
/// </summary>
public class SalePriceList : BaseEntity , IHierarchical
{
  public int? RootId { get; set; }
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
    /// توضیحات
    /// </summary>
  public string? Descriptions { get; set; }
    /// <summary>
    /// نوع لیست قیمتی 
    /// </summary>
  public int? AccountReferenceGroupId { get; set; }
    /// <summary>
    /// تاریخ شروع
    /// </summary>
  public DateTime StartDate { get; set; }
    public virtual ICollection<SalePriceList> InverseParent { get; set; } = new List<SalePriceList>();
    public virtual SalePriceList? Parent { get; set; }
    public virtual ICollection<SalePriceListDetail> SalePriceListDetails { get; set; } = new List<SalePriceListDetail>();
}
