using System;
using System.Collections.Generic;
using Eefa.Common;
using Eefa.Common.Data;
namespace Eefa.Commodity.Data.Entities;

public class CommoditeisView : BaseEntity , IHierarchical
{
  public int? CommodityCategoryId { get; set; }
  public string? Code { get; set; }
  public string? SecondaryCode { get; set; }
  public string? Title { get; set; }
  public int? MeasureId { get; set; }
  public string LevelCode { get; set; } = null!;
  public int? ParentId { get; set; }
  public string? SearchTerm { get; set; }
  public bool? IsActive { get; set; }
  public string? CommodityNationalId { get; set; }
  public string? CommodityNationalTitle { get; set; }
  public string? Descriptions { get; set; }
  public int YearId { get; set; }
  public double? MinimumQuantity { get; set; }
  public double? MaximumQuantity { get; set; }
  public int? PricingTypeBaseId { get; set; }
  public string CategoryLevelCode { get; set; } = null!;
  public string? CategoryTitle { get; set; }
  public string? MeasureTitle { get; set; }
  public int BomsCount { get; set; }
}
