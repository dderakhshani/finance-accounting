using AutoMapper;
using Eefa.Common;
using Eefa.Inventory.Domain;
using System;
using System.Collections.Generic;

namespace Eefa.Inventory.Application
{
  
    public class InsertDocumentHeadsForIOCommodity 
    {
        public int userId { get; set; }
        public int yearId { get; set; }
        public int OwnerRoleId { get; set; }
        public InsertDocumentHeads receiptdocument { get; set; }

       
    }

    public class InsertDocumentHeads : IMapFrom<InsertLeavingWarehouseMaterialCommand>
    {
        public int? DocumentHeadId { get; set; }
        public DateTime DocumentDate { get; set; }

        public int RequestId { get; set; }
        public int WarehouseId { get; set; }
        public int CodeVoucherGroupId { get; set; }
        public string DocumentDescription { get; set; }
        public string Tags { get; set; }
        public int? ViewId { get; set; }
        public int? DebitAccountHeadId { get; set; }
        public int? CreditAccountHeadId { get; set; }
        public int? DebitAccountReferenceId { get; set; }
        public int? DebitAccountReferenceGroupId { get; set; }
        public int? CreditAccountReferenceId { get; set; }
        public int? CreditAccountReferenceGroupId { get; set; }
        public bool IsDocumentIssuance { get; set; }
        public string InvoiceNo { get; set; }
        public string RequestNo { get; set; }
        public int CurrencyBaseId { get; set; }
        public int DefaultLayoutId { get; set; }
        public int DocumentStauseBaseValue { get; set; }
        public int Mode { get; set; }
        public int? ParentId { get; set; }
        public string CommandDescription { get; set; }
        public string CodeVoucherGroupUniqueName { get; set; }
        public List<InsertDocumentItems> receiptDocumentItems { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<InsertLeavingWarehouseMaterialCommand, InsertDocumentHeads>()
                .IgnoreAllNonExisting();
            profile.CreateMap<UpdateLeavingWarehouseMaterialCommand, InsertDocumentHeads>()
                .IgnoreAllNonExisting();
            profile.CreateMap<LeaveCommodityCommand, InsertDocumentHeads>()
               .IgnoreAllNonExisting();
        }

    }

    public class InsertDocumentItems 
    {
        
        public int? DocumentItemId { get; set; }
        public int CommodityId { get; set; }
        public int MainMeasureId { get; set; }
        public int BomValueHeaderId { get; set; }
        public float Quantity { get; set; }
        public int DocumentMeasureId { get; set; }
        public string Description { get; set; }
        public bool IsWrongMeasure { get; set; }
        public int WarehouseLayoutId { get; set; }
        public List<int> AssetsId { get; set; }

       
    }

}
