using AutoMapper;
using System;
using System.Collections.Generic;

public class MoadianInvoiceHeaderDetailedModel : IMapFrom<MoadianInvoiceHeader>
{
    public int Id { get; set; }
    public string PersonFullName { get; set; }
    public string AccountReferenceCode { get; set; }
    public string CustomerCode { get; set; }
    public long ListNumber { get; set; }
    public string? Errors { get; set; }
    public string? Status { get; set; } = default!;
    public string? ReferenceId { get; set; }
    public DateTime? InvoiceDate { get; set; }
    public DateTime? SubmissionDate { get; set; }
    public string TaxId { get; set; } = default!;
    public long Indatim { get; set; } = default!;
    public long Indati2m { get; set; } = default!;
    public int Inty { get; set; } = default!;
    public string Inno { get; set; } = default!;
    public string? Irtaxid { get; set; }
    public int Inp { get; set; } = default!;
    public int Ins { get; set; } = default!;
    public string Tins { get; set; } = default!;
    public int? Tob { get; set; }
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
    public virtual List<MoadianInvoiceDetailModel> MoadianInvoiceDetails { get; set; } = default!;

    public void Mapping(Profile profile)
    {
        profile.CreateMap<MoadianInvoiceHeader, MoadianInvoiceHeaderDetailedModel>()
           .ForMember(x => x.PersonFullName, opt => opt.MapFrom(x => x.Person.FirstName + " " + x.Person.LastName))
           .ForMember(x => x.AccountReferenceCode, opt => opt.MapFrom(x => x.AccountReference.Code))
           .ForMember(x => x.CustomerCode, opt => opt.MapFrom(x => x.Customer.CustomerCode));
    }
}