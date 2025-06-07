using Eefa.Common;
using Eefa.Purchase.Domain.Entities;

namespace Eefa.Purchase.Application.Models
{
    public class BaseValueModel : IMapFrom<BaseValue>
    {
        public int Id { get; set; }
        public string Code { get; set; } = default!;
        
        public string Title { get; set; } = default!;
        /// <description>
        /// نام اختصاصی
        ///</description>

        public string UniqueName { get; set; } = default!;
        /// <description>
        /// مقدار
        ///</description>

        public string Value { get; set; } = default!;
       
    }
}
