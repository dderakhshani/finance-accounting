using AutoMapper;
using Eefa.Accounting.Application.UseCases.VouchersHead.Model;
using Library.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eefa.Accounting.Application.UseCases.Report.Model
{
    public class VoucherDetailReportResultModel : IMapFrom<Data.Entities.VouchersDetail>
    {
        public string Title { get; set; }
        public string Code { get; set; }
        public string Color { get; set; }
        public int Level { get; set; }
        public int Id { get; set; }
        public int RowIndex { get; set; }
        public int VoucherId { get; set; }
        public int? VoucherNumber { get; set; }
        public DateTime? Date { get; set; }
        public string Description { get; set; }
        public double Credit { get; set; }
        public double Debit { get; set; }
        public double Remain { get; set; }

        public double CurrencyCredit { get; set; }
        public double CurrencyDebit { get; set; }
        public double CurrencyRemain { get; set; }
        public double CurrencyFee { get; set; }
        public double CurrencyTypeBaseId { get; set; }
        public int AccountHeadId { get; set; }
        public string AccountHead { get; set; }
        public int Level1 { get; set; }
        public int Level2 { get; set; }
        public int Level3 { get; set; }
        public int AccountReferenceGroupId { get; set; }
        public string AccountReferenceGroup { get; set; }
        public int AccountReferenceId { get; set; }
        public string AccountReference { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Data.Entities.VouchersDetail, VoucherDetailReportResultModel>()
                .ForMember(src => src.Id, opt => opt.MapFrom(dest => dest.Id))
                .ForMember(src => src.RowIndex, opt => opt.MapFrom(dest => dest.RowIndex))
                .ForMember(src => src.VoucherId, opt => opt.MapFrom(dest => dest.VoucherId))
                .ForMember(src => src.VoucherNumber, opt => opt.MapFrom(dest => dest.Voucher.VoucherNo))
                .ForMember(src => src.Date, opt => opt.MapFrom(dest => dest.Voucher.VoucherDate))
                .ForMember(src => src.Description, opt => opt.MapFrom(dest => dest.VoucherRowDescription))
                .ForMember(src => src.Credit, opt => opt.MapFrom(dest => dest.Credit))
                .ForMember(src => src.Debit, opt => opt.MapFrom(dest => dest.Debit))
                .ForMember(src => src.CurrencyCredit, opt => opt.MapFrom(dest => dest.Credit > 0 ?  dest.CurrencyAmount : 0))
                .ForMember(src => src.CurrencyDebit, opt => opt.MapFrom(dest => dest.Debit > 0 ? dest.CurrencyAmount : 0))
                .ForMember(src => src.CurrencyTypeBaseId, opt => opt.MapFrom(dest => dest.CurrencyTypeBaseId))
                .ForMember(src => src.CurrencyFee, opt => opt.MapFrom(dest => dest.CurrencyFee))
                .ForMember(src => src.AccountHeadId, opt => opt.MapFrom(dest => dest.AccountHeadId))
                .ForMember(src => src.AccountHead, opt => opt.MapFrom(dest => dest.AccountHead.Code + " - "+ dest.AccountHead.Title))
                .ForMember(src => src.Level1, opt => opt.MapFrom(dest => dest.Level1))
                .ForMember(src => src.Level2, opt => opt.MapFrom(dest => dest.Level2))
                .ForMember(src => src.Level3, opt => opt.MapFrom(dest => dest.Level3))
                .ForMember(src => src.AccountReferenceId, opt => opt.MapFrom(dest => dest.ReferenceId1))
                .ForMember(src => src.AccountReference, opt => opt.MapFrom(dest => dest.ReferenceId1Navigation.Code + " - " + dest.ReferenceId1Navigation.Title))
                .ForMember(src => src.AccountReferenceGroupId, opt => opt.MapFrom(dest => dest.AccountReferencesGroupId))
                .ForMember(src => src.AccountReferenceGroup, opt => opt.MapFrom(dest => dest.AccountReferencesGroup.Code + " - " + dest.AccountReferencesGroup.Title))
                ;
        }
    }
}
