using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class UpdateMoadianInvoiceHeaderCommand : IRequest<ServiceResult<MoadianInvoiceHeaderDetailedModel>>, IMapFrom<UpdateMoadianInvoiceHeaderCommand>
{
    public int Id { get; set; }
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
    public string? Status { get; set; } = default!;

    public List<UpdateMoadianInvoiceDetailCommand> MoadianInvoiceDetails { get; set; } = default!;
    public void Mapping(Profile profile)
    {
        profile.CreateMap<UpdateMoadianInvoiceHeaderCommand, MoadianInvoiceHeader>()
            .IgnoreAllNonExisting();
    }
}


public class UpdateMoadianInvoiceHeaderCommandHandler : IRequestHandler<UpdateMoadianInvoiceHeaderCommand, ServiceResult<MoadianInvoiceHeaderDetailedModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public UpdateMoadianInvoiceHeaderCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<ServiceResult<MoadianInvoiceHeaderDetailedModel>> Handle(UpdateMoadianInvoiceHeaderCommand request, CancellationToken cancellationToken)
    {
        MoadianInvoiceHeader entity = await _unitOfWork.MoadianInvoiceHeaders
                    .GetByIdAsync(request.Id, x => x.Include(y => y.MoadianInvoiceDetails));

        if (entity.IsSandbox || (!entity.IsSandbox && entity.Status != "SUCCESS"))
        {
            _mapper.Map<UpdateMoadianInvoiceHeaderCommand, MoadianInvoiceHeader>(request, entity);
            _unitOfWork.MoadianInvoiceHeaders.Update(entity);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        else
        {
            throw new Exception("صورتحساب های نهایی قابل ویرایش نمیباشند");
        }

        return ServiceResult.Success(_mapper.Map<MoadianInvoiceHeaderDetailedModel>(entity));
    }
}