using System;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using Eefa.Common;

namespace Eefa.Inventory.Domain
{
   
    [HasUniqueIndex]
    public partial class CommodityPropertyWithThicknessView
    {
        [Key]
        public int Id { get; set; } = default!;
        public string size { get; set; } = default!;
        
        public string thickness { get; set; } = default!;
        public string Weight { get; set; } = default!;
        public string FactorNumber { get; set; } = default!;
        public double? Quantity { get; set; } = default!;
        public int? CommodityId { get; set; } = default!;

        public int? DocumentItemId { get; set; } = default!;
        public int? DocumentId { get; set; } = default!;
        public DateTime? DocumentDate { get; set; } = default!;
        public int? CodeVoucherGroupId { get; set; } = default!;



        //size.Value As size,
        //cast(w.Value as float) AS Weight,
        //cast(f.Value as float) AS FactorNumber,
        //t.Value  as thickness,
        //cast(v.Quantity as float) Quantity,
        //v.Commodityld As CommodityId,
        //v.DocumentItemId,
        //v.DocumentId,
        //v.DocumentDate,
        //cast(v.CodeVoucherGroupId As int) AS CodeVoucherGroupId,
        //v.Id




    }
}
