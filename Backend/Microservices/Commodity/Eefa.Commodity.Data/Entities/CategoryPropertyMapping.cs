using System;
using System.Collections.Generic;
using Eefa.Common;
using Eefa.Common.Data;
namespace Eefa.Commodity.Data.Entities;

/// <summary>
/// نگاشت بین گروه ها و مشخصات کالا
/// </summary>
public class CategoryPropertyMapping : BaseEntity 
{
    /// <summary>
    /// آیتم ویژگی گروه محصول1
    /// </summary>
  public int CommodityCategoryPropertyItems1 { get; set; }
    /// <summary>
    /// آیتم ویژگی گروه محصول2
    /// </summary>
  public int CommodityCategoryPropertyItems2 { get; set; }
    public virtual CommodityCategoryPropertyItem CommodityCategoryPropertyItems1Navigation { get; set; } = null!;
    public virtual CommodityCategoryPropertyItem CommodityCategoryPropertyItems2Navigation { get; set; } = null!;
}
