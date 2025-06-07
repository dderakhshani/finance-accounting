using System.ComponentModel.DataAnnotations;
using Eefa.Common;

namespace Eefa.Purchase.Domain.Entities.SqlView
{
   
    [HasUniqueIndex]
    public partial class AccountReferenceView 
    {
        [Key]
        public int Id { get; set; } = default!;
        public string Code { get; set; } = default!;

        /// <summary>
        /// عنوان
        /// </summary>
        public string Title { get; set; } = default!;
        public string AccountReferencesGroupsCode { get; set; } = default!;
        public int? AccountReferenceGroupId { get; set; } = default!;
        public int? AccountReferenceGoupCodeId { get; set; } = default!;


    }
}
