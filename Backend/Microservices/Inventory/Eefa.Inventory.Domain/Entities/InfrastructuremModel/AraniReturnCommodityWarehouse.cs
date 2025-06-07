using System;

namespace Eefa.Inventory.Domain
{


    public class AraniReturnCommodityWarehouseModel
    {
        public ReturnResult[] result { get; set; }
        public ReturnItem[] items { get; set; }
    }

    public class ReturnResult
    {
        public int Id { get; set; }
        public string DarkhastKonandeh { get; set; }
        public DateTime Tarikh_Sabt { get; set; }
        public string Vaziat { get; set; }
        public string CodePersoneli { get; set; }
        public string Mobile { get; set; }

        public string EllatKhoroj { get; set; }
        public bool BargashtBeAnbar { get; set; }

    }

    public class ReturnItem
    {
        public int Id { get; set; }
        public string Vaziat { get; set; }
        public string Kala_Name { get; set; }
        public string Kala_Code { get; set; }
        public string Kala_Vahed { get; set; }
        public int Megdar_Khorooj { get; set; }
        public string Tarikh_KhorojKala { get; set; }
        public string CodeKootah { get; set; }
        

    }


}
