using AutoMapper;
using Library.Mappings;

namespace Eefa.Admin.Application.CommandQueries.BaseValueType.Model
{
    public class BaseValueTypeModel : IMapFrom<Data.Databases.Entities.BaseValueType>
    {
        public int Id { get; set; }

        public int? ParentId { get; set; }
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
        /// نام گروه
        /// </summary>
        public string? GroupName { get; set; }

        /// <summary>
        /// آیا فقط قابل خواندن است؟
        /// </summary>
        public bool IsReadOnly { get; set; } = default!;

        /// <summary>
        /// زیر سیستم
        /// </summary>
        public string? SubSystem { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Data.Databases.Entities.BaseValueType, BaseValueTypeModel>();
        }
    }

}
