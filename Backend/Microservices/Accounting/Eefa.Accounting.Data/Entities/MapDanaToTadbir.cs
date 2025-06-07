using System;
using System.Collections.Generic;
using DocumentFormat.OpenXml.Drawing;
using Eefa.Accounting.Data.Events.VoucherHead;
using Library.Common;
using System.ComponentModel.DataAnnotations.Schema;
using Library.Models;


namespace Eefa.Accounting.Data.Entities
{
    public class MapDanaToTadbir :  BaseEntity
    {
        public int? DanaID { get; set; }
        public string DanaCode { get; set; }
        public string TadbirCode { get; set; }
        public string TadbirAccountHead { get; set; }
        public int? Levelcode { get; set; }
        public string Condition { get; set; }
    }
}
