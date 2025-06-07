using AutoMapper;
using Library.Mappings;
using System;

namespace Eefa.Admin.Application.CommandQueries.CorrectionRequest.Models
{
    public class CorrectionRequestModel : IMapFrom<Data.Databases.Entities.CorrectionRequest>
    {
        public int Id { get; set; } = default!;
        public string Status { get; set; } = default!;
        public int CodeVoucherGroupId { get; set; } = default!;
        public string CodeVoucherGroupTitle { get; set; } = default!;
        public int? AccessPermissionId { get; set; } = default!;
        public int? DocumentId { get; set; }
        public string OldData { get; set; } = default!;
        public int VerifierUserId { get; set; } = default!;
        public int CreatedById { get; set; }
        public string? PayLoad { get; set; }
        public string ApiUrl { get; set; } = default!;
        public string? ViewUrl { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string VerifierUserName { get; set; }
       
        public string CreatedUserName { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Data.Databases.Entities.CorrectionRequest, CorrectionRequestModel>()
                .ForMember(x => x.CodeVoucherGroupTitle, opt => opt.MapFrom(x => x.CodeVoucherGroup.Title));
        }
    }
}