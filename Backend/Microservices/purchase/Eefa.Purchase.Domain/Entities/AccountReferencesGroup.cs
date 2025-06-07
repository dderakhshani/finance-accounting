using Eefa.Common;
using Eefa.Common.Domain;

namespace Eefa.Purchase.Domain.Entities
{

    public partial class AccountReferencesGroup  : DomainBaseEntity
    {

     
        /// <summary>
        /// کد شرکت
        /// </summary>
        public int CompanyId { get; set; } = default!;

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
