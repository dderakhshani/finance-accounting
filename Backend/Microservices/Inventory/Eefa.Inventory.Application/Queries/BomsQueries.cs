using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;
using Eefa.Inventory.Application.Models;
using Microsoft.EntityFrameworkCore;
using Eefa.Inventory.Domain;

namespace Eefa.Inventory.Application
{
    public class BomsQueries : IBomsQueries
    {

        private readonly IMapper _mapper;
        private readonly IInvertoryUnitOfWork _contex;

        public BomsQueries(
              IMapper mapper
            , IInvertoryUnitOfWork contex
            )
        {
            _mapper = mapper;
            _contex = contex;

        }
        public async Task<PagedList<CommodityBomsModel>> GetAll(string FromDate,string ToDate, PaginatedQueryModel query)
            
        {

            var entitis = queryable();

            var list = await entitis.FilterQuery(query.Conditions)
                         .OrderByMultipleColumns()
                         .Paginate(query.Paginator())
                         .Distinct()
                         .ToListAsync();
            
            var result = new PagedList<CommodityBomsModel>()
            {
                Data = list,
                TotalCount = query.PageIndex <= 1
                            ? entitis.Count()

                            : 0
            };
            return result;
        }
        
        public async Task<PagedList<CommodityBomsModel>> GetByCommodityId(int CommodityId)
        {
            
                var entitis = queryable().Where(a => a.CommodityId == CommodityId && a.IsActive == true);

                var list = await entitis.OrderByMultipleColumns().Distinct().ToListAsync();

                var result = new PagedList<CommodityBomsModel>()
                {
                    Data = list,
                    TotalCount = 0
                };
                return result;
            
            
        }

        public async Task<CommodityBomsModel> GetById(int id)
        {
            var entity = await _contex.Boms.Where(a => a.Id == id).FirstOrDefaultAsync();
            return _mapper.Map<CommodityBomsModel>(entity);
        }

        
        private IQueryable<CommodityBomsModel> queryable()
        {
            return (from Boms in _contex.Boms
                    join Bomv_h in _contex.BomValueHeaders on Boms.Id equals Bomv_h.BomId
                    where Boms.IsDeleted == false && Bomv_h.IsDeleted == false

                    select new CommodityBomsModel
                    {
                        Id = Boms.Id,
                        BomsId = Bomv_h.BomId,
                        BomsHeaderId = Bomv_h.Id,
                        RootId = Boms.RootId,
                        LevelCode = Boms.LevelCode,
                        CommodityCategoryId = Boms.CommodityCategoryId,
                        Title = Boms.Title,
                        IsActive = Boms.IsActive,
                        CommodityId = Bomv_h.CommodityId,
                        Name = Bomv_h.Name,
                        BomDate = Bomv_h.BomDate,
                        BomWarehouseId = _contex.BomValues.Where(a => a.BomValueHeaderId == Bomv_h.Id).Select(a => a.BomWarehouseId).FirstOrDefault()
                    }
                       ); 

        }

    }

}
