using System;
using System.Collections.Generic;
using Eefa.Common;
using Eefa.Common.Data;
namespace Eefa.Sale.Infrastructure.Data.Entities;

public class Customer : BaseEntity 
{
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
}
