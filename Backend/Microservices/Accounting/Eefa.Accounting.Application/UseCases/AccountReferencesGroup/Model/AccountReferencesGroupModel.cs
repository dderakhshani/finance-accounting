using AutoMapper;
using Library.Attributes;
using Library.Mappings;
using System;

namespace Eefa.Accounting.Application.UseCases.AccountReferencesGroup.Model
{
    public class AccountReferencesGroupModel : IMapFrom<Eefa.Accounting.Data.Entities.AccountReferencesGroup>
    {
        public int Id { get; set; }
        /// <summary>
        /// کد شرکت
        /// </summary>
        public int CompanyId { get; set; } = default!;

        /// <summary>
        /// کد والد
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        /// کد سطح
        /// </summary>
        [UniqueIndex]
        public string LevelCode { get; set; }

        /// <summary>
        /// عنوان
        /// </summary>
        [UniqueIndex]
        public string Title { get; set; } = default!;

        /// <summary>
        /// آیا قابل ویرایش است؟
        /// </summary>
        public bool? IsEditable { get; set; } = default!;
        public bool? IsVisible { get; set; } = default!;
        public string Code { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Eefa.Accounting.Data.Entities.AccountReferencesGroup, AccountReferencesGroupModel>().IgnoreAllNonExisting();
        }
    }
}
