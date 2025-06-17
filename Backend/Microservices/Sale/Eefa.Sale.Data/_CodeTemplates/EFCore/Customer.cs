using System;
using System.Collections.Generic;

namespace Eefa.Sale.Domain.Entities;

public partial class Customer
{
    public int Id { get; set; }

    /// <summary>
    /// کد شخص 
    /// </summary>
    public int PersonId { get; set; }

    /// <summary>
    /// نوع مشتری 
    /// </summary>
    public int CustomerTypeBaseId { get; set; }

    /// <summary>
    /// کد اپراتور مرتبط با مشتری 
    /// </summary>
    public int CurrentAgentId { get; set; }

    /// <summary>
    /// شماره مشتری
    /// </summary>
    public string CustomerCode { get; set; } = null!;

    /// <summary>
    /// کد اقتصادی مشتری
    /// </summary>
    public string? EconomicCode { get; set; }

    /// <summary>
    /// توضیحات 
    /// </summary>
    public string? Description { get; set; }

    public bool IsActive { get; set; }

    /// <summary>
    /// کد گروه مشتری 
    /// </summary>
    public int AccountReferenceGroupId { get; set; }

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
