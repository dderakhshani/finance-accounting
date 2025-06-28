using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eefa.Warehouse.Application.Models
{
    public class WarehouseModel
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public int AccountHeadId { get; set; }
        public string Title { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}
