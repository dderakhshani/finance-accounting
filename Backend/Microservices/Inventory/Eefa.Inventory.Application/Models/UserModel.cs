using System;

namespace Eefa.Inventory.Application
{
    public class UserModel 
    {
        public int Id { get; set; }
        public string Title { get; set; } = default!;
      
    }
    public  class CorrectionRequestModel
    {
        public int Id { get; set; }
        public int Status { get; set; }
        public string StatusTitle { get; set; }
        public int CodeVoucherGroupId { get; set; }
        public Nullable<int> DocumentId { get; set; }
        public string OldData { get; set; }
        public int VerifierUserId { get; set; }
        public string PayLoad { get; set; }
        public string ApiUrl { get; set; }
        public string ViewUrl { get; set; }
        public string Description { get; set; }
        public string VerifierDescription { get; set; }
        public string RequesterDescription { get; set; }

        public DateTime CreatedAt { get; set; }
        public int? ModifiedById { get; set; }
        public DateTime ModifiedAt { get; set; }
        public int CreatedById { get; set; }
        public string Username { get; set; }
        

    }
}
