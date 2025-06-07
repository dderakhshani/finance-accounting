using System;

namespace Eefa.Inventory.Domain
{
   

    public class AraniRequestCommodityWarehouseModel
    {
        public Result[] result { get; set; }
        public Item[] items { get; set; }
    }

    public class Result
    {
        public int Id { get; set; }
        public string DarkhastKonandeh { get; set; }
        public string DarkhastKonandeh_Code { get; set; }
        public string DarkhastKonandeh_Mobile { get; set; }
        public DateTime Tarikh { get; set; }
        public string Tarikh_Jalali { get; set; }
        public int Vaziat { get; set; }
        public string Vaziat_Name { get; set; }
        public string PeykeAnbar_Name { get; set; }
        public bool SabtShodeTavasoteAnbar { get; set; }
        public bool Fori { get; set; }
        
    }

    public class Item
    {
        public int Id { get; set; }
        public int AnbarGhataAt_Kala_Id { get; set; }
        public string Code { get; set; }
        public string CodeKootah { get; set; }
        
        public string KalaCodeName { get; set; }
        public float Tedad { get; set; }
        public string Vahed_Name { get; set; }
        public string TozihateAnbar { get; set; }
        public string MahaleEstefade_Name { get; set; }
        public string MahaleEstefadeDetail_Name { get; set; }
        public bool Daghi { get; set; }
        public string SharheSarparast { get; set; }
        public bool BargashteDaghi { get; set; }

        public string Mogheiat { get; set; }

        public float TedadAnbarGhatat { get; set; }
        public float TedadAnbarMasrafi { get; set; }
        public float TedadAnbarDaghi { get; set; }
        public float TedadAnbarZakhire { get; set; }
    }

}
