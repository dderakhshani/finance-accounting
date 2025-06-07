using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Eefa.Accounting.Application.UseCases.VoucherAttachment.Model;
using Eefa.Accounting.Application.UseCases.VouchersDetail.Model;
using Library.Mappings;

namespace Eefa.Accounting.Application.UseCases.VouchersHead.Model
{
    public class VouchersHeadWithDetailModel : IMapFrom<Data.Entities.VouchersHead>
    {
        public int Id { get; set; }
        /// <summary>
        /// کد شرکت
        /// </summary>
        public int CompanyId { get; set; } = default!;

        /// <summary>
        /// کد سند
        /// </summary>
        public int VoucherDailyId { get; set; } = default!;

        /// <summary>
        /// کد سال
        /// </summary>
        public int YearId { get; set; } = default!;

        /// <summary>
        /// شماره سند
        /// </summary>
        public int VoucherNo { get; set; } = default!;

        /// <summary>
        /// تاریخ سند
        /// </summary>
        public DateTime VoucherDate { get; set; } = default!;

        /// <summary>
        /// شرح سند
        /// </summary>
        public string VoucherDescription { get; set; } = default!;

        /// <summary>
        /// کد گروه سند
        /// </summary>
        public int CodeVoucherGroupId { get; set; } = default!;

        /// <summary>
        /// کد وضعیت سند
        /// </summary>
        public int VoucherStateId { get; set; } = default!;

        /// <summary>
        /// نام وضعیت سند
        /// </summary>
        public string? VoucherStateName { get; set; }

        /// <summary>
        /// گروه سند مکانیزه
        /// </summary>
        public int? AutoVoucherEnterGroup { get; set; }

        /// <summary>
        /// جمع بدهی
        /// </summary>
        public double? TotalDebit { get; set; }

        /// <summary>
        /// جمع بستانکاری
        /// </summary>
        public double? TotalCredit { get; set; }

        /// <summary>
        /// اختلاف
        /// </summary>
        public double? Difference { get; set; }

        /// <summary>
        /// نقش صاحب سند
        /// </summary>

        public string Creator { get; set; }
        public string Modifier { get; set; }

        public int CreatedById { get; set; }
        public int ModifiedById { get; set; }

        public string CodeVoucherGroupTitle { get; set; }
        public bool HasCorrectionRequest { get; set; }


        public List<VouchersDetailModel> VouchersDetails { get; set; }
        public ICollection<VoucherAttachmentModel> VoucherAttachmentsList { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Data.Entities.VouchersHead, VouchersHeadWithDetailModel>()
                  .ForMember(src => src.Creator, opt => opt.MapFrom(dest => (dest.CreatedBy.Person.FirstName ?? "") + " " + (dest.CreatedBy.Person.LastName ?? "")))
                 .ForMember(src => src.Modifier, opt => opt.MapFrom(dest => (dest.ModifiedBy.Person.FirstName ?? "") + " " + (dest.ModifiedBy.Person.LastName ?? "")))
                .ForMember(x => x.CodeVoucherGroupTitle, opt => opt.MapFrom(x => x.CodeVoucherGroup.Title))
                .ForMember(src => src.VoucherAttachmentsList, opt => opt.MapFrom(dest => dest.VoucherAttachments))
                .ForMember(x => x.VouchersDetails, opt => opt.MapFrom(x => x.VouchersDetails))
                .ForMember(src => src.CreatedById, opt => opt.MapFrom(x => x.CreatedById))
                .ForMember(src => src.ModifiedById, opt => opt.MapFrom(x => x.ModifiedById))
                .ForMember(src => src.HasCorrectionRequest, opt => opt.MapFrom(dest => dest.CodeVoucherGroup.CorrectionRequests.Any(x => x.DocumentId == dest.Id && x.Status == 0)))

                ;
        }
    }
}
