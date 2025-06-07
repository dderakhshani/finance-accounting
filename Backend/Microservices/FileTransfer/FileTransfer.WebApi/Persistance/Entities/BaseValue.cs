using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Library.Attributes;
using Library.Interfaces;

namespace FileTransfer.WebApi.Persistance.Entities
{
    [Table(name: "BaseValues", Schema = "admin")]
    [HasUniqueIndex]
    public partial class BaseValue : HierarchicalBaseEntity
    {

        public int BaseValueTypeId { get; set; } = default!;


        /// <summary>
        /// شناسه
        /// </summary>
        [UniqueIndex]
        public string Code { get; set; } = default!;


        /// <summary>
        /// عنوان
        /// </summary>
        public string Title { get; set; } = default!;

        /// <summary>
        /// نام یکتا
        /// </summary>
        [UniqueIndex]
        public string UniqueName { get; set; } = default!;

        /// <summary>
        /// مقدار
        /// </summary>
        public string Value { get; set; } = default!;

        /// <summary>
        /// ترتیب آرتیکل سند حسابداری
        /// </summary>
        public int OrderIndex { get; set; } = default!;

        /// <summary>
        /// آیا فقط قابل خواندن است؟
        /// </summary>
        public bool IsReadOnly { get; set; } = default!;


        public virtual BaseValueType BaseValueType { get; set; } = default!;
        public virtual User CreatedBy { get; set; } = default!;
        public virtual User? ModifiedBy { get; set; } = default!;
        public virtual ICollection<Attachment> Attachments { get; set; } = default!;
        public virtual ICollection<Language> DefaultCurrencyLanguages { get; set; } = default!;
        public virtual ICollection<Language> DirectionLanguages { get; set; } = default!;
        public virtual ICollection<User> Users { get; set; } = default!;


    }
}
