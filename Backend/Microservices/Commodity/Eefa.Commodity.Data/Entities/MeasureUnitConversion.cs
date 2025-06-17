using System;
using System.Collections.Generic;
using Eefa.Common;
using Eefa.Common.Data;
namespace Eefa.Commodity.Data.Entities;

/// <summary>
/// تبدیل واحد های اندازه گیری
/// </summary>
public class MeasureUnitConversion : BaseEntity 
{
    /// <summary>
    /// واحد اندازه گیری اولیه
    /// </summary>
  public int SourceMeasureUnitId { get; set; }
    /// <summary>
    /// واحد اندازه گیری ثانویه
    /// </summary>
  public int DestinationMeasureUnitId { get; set; }
    /// <summary>
    /// ضریب تبدیل
    /// </summary>
  public double? Ratio { get; set; }
    public virtual MeasureUnit DestinationMeasureUnit { get; set; } = null!;
    public virtual ICollection<DocumentItem> DocumentItems { get; set; } = new List<DocumentItem>();
    public virtual MeasureUnit SourceMeasureUnit { get; set; } = null!;
}
