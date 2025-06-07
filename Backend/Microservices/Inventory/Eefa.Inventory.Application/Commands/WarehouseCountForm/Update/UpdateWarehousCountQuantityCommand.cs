using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using Eefa.Common.Data;
using Eefa.Inventory.Domain;
using MediatR;
using System.Linq;
using Eefa.Inventory.Domain.Aggregates.WarehouseAggregate;
using Eefa.Inventory.Domain.Enum;

namespace Eefa.Inventory.Application
{
    public class UpdateWarehousCountQuantityCommand 
    {
        public int Id { get; set; }       
        public double? CountedQuantity { get; set; }
        public string Description { get; set; }               
    }

}
