using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Eefa.Accounting.Application.UseCases.VoucherAttachment.Model;
using Library.Mappings;

namespace Eefa.Accounting.Application.UseCases.VouchersHead.Model
{
    public class VouchersHeadModel : IMapFrom<Data.Entities.VouchersHead>
    {
        public int Id { get; set; }
        public int CompanyId { get; set; } = default!;
        public int VoucherDailyId { get; set; } = default!;
        public int YearId { get; set; } = default!;
        public int VoucherNo { get; set; } = default!;
        public int? TraceNumber { get; set; } = default!;
        public DateTime? VoucherDate { get; set; }
        public string VoucherDescription { get; set; } = default!;
        public int CodeVoucherGroupId { get; set; } = default!;
        public int VoucherStateId { get; set; } = default!;
        public string? VoucherStateName { get; set; }
        public int? AutoVoucherEnterGroup { get; set; }
        public double? TotalDebit { get; set; }
        public double? TotalCredit { get; set; }
        public double? Difference { get; set; }
        public string Creator { get; set; }
        public string Modifier { get; set; }
        public int CreatedById { get; set; }
        public int ModifiedById { get; set; }
        public string CodeVoucherGroupTitle { get; set; }
        public bool HasCorrectionRequest { get; set; }
        public ICollection<VoucherAttachmentModel> VoucherAttachmentsList { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Data.Entities.VouchersHead, VouchersHeadModel>()
                .ForMember(src => src.Creator, opt => opt.MapFrom(dest => (dest.CreatedBy.Person.FirstName ?? "") + " " + (dest.CreatedBy.Person.LastName ?? "")))
                .ForMember(src => src.Modifier, opt => opt.MapFrom(dest => (dest.ModifiedBy.Person.FirstName ?? "") + " " + (dest.ModifiedBy.Person.LastName ?? "")))
                .ForMember(src => src.CodeVoucherGroupTitle, opt => opt.MapFrom(dest => dest.CodeVoucherGroup.Title))
                .ForMember(src => src.VoucherAttachmentsList, opt => opt.MapFrom(dest => dest.VoucherAttachments))
                .ForMember(src => src.CreatedById, opt => opt.MapFrom(x => x.CreatedById))
                .ForMember(src => src.ModifiedById, opt => opt.MapFrom(x => x.ModifiedById))
                .ForMember(src => src.HasCorrectionRequest, opt => opt.MapFrom(dest => dest.CodeVoucherGroup.CorrectionRequests.Any(x => x.DocumentId == dest.Id && x.Status == 0)))
                ;
        }
    }
}
