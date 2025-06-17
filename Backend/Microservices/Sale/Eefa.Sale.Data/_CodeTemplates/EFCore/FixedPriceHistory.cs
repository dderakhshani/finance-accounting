using System;
using System.Collections.Generic;

namespace Eefa.Sale.Domain.Entities;

/// <summary>
/// قیمت تمام شده
/// </summary>
public partial class FixedPriceHistory
{
    public int Id { get; set; }

    /// <summary>
    /// کد کالا
    /// </summary>
    public int CommodityId { get; set; }

    /// <summary>
    /// قیمت
    /// </summary>
    public long Price { get; set; }

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
}
