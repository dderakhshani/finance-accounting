using Eefa.Common.Data;

namespace Eefa.Bursary.Domain.Entities
{
    public partial class DataDictionary : BaseEntity
    {

        /// <summary>
//شناسه
        /// </summary>
         

        /// <summary>
//نام اصلی
        /// </summary>
        public string? ResourceName { get; set; }

        /// <summary>
//مقدار اصلی
        /// </summary>
        public string? ResourceValue { get; set; }

        /// <summary>
//کد زبان
        /// </summary>
        public int? LaguageId { get; set; }
    }
}
