using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Library.Attributes;
using Library.Interfaces;

namespace FileTransfer.WebApi.Persistance.Entities
{
    [Table(name: "Roles", Schema = "admin")]

    [HasUniqueIndex]
    public partial class Role : HierarchicalBaseEntity
    {
        //public int? ParentId { get; set; }

        ///// <summary>
        ///// کد سطح
        ///// </summary>
        //public string LevelCode { get; set; } = default!;

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
        /// توضیحات
        /// </summary>
        public string? Description { get; set; }


        public virtual Role? Parent { get; set; } = default!;
        public virtual ICollection<Attachment> Attachments { get; set; } = default!;
        public virtual ICollection<BaseValueType> BaseValueTypes { get; set; } = default!;
        public virtual ICollection<Role> InverseParent { get; set; } = default!;
        public virtual ICollection<Language> Languages { get; set; } = default!;
        public virtual ICollection<UserRole> UserRoleOwnerRoles { get; set; } = default!;
        public virtual ICollection<UserRole> UserRoleRoles { get; set; } = default!;
    }
}
