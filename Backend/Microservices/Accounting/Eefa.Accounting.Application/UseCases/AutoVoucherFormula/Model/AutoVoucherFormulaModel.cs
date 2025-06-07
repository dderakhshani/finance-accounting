using AutoMapper;
using Library.Mappings;

namespace Eefa.Accounting.Application.UseCases.AutoVoucherFormula.Model
{
    public class AutoVoucherFormulaModel : IMapFrom<Data.Entities.AutoVoucherFormula>
    {
        public int Id { get; set; }

        /// <summary>
        /// کد نوع سند مقصد
        /// </summary>
        public int VoucherTypeId { get; set; } = default!;
        public string VoucherTypeTitle { get; set; } = default!;
        /// <summary>
        /// کد نوع سند مبدا
        /// </summary>
        public int SourceVoucherTypeId { get; set; } = default!;
        public string SourceVoucherTypeTitle { get; set; } = default!;

        /// <summary>
        /// ترتیب آرتیکل سند حسابداری
        /// </summary>
        public int OrderIndex { get; set; } = default!;

        /// <summary>
        /// وضعیت مانده حساب
        /// </summary>
        public byte DebitCreditStatus { get; set; } = default!;

        /// <summary>
        /// کد سطح
        /// </summary>
        public int AccountHeadId { get; set; } = default!;

        /// <summary>
        /// توضیحات سطر
        /// </summary>
        public string? RowDescription { get; set; }

        public string? Formula { get; set; }

        /// <summary>
        /// شرط
        /// </summary>
        public string? Conditions { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Data.Entities.AutoVoucherFormula, AutoVoucherFormulaModel>()
             .ForMember(x => x.VoucherTypeTitle, opt => opt.MapFrom(x => x.VoucherType.Title));
            // .ForMember(x => x.SourceVoucherTypeTitle, opt => opt.MapFrom(x => x.SourceVoucherType.Title)).IgnoreAllNonExisting();

        }
    }

}
