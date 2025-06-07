using Eefa.Bursary.Domain.Entities.Definitions;
using Eefa.Common.Data;
using System;

namespace Eefa.Bursary.Domain.Entities
{
    /// <summary>
   // &#1662;&#1585;&#1583;&#1575;&#1582;&#1578; &#1705;&#1585;&#1575;&#1740;&#1607; &#1581;&#1605;&#1604; &#1576;&#1575;&#1585;
    /// </summary>
    public partial class FreightPays : BaseEntity
    {
   
        public string ReceiptNo { get; set; } = default!;
        public int? FreightPayListId { get; set; }
        public string PersianReceiptDate { get; set; } = default!;
        public int SourceId { get; set; } = default!;
        public int? FreightId { get; set; }
        public long FreightAmount { get; set; } = default!;
        public int Fee { get; set; } = default!;
        public int LoadWeight { get; set; } = default!;
        public long DriverId { get; set; } = default!;
        public string PlateNo { get; set; } = default!;
        public int VehicleTypeId { get; set; } = default!;
        public string VehicleLoad { get; set; } = default!;
        public string AccountSheba { get; set; } = default!;
        public DateTime DateTime { get; set; } = default!;
        public string? NameShebaAccounts { get; set; }
        public string? FamilShebaAccounts { get; set; }
         
        public short? Status { get; set; }
        public string? OrderNumber { get; set; }
        public string? WaybillNumber { get; set; }
        public short? IsBankFile { get; set; }
        public DateTime ReceiptDate { get; set; } = default!;
        public long? OtherAmount { get; set; }
        public DateTime? BankFileDate { get; set; }
        public string? SSNAcountSheba { get; set; }
        public int? DriverAccountBankId { get; set; }

        /// <summary>
//نقش صاحب سند
        /// </summary>
         

        /// <summary>
//ایجاد کننده
        /// </summary>
         

        /// <summary>
//تاریخ و زمان ایجاد
        /// </summary>
         

        /// <summary>
//اصلاح کننده
        /// </summary>
         

        /// <summary>
//تاریخ و زمان اصلاح
        /// </summary>
         

        public virtual FreightPayList FreightPayList { get; set; } = default!;
        public virtual BaseValues Source { get; set; } = default!;
        public virtual BaseValues VehicleType { get; set; } = default!;
    }
}
