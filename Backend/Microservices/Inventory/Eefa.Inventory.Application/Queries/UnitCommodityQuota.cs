using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;
using Eefa.Inventory.Domain;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Inventory.Application
{
    public class UnitCommodityQuotaQueries : IUnitCommodityQuotaQueries
    {
        private readonly IMapper _mapper;
        private readonly IInvertoryUnitOfWork _context;
        private readonly IUnitCommodityQuotaRepository _UnitCommodityQuotaRepository;

        public UnitCommodityQuotaQueries(
                IInvertoryUnitOfWork context,
                IUnitCommodityQuotaRepository UnitCommodityQuotaRepository,
                IMapper mapper
            )
        {
            _mapper = mapper;
            _context = context;
            _UnitCommodityQuotaRepository = UnitCommodityQuotaRepository;
        }

        public async Task<UnitCommodityQuotaModel> GetById(int id)
        {
            var entity = await _UnitCommodityQuotaRepository.Find(id);
            return _mapper.Map<UnitCommodityQuotaModel>(entity);
        }
        
        public async Task<PagedList<UnitCommodityQuotaModel>> GetAll(PaginatedQueryModel query)
        {
                var entities = _context.UnitCommodityQuotaView
                          .ProjectTo<UnitCommodityQuotaModel>(_mapper.ConfigurationProvider)
                          .FilterQuery(query.Conditions)
                          .OrderByMultipleColumns(query.OrderByProperty);
                return new PagedList<UnitCommodityQuotaModel>()
                {

                    Data = await entities.Paginate(query.Paginator()).ToListAsync(),
                    TotalCount = query.PageIndex <= 1
                        ? await entities
                            .CountAsync()
                        : 0
                };
        }
        public async Task<PagedList<UnitsModel>> GetAllUnits(PaginatedQueryModel query)
        {
                var entities = _context.Units
                           .ProjectTo<UnitsModel>(_mapper.ConfigurationProvider)
                           .FilterQuery(query.Conditions)
                           .OrderByMultipleColumns(query.OrderByProperty);
                return new PagedList<UnitsModel>()
                {

                    Data = await entities.Paginate(query.Paginator()).ToListAsync(),
                    TotalCount = query.PageIndex <= 1
                        ? await entities
                            .CountAsync()
                        : 0
                };
        }
        public async Task<PagedList<QuotaGroupModel>> GetAllQuotaGroup(PaginatedQueryModel query)
        {
            var entities = _context.QuotaGroups
                       .ProjectTo<QuotaGroupModel>(_mapper.ConfigurationProvider)
                       .FilterQuery(query.Conditions)
                       .OrderByMultipleColumns(query.OrderByProperty);
                       
            return new PagedList<QuotaGroupModel>()
            {

                Data = await entities.Paginate(query.Paginator()).ToListAsync(),
                TotalCount = query.PageIndex <= 1
                    ? await entities
                        .CountAsync()
                    : 0
            };
        }

    }
    
}
