using Library.Common;

namespace Eefa.WorkflowAdmin.WebApi.Domain.Databases.Entities
{
    public partial class CorrectionRequest:BaseEntity
    {
        public short Status { get; set; } 
        public short Type { get; set; }
        public int? DocumentId { get; set; }
        public string Changes { get; set; } = default!;
        public int VerifierUserId { get; set; } = default!;
        public int MenuId { get; set; } = default!;
        public int? CorrectedId { get; set; }
        public string PayLoad { get; set; }
        public string Description { get; set; }
        public string CallBack { get; set; }
        public virtual User CreatedBy { get; set; } = default!;
        public virtual DocumentHead? Document { get; set; } = default!;
        public virtual MenuItem Menu { get; set; } = default!;
        public virtual User ModifiedBy { get; set; } = default!;
        public virtual Role OwnerRole { get; set; } = default!;
        public virtual User VerifierUser { get; set; } = default!;
    }
}
