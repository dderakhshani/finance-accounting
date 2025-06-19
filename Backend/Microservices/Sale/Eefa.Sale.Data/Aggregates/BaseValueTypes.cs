using System.Collections.Generic;
using Eefa.Common.Data;

namespace Eefa.Sale.Domain.Aggregates
{

    public partial class BaseValueTypes : BaseEntity
    {
        public BaseValueTypes()
        {
            InverseParent = new HashSet<BaseValueTypes>();
        }




        
        public int? ParentId { get; set; }

        
        public string LevelCode { get; set; } = default!;

        
        public string Title { get; set; } = default!;

        
        public string UniqueName { get; set; } = default!;

        
        public string? GroupName { get; set; }

        
        public bool IsReadOnly { get; set; } = default!;

        
        public string? SubSystem { get; set; }

        
      
        public virtual BaseValueTypes Parent { get; set; } = default!;
        public virtual ICollection<BaseValueTypes> InverseParent { get; set; } = default!;
    }
}
