using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using Eefa.Inventory.Domain;
using MediatR;

namespace Eefa.Inventory.Application
{ 
    public class DeleteUnitCommodityQuotaCommand : CommandBase, IRequest<ServiceResult<UnitCommodityQuotaModel>>, IMapFrom<Domain.UnitCommodityQuota>, ICommand
    {
        public int Id { get; set; }
    }

   
    public class DeleteUnitCommodityQuotaCommandHandler : IRequestHandler<DeleteUnitCommodityQuotaCommand, ServiceResult<UnitCommodityQuotaModel>>
    {
        private readonly IUnitCommodityQuotaRepository _UnitCommodityQuotaRepository;
        private readonly IMapper _mapper;

        public DeleteUnitCommodityQuotaCommandHandler(IUnitCommodityQuotaRepository UnitCommodityQuotaRepository, IMapper mapper)
        {
            _mapper = mapper;
            _UnitCommodityQuotaRepository = UnitCommodityQuotaRepository;
        }

        public async Task<ServiceResult<UnitCommodityQuotaModel>> Handle(DeleteUnitCommodityQuotaCommand request, CancellationToken cancellationToken)
        {
            var entity = await _UnitCommodityQuotaRepository.Find(request.Id);

            

            _UnitCommodityQuotaRepository.Delete(entity);
            if(await _UnitCommodityQuotaRepository.SaveChangesAsync(cancellationToken) > 0)
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
