using System;
using System.Collections.Generic;
using Eefa.Common;
using Eefa.Common.Data;
namespace Eefa.Commodity.Data.Entities;

/// <summary>
/// ریز اقلام اسناد
/// </summary>
public class DocumentItem : BaseEntity 
{
    /// <summary>
    /// کد سرفصل سند
    /// </summary>
  public int DocumentHeadId { get; set; }
    /// <summary>
    /// کد سال
    /// </summary>
  public int YearId { get; set; }
  public int CommodityId { get; set; }
    /// <summary>
    /// لوکیشن کالا در انبار 
    /// </summary>
  public int? WarehouseLayoutId { get; set; }
    /// <summary>
    /// نوع ورود1 خروج -1
    /// </summary>
  public int? IOMode { get; set; }
    /// <summary>
    /// سریال کالا
    /// </summary>
  public string? CommoditySerial { get; set; }
    /// <summary>
    /// قیمت در سیستم  درخواست 
    /// </summary>
  public long UnitBasePrice { get; set; }
    /// <summary>
    /// قیمت واحد 
    /// </summary>
  public double UnitPrice { get; set; }
    /// <summary>
    /// قیمت واحد تمام شده 
    /// </summary>
  public double? UnitPriceWithExtraCost { get; set; }
    /// <summary>
    /// قیمت پایه
    /// </summary>
  public double ProductionCost { get; set; }
    /// <summary>
    /// وزن کالا 
    /// </summary>
  public double Weight { get; set; }
    /// <summary>
    /// تعداد
    /// </summary>
  public double Quantity { get; set; }
    /// <summary>
    /// تعداد/مقدار باقی مانده 
    /// </summary>
  public double? RemainQuantity { get; set; }
    /// <summary>
    /// تعداد / مقدار فرعی
    /// </summary>
  public double? SecondaryQuantity { get; set; }
    /// <summary>
    /// نوع ارز
    /// </summary>
  public int? CurrencyBaseId { get; set; }
    /// <summary>
    /// مبلغ ارز
    /// </summary>
  public double? CurrencyPrice { get; set; }
    /// <summary>
    /// نرخ واحد تبدیل ارز
    /// </summary>
  public double? CurrencyRate { get; set; }
    /// <summary>
    /// تخفیف
    /// </summary>
  public long Discount { get; set; }
    /// <summary>
    /// شرح کالا
    /// </summary>
  public string? Description { get; set; }
    /// <summary>
    /// واحد شمارش اصلی کالا
    /// </summary>
  public int MainMeasureId { get; set; }
    /// <summary>
    /// واحد شمارش فعلی کالا
    /// </summary>
  public int DocumentMeasureId { get; set; }
    /// <summary>
    /// اعلام اشتباه بودن واحد کالا 
    /// </summary>
  public bool? IsWrongMeasure { get; set; }
    /// <summary>
    /// ضریب تبدیل به واحد اصلی
    /// </summary>
  public int? MeasureUnitConversionId { get; set; }
    /// <summary>
    /// نرخ تبدیل-فعلا استفاده نمیشود  
    /// </summary>
  public double? ConversionRatio { get; set; }
    /// <summary>
    /// شماره فرمول ساخت 
    /// </summary>
  public int? BomValueHeaderId { get; set; }
    /// <summary>
    /// آیدی درخواست مربوط به این کالا
    /// </summary>
  public int? RequestDocumentItemId { get; set; }
  public byte[]? RowVersion { get; set; }
    public virtual MeasureUnit DocumentMeasure { get; set; } = null!;
    public virtual MeasureUnitConversion? MeasureUnitConversion { get; set; }
}
