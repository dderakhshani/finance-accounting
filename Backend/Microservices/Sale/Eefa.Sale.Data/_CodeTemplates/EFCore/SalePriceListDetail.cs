using System;
using System.Collections.Generic;

namespace Eefa.Sale.Domain.Entities;

/// <summary>
/// لیست قیمت ریز محصولات
/// </summary>
public partial class SalePriceListDetail
{
    /// <summary>
    /// شناسه
    /// </summary>
    public int Id { get; set; }

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

    public virtual SalePriceList SalePriceList { get; set; } = null!;
}
