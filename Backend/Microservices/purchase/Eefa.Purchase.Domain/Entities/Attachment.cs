using Eefa.Common.Domain;

namespace Eefa.Purchase.Domain
{

    public partial class Attachment : DomainBaseEntity
    {

        public int LanguageId { get; set; } = default!;
        public int TypeBaseId { get; set; } = default!;
        public string Extention { get; set; } = default!;
        public bool IsUsed { get; set; }

        /// <summary>
        /// عنوان
        /// </summary>
        public string Title { get; set; } = default!;

        /// <summary>
        /// توضیحات
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// کلمات کلیدی
        /// </summary>
        public string KeyWords { get; set; }

        /// <summary>
        /// لینک
        /// </summary>
        public string Url { get; set; } = default!;

    }
}
