using System;
using System.Collections.Generic;
using Eefa.Common;
using Eefa.Common.Data;
namespace Eefa.Commodity.Data.Entities;

/// <summary>
/// فرمول های ساخت
/// </summary>
public class Bom : BaseEntity 
{
  public int? RootId { get; set; }
  public string Title { get; set; } = null!;
  public string Description { get; set; } = null!;
  public bool IsActive { get; set; }
    /// <summary>
    /// کد سطح
    /// </summary>
  public string LevelCode { get; set; } = null!;
    /// <summary>
    /// کد گروه کالا
    /// </summary>
  public int CommodityCategoryId { get; set; }
    public virtual ICollection<BomItem> BomItems { get; set; } = new List<BomItem>();
    public virtual ICollection<BomValueHeader> BomValueHeaders { get; set; } = new List<BomValueHeader>();
}
