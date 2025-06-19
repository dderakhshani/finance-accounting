using System;
using System.Collections.Generic;

namespace Eefa.Sale.Domain.Entities;

/// <summary>
/// لیست قیمت محصولات
/// </summary>
public partial class SalePriceList
{
    /// <summary>
    /// شناسه
    /// </summary>
    public int Id { get; set; }

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

    /// <summary>
    /// نقش صاحب سند
    /// </summary>
    public int OwnerRoleId { get; set; }

    /// <summary>
    /// ایجاد کننده
    /// </summary>
    public int CreatedById { get; set; }

    /// <summary>
    /// تاریخ و زمان ایجاد
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// اصلاح کننده
    /// </summary>
    public int? ModifiedById { get; set; }

    /// <summary>
    /// تاریخ و زمان اصلاح
    /// </summary>
    public DateTime ModifiedAt { get; set; }

    /// <summary>
    /// آیا حذف شده است؟
    /// </summary>
    public bool IsDeleted { get; set; }

    public virtual ICollection<SalePriceList> InverseParent { get; set; } = new List<SalePriceList>();

    public virtual SalePriceList? Parent { get; set; }

    public virtual ICollection<SalePriceListDetail> SalePriceListDetails { get; set; } = new List<SalePriceListDetail>();
}
