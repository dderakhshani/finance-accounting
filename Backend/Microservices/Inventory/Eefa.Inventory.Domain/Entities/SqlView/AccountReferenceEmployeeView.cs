

namespace Eefa.Inventory.Domain
{
    using Eefa.Common;
    using System.ComponentModel.DataAnnotations;

    [HasUniqueIndex]
    public partial class AccountReferenceEmployeeView
    {
        [Key]
        public int Id { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public string SearchTerm { get; set; }
        public string EmployeeCode { get; set; }
        public int? PersonId { get; set; }
        public int? UnitPositionId { get; set; }

    }
}
