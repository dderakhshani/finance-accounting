using System;
using AutoMapper;
using Library.Mappings;

namespace Eefa.Accounting.Application.UseCases.Report.Model
{
    public class AccountReviewModel : IMapFrom<Data.Entities.VouchersDetail>
    {
        public int Id { get; set; }

        /// <summary>
        /// کد سند
        /// </summary>
        public int VoucherId { get; set; } = default!;

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
        /// کد گروه سند
        /// </summary>
        public int VoucherGroupId { get; set; } = default!;

        /// <summary>
        /// کد وضعیت سند
        /// </summary>
        public int VoucherStateId { get; set; } = default!;


        /// <summary>
        /// بدهکار
        /// </summary>
        public long Debit { get; set; } = default!;

        /// <summary>
        /// اعتبار
        /// </summary>
        public long Credit { get; set; } = default!;

        /// <summary>
        /// شماره سند مرتبط 
        /// </summary>
        public int? DocumentId { get; set; }

        /// <summary>
        /// شماره مرجع
        /// </summary>
        public int? ReferenceId { get; set; }

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

        
        public long? Remain { get; set; }


        public string VoucherGroupTitle { get; set; }

        public int AccountHeadId { get; set; }
        public int? CurrencyValue { get; set; }
        public int? ExchengeValue { get; set; }
        public int? TraceNumber { get; set; }
        public int? QuantityValue { get; set; }


        public Data.Entities.AccountHead AccountHead { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Data.Entities.VouchersDetail, AccountReviewModel>()
                .ForMember(x=>x.VoucherStateId,opt=>opt.MapFrom(x=>x.Voucher.VoucherStateId))
                .ForMember(src => src.AccountHead, opt => opt.MapFrom(dest => dest.AccountHead))
                ;
        }
    }
}
