using AutoMapper;
using Eefa.Common;
using Eefa.Inventory.Domain;
using Eefa.Inventory.Domain.Aggregates.WarehouseAggregate;
using Eefa.Inventory.Domain.Enum;
using System;
using System.Collections.Generic;

namespace Eefa.Inventory.Application
{

    public record WarehouseCountFormHeadModel : IMapFrom<WarehouseCountFormHead>
    {
        public int Id { get; set; }
        public int FormNo { get; set; }
        public int? ParentId { get; set; }
        public DateTime FormDate { get; set; }
        public int WarehouseId { get; set; }
        public int WarehouseLayoutId { get; set; }
        public string WarehouseLayoutTitle { get; set; }
        public int CounterUserId { get; set; }
        public int ConfirmerUserId { get; set; }
        public WarehouseStateForm FormState { get; set; }
        public string FormStateMessage { get; set; }
        public string Description { get; set; }
        public string ConfirmerUserName { get; set; }
        public string CounterUserName { get; set; }
        public List<WarehouseCountFormDetail> warehouseCountFormDetails { get; set; } = default!;
        public void Mapping(Profile profile)
        {
            profile.CreateMap<WarehouseCountFormHead, WarehouseCountFormHeadModel>();
        }
    }
}
