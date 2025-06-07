
using System.ComponentModel.DataAnnotations;
using Eefa.Common;

namespace Eefa.Purchase.Domain.Entities.SqlView
{

    [HasUniqueIndex]
    public partial class AccountReferenceEmployeeView
    {
        [Key]
        public int Id { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public string SearchTerm { get; set; }
        public string EmployeeCode { get; set; }
    }
}
