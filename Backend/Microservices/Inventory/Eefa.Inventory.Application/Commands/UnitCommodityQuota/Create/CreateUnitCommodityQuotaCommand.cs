using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using Eefa.Inventory.Domain;
using MediatR;

namespace Eefa.Inventory.Application
{
    public class CreateUnitCommodityQuotaCommand : CommandBase, IRequest<ServiceResult<UnitCommodityQuotaModel>>, IMapFrom<CreateUnitCommodityQuotaCommand>, ICommand
    {
        
        public int CommodityId { get; set; } = default!;
        public int QuotaGroupsId { get; set; } = default!;
        public int CommodityQuota { get; set; } = default!;
        public int QuotaDays { get; set; } = default!;



        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateUnitCommodityQuotaCommand, Domain.UnitCommodityQuota>()
                .IgnoreAllNonExisting();
        }
    }

    public class CreateUnitCommodityQuotaCommandHandler : IRequestHandler<CreateUnitCommodityQuotaCommand, ServiceResult<UnitCommodityQuotaModel>>
    {
        private readonly IUnitCommodityQuotaRepository _UnitCommodityQuotaRepository;
        private readonly IMapper _mapper;
        
        public CreateUnitCommodityQuotaCommandHandler(
            IUnitCommodityQuotaRepository UnitCommodityQuotaRepository,
            IMapper mapper
            

            )
        {
            _mapper = mapper;
            _UnitCommodityQuotaRepository = UnitCommodityQuotaRepository;
         
        }

        public async Task<ServiceResult<UnitCommodityQuotaModel>> Handle(CreateUnitCommodityQuotaCommand request, CancellationToken cancellationToken)
        {
            Domain.UnitCommodityQuota UnitCommodityQuota;

            UnitCommodityQuota = _mapper.Map<Domain.UnitCommodityQuota>(request);
            var entity = _UnitCommodityQuotaRepository.Insert(UnitCommodityQuota);


            if (await _UnitCommodityQuotaRepository.SaveChangesAsync(cancellationToken) > 0)
            {
                var model = _mapper.Map<UnitCommodityQuotaModel>(entity);
                return ServiceResult<UnitCommodityQuotaModel>.Success(model);
            }

            else
            {
                return ServiceResult<UnitCommodityQuotaModel>.Failed();
            }


        }

    }


}
