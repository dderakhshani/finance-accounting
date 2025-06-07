using System.Collections.Generic;

public class ProfitAndLossReportModel
{
    public List<Level3> level3s { get; set; } = new List<Level3>();
    public List<Level2> level2s { get; set; } = new();
    public double SaleSum { get; set; }//جمع فروش ناخالص
    public double TotalOperatingIncome { get; set; }//جمع درآمد عملیاتی
    public double GrossProfit { get; set; }//سود ناخالص
    public double TotalOperatingCosts { get; set; }//جمع هزینه های عملیاتی
    public double OperatingProfitAndLoss { get; set; }//سود و زیان عملیاتی
    public double GrossProfitBeforeTax { get; set; }//سود نا خالص قبل از مالیات
    public double IncomeTaxEexpense { get; set; }//هزینه مالیات بر درآمد
    public double NetProfit { get; set; }//سود خالص
    public double SalesMargin { get; set; }//حاشیه فروش
    public double OperatingPprofitMargin { get; set; }//حاشیه سود عملیاتی
    public double ProfitMarigin { get; set; }//حاشیه سود
}
public class Level3
{
    public string Level3Code { get; set; }
    public string Leve3Name { get; set; }
    public double Price { get; set; }
}
public class Level2
{
    public string Level2Code { get; set; }
    public string Leve2Name { get; set; }
    public double Price { get; set; }
}