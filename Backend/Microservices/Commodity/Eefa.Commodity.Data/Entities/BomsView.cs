using System;
using System.Collections.Generic;
using Eefa.Common;
using Eefa.Common.Data;
namespace Eefa.Commodity.Data.Entities;

public class BomsView : BaseEntity 
{
  public int? RootId { get; set; }
  public string Title { get; set; } = null!;
  public string Description { get; set; } = null!;
  public string LevelCode { get; set; } = null!;
  public int CommodityCategoryId { get; set; }
  public bool IsActive { get; set; }
  public string? CommodityCategoryTitle { get; set; }
}
