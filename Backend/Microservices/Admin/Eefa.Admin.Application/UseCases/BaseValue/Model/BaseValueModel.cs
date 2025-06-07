using AutoMapper;
using Library.Mappings;

namespace Eefa.Admin.Application.CommandQueries.BaseValue.Model
{
    public class BaseValueModel : IMapFrom<Data.Databases.Entities.BaseValue>
    {
        public int Id { get; set; }
        /// <summary>
        /// کد نوع مقدار
        /// </summary>
        public int BaseValueTypeId { get; set; } = default!;

        /// <summary>
        /// کد والد
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        /// کد سطح
        /// </summary>
        public string LevelCode { get; set; } = default!;

        /// <summary>
        /// عنوان
        /// </summary>
        public string Title { get; set; } = default!;

        /// <summary>
        /// نام اختصاصی
        /// </summary>
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
        public string Code { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Data.Databases.Entities.BaseValue, BaseValueModel>()
                ;
        }
    }
}
