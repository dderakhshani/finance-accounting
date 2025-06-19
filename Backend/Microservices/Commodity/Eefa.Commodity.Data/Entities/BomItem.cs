using System;
using System.Collections.Generic;
using Eefa.Common;
using Eefa.Common.Data;
namespace Eefa.Commodity.Data.Entities;

public class BomItem : BaseEntity 
{
  public int BomId { get; set; }
  public int? SubCategoryId { get; set; }
  public int? CommodityId { get; set; }
    public virtual Bom Bom { get; set; } = null!;
    public virtual Commodity? Commodity { get; set; }
}
