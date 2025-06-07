using Eefa.Common.Domain;

namespace Eefa.Inventory.Domain
{
   
   
    public partial class Units : DomainBaseEntity
    {
        public string LevelCode { get; set; } = default!;
        public int? ParentId { get; set; } = default!;
        public string Code { get; set; } = default!;
        public string Title { get; set; } = default!;
        public int BranchId { get; set; } = default!;


        
    }
}
