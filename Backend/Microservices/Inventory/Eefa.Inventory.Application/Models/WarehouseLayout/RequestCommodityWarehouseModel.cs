using System;
using System.Collections.Generic;

namespace Eefa.Inventory.Application
{
    public class RequestCommodityWarehouseModel
    {
        public RequestResult Request { get; set; }
        public List<RequestItemCommodity> items { get; set; }
    }
    //  StatusId :
    //1	در دست اقدام
    //2	تایید سرپرست
    //3	تایید مدیر واحد
    //4	تایید انبار
    //5	جمع آوری
    //6	آماده ارسال
    //7	ارسال شده
    //8	تحویل شده
    //9	در انتظار داغی 
    //10	داغی دریافت شده
    //11	پایان فرایند تحویل
    //12	ابطال درخواست
    //13	آرشیو
    //14	خروج پیک
    //15	برگشت پیک
    //16	شروع جمع آوری
    //17	پایان جمع آوری
    //18	تغییر وضعیت داغی قطعه


    public class RequestResult
    {
        public int Id { get; set; }
        public string RequesterTitle { get; set; }
        public string RequesterId { get; set; }
        public DateTime RequestDate { get; set; }
        public string RequestDate_Jalali { get; set; }
        public int StatusId { get; set; }
        public string StatusTitle { get; set; }
        public string warehouseCourierTitle { get; set; }//پیک انبار
        public bool SabtShodeTavasoteAnbar { get; set; }
        public bool Force { get; set; }
        public bool NewSearch { get; set; }
        public string RequestNo { get; set; }
        public int DocumentNo { get; set; }
        public int? DocumentId { get; set; }

    }

    public class RequestItemCommodity
    {
        public int RequestItemId { get; set; }
        public int DocumentItemsId { get; set; }
        public int DocumentHeadId { get; set; } = default!;
        public string CommodityCode { get; set; }
        public int? CommodityId { get; set; }
        public string CommodityName { get; set; }
        public double Quantity { get; set; }
        public double QuantityExit { get; set; }

        public double QuantityTotal { get; set; }
        public string MeasureTitle { get; set; }
        public int? MeasureId { get; set; }
        public string Description { get; set; }
        public string LayoutTitle { get; set; }
        public string PlaceUse { get; set; }
        public string PlaceUseDetail { get; set; }
        public bool Daghi { get; set; }
        public string DescriptionSupervisor { get; set; }
        public bool ReturnDaghi { get; set; }
        public double? Inventory { get; set; }
        
        public List<WarehouseLayoutsCommoditiesModel> Layouts { get; set; }
    }
}
