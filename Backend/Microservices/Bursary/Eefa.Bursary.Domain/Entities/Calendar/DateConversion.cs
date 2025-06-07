using Eefa.Common.Data;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eefa.Bursary.Domain.Entities.Calendar
{
    [Table("DateConversion", Schema = "cal")]
    public class DateConversion : BaseEntity
    {
        public int PersianDate { get; set; }
        public string PersianDateSlash { get; set; }
        public int GreDate { get; set; }
        public string GreDateSlash { get; set; }
        public DateTime GreDateTime { get; set; }
        public int PersianYM { get; set; }
        public int GreYM { get; set; }
        public int PersianYear { get; set; }
        public int GreYear { get; set; }
        public int FiscalYear { get; set; }
        public int PersianDow { get; set; }
        public string PersianDowName { get; set; }
        public int PersianMonth { get; set; }
        public bool IsHoliday { get; set; }
    }
}
