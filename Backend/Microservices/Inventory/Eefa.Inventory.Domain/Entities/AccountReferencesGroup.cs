using Eefa.Common;
using Eefa.Common.Domain;

namespace Eefa.Inventory.Domain
{

    public partial class AccountReferencesGroup  : DomainBaseEntity
    {

     
        /// <summary>
        /// کد شرکت
        /// </summary>
        public int CompanyId { get; set; } = default!;
        public int? ParentId { get; set; } = default!;

        [UniqueIndex]
        public string Code { get; set; }


        /// <summary>
        /// عنوان
        /// </summary>
        [UniqueIndex]
        public string Title { get; set; } = default!;

        /// <summary>
        /// آیا قابل ویرایش است؟
        /// </summary>
        public bool? IsEditable { get; set; } = default!;

       
       
       
       
    }
}
