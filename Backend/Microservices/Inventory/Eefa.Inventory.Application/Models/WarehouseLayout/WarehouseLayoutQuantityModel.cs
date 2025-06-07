using System;
using AutoMapper;
using Eefa.Common;
using Eefa.Inventory.Domain;

namespace Eefa.Inventory.Application
{
    public class WarehouseLayoutQuantityModel : IMapFrom<WarehouseLayoutQuantity>
    {
        public int Id { get; set; } = default!;
        public int WarehouseLayoutId { get; set; } = default!;
        public string WarehouseLayoutTitle { get; set; }
        /// <description>
        /// کد کالا
        ///</description>

        public int CommodityId { get; set; } = default!;
        /// <description>
        /// تعداد
        ///</description>
        public double? Quantity { get; set; } = default!;
       

        public double? QuantityAvailable { get { return CalcuteQuantityAvailable(QuantityTotal, Quantity); } } 
        public double? QuantityTotal { get; set; } = default!;
        public double? QuantityNeed { get; set; } = default!;
       
        
        /// <description>
        /// نقش صاحب سند
        ///</description>

        public bool IsDeleted { get; set; }
        public virtual WarehouseLayoutModel WarehouseLayout { get; set; } = default!;
        public void Mapping(Profile profile)
        {

            profile.CreateMap<WarehouseLayoutQuantity, WarehouseLayoutQuantityModel>();
                   

        }
        private double CalcuteQuantityAvailable(double? Capacity, double? Quantity)
        {
            double result = Convert.ToDouble(Capacity) - Convert.ToDouble(Quantity);
            return result;
        }
    }
    
}
