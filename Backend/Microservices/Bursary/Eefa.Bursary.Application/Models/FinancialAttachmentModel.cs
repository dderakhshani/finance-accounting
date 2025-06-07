using AutoMapper;
using Eefa.Bursary.Domain.Entities;
using Eefa.Common;

namespace Eefa.Bursary.Application
{
    public class FinancialAttachmentModel : IMapFrom<FinancialAttachmentModel>
    {
        public int? Id { get; set; }
        public int? FinancialRequestId { get; set; }
        public int? AttachmentId { get; set; }
        public bool? isVerified { get; set; }
        public string AddressUrl { get; set; }
        public bool IsDeleted { get; set; }
        public int? ChequeSheetId  { get; set; }
    public void Mapping(Profile profile)
        {
            profile.CreateMap<FinancialAttachmentModel, FinancialRequestAttachments>().IgnoreAllNonExisting();
                 
        }
    }

}
