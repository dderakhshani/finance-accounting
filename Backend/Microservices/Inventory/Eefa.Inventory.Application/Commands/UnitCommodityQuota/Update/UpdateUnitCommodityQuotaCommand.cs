using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using Eefa.Inventory.Domain;
using MediatR;

namespace Eefa.Inventory.Application
{
    public class UpdateUnitCommodityQuotaCommand : CommandBase, IRequest<ServiceResult<UnitCommodityQuotaModel>>, IMapFrom<Domain.UnitCommodityQuota>, ICommand
    {
        public int Id { get; set; } = default!;
        public int CommodityId { get; set; } = default!;
        public int QuotaGroupsId { get; set; } = default!;
        public int CommodityQuota { get; set; } = default!;
        public int QuotaDays { get; set; } = default!;

       


        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateUnitCommodityQuotaCommand, Domain.UnitCommodityQuota>()
                .IgnoreAllNonExisting();
        }
    }


    public class UpdateUnitCommodityQuotaCommandHandler : IRequestHandler<UpdateUnitCommodityQuotaCommand, ServiceResult<UnitCommodityQuotaModel>>
    {
        private readonly IUnitCommodityQuotaRepository _UnitCommodityQuotaRepository;
        private readonly IMapper _mapper;
        

        public UpdateUnitCommodityQuotaCommandHandler(IUnitCommodityQuotaRepository UnitCommodityQuotaRepository, IMapper mapper)
        {
            _mapper = mapper;
            _UnitCommodityQuotaRepository = UnitCommodityQuotaRepository;
            
        }

        public async Task<ServiceResult<UnitCommodityQuotaModel>> Handle(UpdateUnitCommodityQuotaCommand request, CancellationToken cancellationToken)
        {
            

            var entity = await _UnitCommodityQuotaRepository.Find(request.Id);


            _mapper.Map<UpdateUnitCommodityQuotaCommand, Domain.UnitCommodityQuota>(request, entity);


            _UnitCommodityQuotaRepository.Update(entity);

            await _UnitCommodityQuotaRepository.SaveChangesAsync(cancellationToken);

            var model = _mapper.Map<UnitCommodityQuotaModel>(entity);
            return ServiceResult<UnitCommodityQuotaModel>.Success(model);
        }
    }
}
