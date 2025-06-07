using Eefa.Common.Data;

namespace Eefa.Bursary.Domain.Entities
{
    public partial class CodingTemplateProperties : BaseEntity
    {
         
        public int CodingTemplateId { get; set; } = default!;
        public int OrderIndex { get; set; } = default!;
        public int CategoryPropertyId { get; set; } = default!;

        public virtual CodingTemplates IdNavigation { get; set; } = default!;
    }
}
