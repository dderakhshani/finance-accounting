using AutoMapper;
using Library.Common;
using Library.Interfaces;
using Library.Mappings;
using Library.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Accounting.Application.UseCases.Moadian.MoadianInvoiceDetail.Commands.Create
{
    public class UpdateMoadianInvoiceDetailCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<UpdateMoadianInvoiceDetailCommand>, ICommand
    {
        public int Id { get; set; }
        public int InvoiceHeaderId { get; set; } = default!;
        public string Sstid { get; set; } = default!;
        public string Sstt { get; set; } = default!;
        public string Mu { get; set; } = default!;
        public decimal Am { get; set; } = default!;
        public decimal Fee { get; set; } = default!;
        public decimal Cfee { get; set; } = default!;
        public string? Cut { get; set; }
        public decimal Exr { get; set; } = default!;
        public decimal Prdis { get; set; } = default!;
        public decimal Dis { get; set; } = default!;
        public decimal Adis { get; set; } = default!;
        public decimal Vra { get; set; } = default!;
        public decimal Vam { get; set; } = default!;
        public string? Odt { get; set; }
        public decimal Odr { get; set; } = default!;
        public decimal Odam { get; set; } = default!;
        public string? Olt { get; set; }
        public decimal Olr { get; set; } = default!;
        public decimal Olam { get; set; } = default!;
        public decimal Consfee { get; set; } = default!;
        public decimal Spro { get; set; } = default!;
        public decimal Bros { get; set; } = default!;
        public decimal Tcpbs { get; set; } = default!;
        public decimal Cop { get; set; } = default!;
        public decimal Vop { get; set; } = default!;
        public string? Bsrn { get; set; }
        public decimal Tsstam { get; set; } = default!;
        public decimal Nw { get; set; } = default!;
        public decimal Sscv { get; set; } = default!;
        public decimal Ssrv { get; set; } = default!;
        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateMoadianInvoiceDetailCommand, Data.Entities.MoadianInvoiceDetail>()
                .IgnoreAllNonExisting();
        }
    }


    public class UpdateMoadianInvoiceDetailCommandHandler : IRequestHandler<UpdateMoadianInvoiceDetailCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;
        public UpdateMoadianInvoiceDetailCommandHandler(IRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<ServiceResult> Handle(UpdateMoadianInvoiceDetailCommand request, CancellationToken cancellationToken)
        {

            return ServiceResult.Success(null);
        }
    }
}
