using System;
using System.ComponentModel.DataAnnotations;
using Eefa.Common;

namespace Eefa.Logistic.Domain
{
   
    [HasUniqueIndex]
    public partial class Prehension 
    {
        [Key]
       public int Id { get; set; } = default!;
       public string PrehensionCode { get; set; } = default!;
       public string TheAccountTitle { get; set; } = default!;
       public string ProductTypeTitle { get; set; } = default!;
       public string ProductTitle { get; set; } = default!;
       public string CarTypeTitle { get; set; } = default!;
       public string DriverName { get; set; } = default!;
       public string DriverPhone { get; set; } = default!;
       public string CarNumber { get; set; } = default!;
       public DateTime? PrehensionDateTime { get; set; } = default!;
       public string PrehensionBarNo { get; set; } = default!;
       public string PrehensionHavaleNo { get; set; } = default!;
       public double? UTransferPrice { get; set; } = default!;
       public double? TransferPrice { get; set; } = default!;
       public string TotalNetWt { get; set; } = default!;


    }
}
