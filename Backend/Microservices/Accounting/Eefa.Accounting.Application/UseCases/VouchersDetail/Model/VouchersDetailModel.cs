using System;
using AutoMapper;
using Library.Mappings;

namespace Eefa.Accounting.Application.UseCases.VouchersDetail.Model
{
    public class VouchersDetailModel : Library.Models.PermissionForListModel, IMapFrom<Data.Entities.VouchersDetail>
    {
        public int Id { get; set; }
        /// <summary>
        /// کد سند
        /// </summary>
        public int VoucherId { get; set; } = default!;

        /// <summary>
        /// تاریخ سند
        /// </summary>
        public DateTime? VoucherDate { get; set; }

        /// <summary>
        /// کد حساب سرپرست
        /// </summary>
        public int AccountHeadId { get; set; } = default!;
        public string AccountHeadCode { get; set; }
        public string ReferenceCode1 { get; set; }
        public string ReferenceTitle1 { get; set; }
        public string ReferenceTitle2 { get; set; }
        public string ReferenceTitle3 { get; set; }
        public string ReferenceCode2 { get; set; }
        public string ReferenceCode3 { get; set; }
        /// <summary>
        /// شرح آرتیکل  سند
        /// </summary>
        public string VoucherRowDescription { get; set; } = default!;

        /// <summary>
        /// بدهکار
        /// </summary>
        public double Debit { get; set; } = default!;

        /// <summary>
        /// اعتبار
        /// </summary>
        public double Credit { get; set; } = default!;

        /// <summary>
        /// ترتیب سطر
        /// </summary>
        public int? RowIndex { get; set; }

        /// <summary>
        /// شماره سند مرتبط 
        /// </summary>
        public int? DocumentId { get; set; }
        public string? DocumentNo { get; set; }
        public string? DocumentIds { get; set; }
        public string? FinancialOperationNumber { get; set; }


        /// <summary>
        /// تاریخ مرجع
        /// </summary>
        public DateTime? ReferenceDate { get; set; }

        /// <summary>
        /// مقدار مرجع
        /// </summary>
        public double? Weight { get; set; }

        /// <summary>
        /// کد مرجع1
        /// </summary>
        public int? ReferenceId1 { get; set; }

        /// <summary>
        /// کد مرجع2
        /// </summary>
        public int? ReferenceId2 { get; set; }

        /// <summary>
        /// کد مرجع3
        /// </summary>
        public int? ReferenceId3 { get; set; }

        /// <summary>
        /// سطح 1
        /// </summary>
        public int? Level1 { get; set; }
        public string Level1Name { get; set; }


        /// <summary>
        /// سطح 2
        /// </summary>
        public int? Level2 { get; set; }

        /// <summary>
        /// سطح 3
        /// </summary>
        public int? Level3 { get; set; }

        /// <summary>
        /// وضعیت مانده حساب
        /// </summary>
        public byte? DebitCreditStatus { get; set; }

        /// <summary>
        /// باقیمانده
        /// </summary>
        public double? Remain { get; set; }
        public int? CurrencyTypeBaseId { get; set; }

        public double? CurrencyFee { get; set; }
        public double? CurrencyAmount { get; set; }

        public int? TraceNumber { get; set; }

        public int? AccountReferencesGroupId { get; set; } = default!;

        public bool CurrencyFlag { get; set; } = default!;

        public bool ExchengeFlag { get; set; } = default!;

        public bool TraceFlag { get; set; } = default!;
        public bool QuantityFlag { get; set; } = default!;
        public int VoucherNo { get; set; }
        public int VoucherDailyId { get; set; }
        public double? Quantity { get; set; }

        public string? InvoiceNo { get; set; }

        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Data.Entities.VouchersDetail, VouchersDetailModel>()
                .ForMember(src => src.ReferenceCode1, opt => opt.MapFrom(dest => dest.ReferenceId1Navigation.Code))
                .ForMember(src => src.ReferenceCode2, opt => opt.MapFrom(dest => dest.ReferenceId2Navigation.Code))
                .ForMember(src => src.ReferenceCode3, opt => opt.MapFrom(dest => dest.ReferenceId3Navigation.Code))
                .ForMember(src => src.ReferenceTitle1, opt => opt.MapFrom(dest => dest.ReferenceId1Navigation.Title))
                .ForMember(src => src.ReferenceTitle2, opt => opt.MapFrom(dest => dest.ReferenceId2Navigation.Title))
                .ForMember(src => src.ReferenceTitle3, opt => opt.MapFrom(dest => dest.ReferenceId3Navigation.Title))
                .ForMember(src => src.AccountHeadCode, opt => opt.MapFrom(dest => dest.AccountHead.Code))
                .ForMember(src => src.QuantityFlag, opt => opt.MapFrom(dest => dest.AccountHead.QuantityFlag))
                .ForMember(src => src.TraceFlag, opt => opt.MapFrom(dest => dest.AccountHead.TraceFlag))
                .ForMember(src => src.ExchengeFlag, opt => opt.MapFrom(dest => dest.AccountHead.ExchengeFlag))
                .ForMember(src => src.CurrencyFlag, opt => opt.MapFrom(dest => dest.AccountHead.CurrencyFlag))
                .ForMember(src => src.VoucherNo, opt => opt.MapFrom(dest => dest.Voucher.VoucherNo))
                .ForMember(src => src.VoucherDailyId, opt => opt.MapFrom(dest => dest.Voucher.VoucherDailyId))
                .ForMember(src => src.CreatedBy, opt => opt.MapFrom(x => (x.CreatedBy.Person.FirstName+ " " + x.CreatedBy.Person.LastName)))
                .ForMember(src => src.ModifiedBy, opt => opt.MapFrom(x => (x.ModifiedBy.Person.FirstName+ " " + x.ModifiedBy.Person.LastName)))
                ;
        }
    }
}
