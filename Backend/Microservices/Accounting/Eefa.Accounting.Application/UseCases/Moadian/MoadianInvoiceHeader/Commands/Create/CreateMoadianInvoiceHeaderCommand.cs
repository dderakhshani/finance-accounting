using AutoMapper;
using Eefa.Accounting.Application.UseCases.Moadian.MoadianInvoiceDetail.Commands.Create;
using Eefa.Accounting.Application.UseCases.Moadian.MoadianInvoiceHeader.Model;
using Library.Common;
using Library.Interfaces;
using Library.Mappings;
using Library.Models;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Accounting.Application.UseCases.Moadian.MoadianInvoiceHeader.Commands.Create
{
    public class CreateMoadianInvoiceHeaderCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<CreateMoadianInvoiceHeaderCommand>, ICommand
    {
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

        public List<CreateMoadianInvoiceDetailCommand> MoadianInvoiceDetails { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateMoadianInvoiceHeaderCommand, Data.Entities.MoadianInvoiceHeader>()
                .IgnoreAllNonExisting();
        }
    }


    public class CreateMoadianInvoiceHeaderCommandHandler : IRequestHandler<CreateMoadianInvoiceHeaderCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;
        public CreateMoadianInvoiceHeaderCommandHandler(IMapper mapper, IRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }
        public async Task<ServiceResult> Handle(CreateMoadianInvoiceHeaderCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Data.Entities.MoadianInvoiceHeader>(request);

            _repository.Insert(entity);

            await _repository.SaveChangesAsync(cancellationToken);

            return ServiceResult.Success(_mapper.Map<MoadianInvoiceHeaderDetailedModel>(entity));
        }
    }
}
