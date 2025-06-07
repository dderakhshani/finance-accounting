using AutoMapper;
using Library.Mappings;
using System;

namespace Eefa.Accounting.Application.UseCases.Moadian.MoadianInvoiceHeader.Model
{
    public class MoadianInvoiceHeaderModel : IMapFrom<Data.Entities.MoadianInvoiceHeader>
    {
        public int Id { get; set; }
        public bool IsSandbox { get; set; }
        public string PersonFullName { get; set; }
        public string AccountReferenceCode { get; set; }
        public string CustomerCode { get; set; }
        public long ListNumber { get; set; }
        public string? Errors { get; set; }
        public string? StatusTitle { get; set; }
        public string? ReferenceId { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public DateTime? SubmissionDate { get; set; }
        public string TaxId { get; set; } = default!;
        public long Indatim { get; set; } = default!;
        public long Indati2m { get; set; } = default!;
        public string IntyTitle { get; set; }
        public string InvoiceNumber { get; set; } = default!;
        public string Inno { get; set; } = default!;
        public string? Irtaxid { get; set; }
        public string InpTitle { get; set; }
        public int Ins { get; set; } = default!;
        public string InsTitle { get; set; } = default!;
        public string Tins { get; set; } = default!;
        public string TobTitle { get; set; }
        public string? Bid { get; set; }
        public string? Tinb { get; set; }
        public string? Sbc { get; set; }
        public string? Bpc { get; set; }
        public string? Bbc { get; set; }
        public int? Ft { get; set; }
        public string? Bpn { get; set; }
        public string? Scln { get; set; }
        public string? Scc { get; set; }
        public string? Crn { get; set; }
        public string? Billid { get; set; }
        public decimal Tprdis { get; set; } = default!;
        public decimal Tdis { get; set; } = default!;
        public decimal Tadis { get; set; } = default!;
        public decimal Tvam { get; set; } = default!;
        public decimal Todam { get; set; } = default!;
        public decimal Tbill { get; set; } = default!;
        public int Setm { get; set; } = default!;
        public decimal Cap { get; set; } = default!;
        public decimal Insp { get; set; } = default!;
        public decimal Tvop { get; set; } = default!;
        public decimal Tax17 { get; set; } = default!;
        public string? Cdcn { get; set; }
        public int? Cdcd { get; set; }
        public decimal Tonw { get; set; } = default!;
        public decimal Torv { get; set; } = default!;
        public decimal Tocv { get; set; } = default!;

        public string Creator { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Data.Entities.MoadianInvoiceHeader, MoadianInvoiceHeaderModel>()
                .ForMember(x => x.PersonFullName, opt=> opt.MapFrom(x => x.Person.FirstName + " " + x.Person.LastName))
                .ForMember(x => x.AccountReferenceCode, opt=> opt.MapFrom(x => x.AccountReference.Code))
                .ForMember(x => x.CustomerCode, opt=> opt.MapFrom(x => x.Customer.CustomerCode))
                .ForMember(x => x.Creator, opt => opt.MapFrom(x => x.CreatedBy.Person.FirstName + " " + x.CreatedBy.Person.LastName))
                .ForMember(x => x.InpTitle, opt => opt.MapFrom(x => x.Inp == 1 ? "داخلی" : x.Inp == 7 ? "صادراتی" : ""))
                .ForMember(x => x.StatusTitle, opt => opt.MapFrom(x => (x.Status == "SENT" ? "ارسال شده" 
                    : (x.Status == "SUCCESS" ? "ثبت شده" : (x.Status == "FAILED" ? "خطا" 
                    : (x.Status == "IN_PROGRESS" ? "در انتظار ثبت" : (x.Status == "INVALID_DATA" ? "اطلاعات نامعتبر" 
                    : (x.Status == "DECLINED" ? "رد شده" : (x.Status == null ? "ارسال نشده"
                    : (x.Status == "NOT_FOUND" ? "در سامانه پیدا نشد" : ""))))))))))
                .ForMember(x => x.TobTitle, opt => opt.MapFrom(x => (x.Tob == 1 ? "حقیقی" 
                    : (x.Tob == 2 ? "حقوقی" : (x.Tob == 3 ? "مشاركت مدنی" : (x.Tob == 4 ? "اتباع غیر ایرانی" 
                    : (x.Tob == 5 ? "مصرف كننده نهایی" : "")))))))
                .ForMember(x => x.IntyTitle, opt => opt.MapFrom(x => (x.Inty == 1 ? "الکترونیکی نوع اول"
                    : (x.Inty == 2 ? "الکترونیکی نوع دوم" : ""))))

                  .ForMember(x => x.InsTitle, opt => opt.MapFrom(x => (x.Ins == 1 ? "اصلی"
                    : (x.Ins == 2 ? "اصلاحی" : (x.Ins == 3 ? "ابطالی": (x.Ins == 4 ? "برگشت از فروش" : ""))))))
                ;
        }
    }
}