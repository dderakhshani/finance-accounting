 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Common;

namespace Eefa.Bursary.Application.Queries.DocumentHead
{
    public record DocumentHeadModel : IMapFrom<Domain.Entities.DocumentHead>
    {
        public int Id { get; set; }
        public int? AccountReferencesGroupId { get; set; }

   //     public string AccountReferencesGroupTitle { get; set; }
        public int CodeVoucherGroupId { get; set; } = default!;
    //    public string CodeVoucherGroupTitle { get; set; }
        /// <summary>
        /// کد سال
        /// </summary>
        public int YearId { get; set; } = default!;
     //   public string YearTitle { get; set; }

        public string DocumentHeadFullDescriptions { get; set; }

        /// <summary>
        /// کد انبار
        /// </summary>
        public int WarehouseId { get; set; } = default!;
        public string WarehouseTitle { get; set; }

        /// <summary>
        /// کد والد
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        /// کد مرجع
        /// </summary>
        public int? ReferenceId { get; set; }
        public string ReferenceTitle { get; set; }
        /// <summary>
        /// شماره سند
        /// </summary>
        public int? DocumentNo { get; set; }

        /// <summary>
        /// تاریخ سند
        /// </summary>
        public DateTime DocumentDate { get; set; } = default!;

        /// <summary>
        /// توضیحات سند
        /// </summary>
        public string? DocumentDescription { get; set; }

        /// <summary>
        /// کد وضعیت سند
        /// </summary>
        public int DocumentStateBaseId { get; set; } = default!;

        /// <summary>
        /// دستی
        /// </summary>
        public bool IsManual { get; set; } = default!;

        /// <summary>
        /// کد سند حسابداری
        /// </summary>
        public int? VoucherHeadId { get; set; }
        public double TotalWeight { get; set; } = default!;
        public double TotalQuantity { get; set; } = default!;

        /// <summary>
        /// جمع مبلغ قابل پرداخت
        /// </summary>
        public long TotalItemPrice { get; set; } = default!;

        /// <summary>
        /// عوارض ارزش افزوده
        /// </summary>
        public long VatTax { get; set; } = default!;

        /// <summary>
        /// مالیات ارزش افزوده
        /// </summary>
        public long VatDutiesTax { get; set; } = default!;

        /// <summary>
        /// عوارض سلامت
        /// </summary>
        public long HealthTax { get; set; } = default!;

        /// <summary>
        /// جمع تخفیف
        /// </summary>
        public long TotalItemsDiscount { get; set; } = default!;

        /// <summary>
        /// جمع قیمت تمام شده
        /// </summary>
        public long? TotalProductionCost { get; set; }

        /// <summary>
        /// درصد تخفیف کل فاکتور
        /// </summary>
        public double? DiscountPercent { get; set; }

        /// <summary>
        /// تخفیف کل سند
        /// </summary>
        public double? DocumentDiscount { get; set; }

        /// <summary>
        /// قیمت بعد از کسر تخفیف
        /// </summary>
       public double? PriceMinusDiscount { get; set; }

        /// <summary>
        /// قیمت با مالیات بعد از کسر تخفیف 
        /// </summary>
        public double? PriceMinusDiscountPlusTax { get; set; }

        /// <summary>
        /// نوع پرداخت
        /// </summary>
        public int PaymentTypeBaseId { get; set; } = default!;

        /// <summary>
        /// تاریخ انقضا
        /// </summary>
        public DateTime? ExpireDate { get; set; }

        /// <summary>
        /// شماره بخش
        /// </summary>
        public string? PartNumber { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.DocumentHead, DocumentHeadModel>()
                .ForMember(d => d.ReferenceTitle, opt => opt.MapFrom(src => src.Reference.Title));
        }

    }
}
