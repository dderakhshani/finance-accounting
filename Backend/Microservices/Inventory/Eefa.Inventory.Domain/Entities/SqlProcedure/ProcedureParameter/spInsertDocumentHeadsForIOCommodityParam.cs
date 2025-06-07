using AutoMapper;
using System;

namespace Eefa.Inventory.Domain
{
    public class spInsertDocumentHeadsForIOCommodityParam
    {
        public int userId { get; set; }
        public int yearId { get; set; }
        public int OwnerRoleId { get; set; }
        public string jsonData { get; set; }
    }
   


}
