using System;

namespace Eefa.Inventory.Domain
{
    public class Stp_FreightPaysParam
    {
        public int? AccountReferenceId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        

    }
    public class spGetTotalWeightProductParam
    {
      
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }


    }
    public class spUpdateWarehouseLayoutParam
    {

        public int warehouseId { get; set; }
        public int UserId { get; set; }
        

    }
    public class ArchiveDocumentHeadsByDocumentDateParam
    {

        public int WarehouseId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int ? DocumentStauseBaseValue { get; set; }
        public int UserId { get; set; }


    }

    public class spInsertDocumentHeadsParam
    {
        public string jsonData { get; set; }
       
    }
}
