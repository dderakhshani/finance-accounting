using AutoMapper;
using AutoMapper.QueryableExtensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;
using Eefa.Common;
using Eefa.Commodity.Data.Context;

namespace Eefa.Commodity.Application.Queries.Bom
{
    public class BomQueries : IBomQueries
    {
        private readonly IMapper _mapper;
        private readonly ICommodityUnitOfWork _context;
        private readonly IRepository<Data.Entities.Bom> _repositoryBom;
        private readonly IRepository<Data.Entities.BomItem> _repositoryBomItem;
        private readonly IRepository<Data.Entities.BomValueHeader> _repositoryBomValueHeader;
        private readonly IRepository<Data.Entities.BomValue> _repositoryBomValue;
        

        public BomQueries(IRepository<Data.Entities.Bom> repositoryBom,
                          IRepository<Data.Entities.BomItem> repositoryBomItem,
                          IRepository<Data.Entities.BomValueHeader> repositoryBomValueHeader,
                          IRepository<Data.Entities.BomValue> repositoryBomValue,
                          ICommodityUnitOfWork context,
            IMapper mapper)
        {
            _mapper = mapper;
            _repositoryBom = repositoryBom;
            _repositoryBomItem = repositoryBomItem;
            _repositoryBomValueHeader = repositoryBomValueHeader;
            _repositoryBomValue = repositoryBomValue;
            _context = context;
                
        }

        public async Task<ServiceResult<BomModel>> GetBomById(int id)
        {
            var entity = await _repositoryBom.Find(id);
            var Items =await _repositoryBomItem.GetAll().Where(x => x.BomId == id).ToListAsync();

           

            var result = _mapper.Map<BomModel>(entity);

            var BomItemModels = (from rep in Items
                                join
                                com in _context.Commodities on rep.CommodityId equals com.Id
                               select new BomItemModel()
                               {
                                   Id = rep.Id,
                                   CommodityId = rep.CommodityId,
                                   
                                   SubCategoryId = rep.SubCategoryId,
                                   CommodityCode = com.Code,
                                   CommodityTitle = com.Title

                               }).ToList();
            result.Items = BomItemModels;
            return ServiceResult<BomModel>.Success(result);
           
        }

        public async Task<ServiceResult<PagedList<BomModel>>> GetBoms(PaginatedQueryModel query)
        {
            
                var entities = _context.BomsView.ProjectTo<BomModel>(_mapper.ConfigurationProvider)
                           .FilterQuery(query.Conditions)
                           
                           .OrderByMultipleColumns(query.OrderByProperty);
                return ServiceResult<PagedList<BomModel>>.Success(new PagedList<BomModel>()
                {
                    Data = await entities.Paginate(query.Paginator()).ToListAsync(),
                    TotalCount = query.PageIndex <= 1
                        ? await entities
                            .CountAsync()
                        : 0
                });
          

        }
        public async Task<ServiceResult<PagedList<BomValueHeaderModel>>> GetBomValueHeadersByCommodityId(int commodityId, PaginatedQueryModel query)
        {
            var entitis = _repositoryBomValueHeader.GetAll()
                      .Where(c => c.CommodityId == commodityId)
                      .ProjectTo<BomValueHeaderModel>(_mapper.ConfigurationProvider)
                      .FilterQuery(query.Conditions)
                     
                      .OrderByMultipleColumns(query.OrderByProperty);
            return ServiceResult<PagedList<BomValueHeaderModel>>.Success(new PagedList<BomValueHeaderModel>()
            {
                Data = await entitis.Paginate(query.Paginator()).ToListAsync(),
                TotalCount = query.PageIndex <= 1
                    ? await entitis
                        .CountAsync()
                    : 0
            });

        }
        public async Task<ServiceResult<PagedList<BomValueHeaderModel>>> GetAllBomValueHeaders(PaginatedQueryModel query)
        {

            var entities = _repositoryBomValueHeader.GetAll()
                      .ProjectTo<BomValueHeaderModel>(_mapper.ConfigurationProvider)
                      .FilterQuery(query.Conditions)
                     
                      .OrderByMultipleColumns(query.OrderByProperty);
            return ServiceResult<PagedList<BomValueHeaderModel>>.Success(new PagedList<BomValueHeaderModel>()
            {
                Data = await entities.ToListAsync(),
                TotalCount = query.PageIndex <= 1
                    ? await entities.Paginate(query.Paginator())
                        .CountAsync()
                    : 0
            });

        }
        public async Task<ServiceResult<BomValueHeaderModel>> GetBomValueHeaderById(int id)
        {
            var entity = await _repositoryBomValueHeader.Find(id);
           
            var Items = await (from rep in _context.BomValues
                               join
                               com in _context.Commodities on rep.UsedCommodityId equals com.Id
                               join
                               mus in _context.MeasureUnits on com.MeasureId equals mus.Id
                               where rep.BomValueHeaderId ==id
                               select new BomValueModel()
                               {
                                   Id = rep.Id,
                                   UsedCommodityId = rep.UsedCommodityId,
                                   BomWarehouseId= rep.BomWarehouseId,
                                   Value = rep.Value,
                                   MainMeasureTitle= mus.Title,
                                   BomValueHeaderId= rep.BomValueHeaderId,
                                   CommodityCode=com.Code,
                                   CommodityTitle= com.Title,
                                   
                               }).ToListAsync();
             var result = _mapper.Map<BomValueHeaderModel>(entity);
             result.Values = _mapper.Map<List<BomValueModel>>(Items);
            return ServiceResult<BomValueHeaderModel>.Success(result);
            
        }
        //-------------------------------------------------------------------------
        public async Task<ServiceResult<PagedList<BomModel>>> GetBomsByCommodityCategoryId( PaginatedQueryModel query)
        {
            
           

            var entities = _repositoryBom.GetAll().ProjectTo<BomModel>(_mapper.ConfigurationProvider)
                          
                           .FilterQuery(query.Conditions)
                           .OrderByMultipleColumns(query.OrderByProperty);
            return ServiceResult<PagedList<BomModel>>.Success(new PagedList<BomModel>()
            {
                Data = await entities.Paginate(query.Paginator()).ToListAsync(),
                TotalCount = query.PageIndex <= 1
                    ? await entities
                        .CountAsync()
                    : 0
            });

        }

        public async Task<ServiceResult<PagedList<BomItemModel>>> GetBomItemsByBomId(int bomId, PaginatedQueryModel query)
        {
            
                var entities = _repositoryBomItem.GetAll().Where(c => c.BomId == bomId).ProjectTo<BomItemModel>(_mapper.ConfigurationProvider)
                           .FilterQuery(query.Conditions)
                           .OrderByMultipleColumns(query.OrderByProperty);
           
            return ServiceResult<PagedList<BomItemModel>>.Success(new PagedList<BomItemModel>()
            {
                Data = await entities.Paginate(query.Paginator()).ToListAsync(),
                TotalCount = query.PageIndex <= 1
                    ? await entities
                        .CountAsync()
                    : 0
            });


        }

        public async Task<PagedList<BomValueHeaderModel>> GetBomValueHeadersByBomId(int bomId, PaginatedQueryModel query)
        {
            
                var entities = _repositoryBomValueHeader.GetAll()
                           .Where(c => c.BomId==bomId)
                           .ProjectTo<BomValueHeaderModel>(_mapper.ConfigurationProvider)
                           .FilterQuery(query.Conditions)
                           
                           .OrderByMultipleColumns(query.OrderByProperty);
            return new PagedList<BomValueHeaderModel>()
            {
                Data = (IEnumerable<BomValueHeaderModel>)await entities
                    
                    .ToListAsync(),
                TotalCount = query.PageIndex <= 1
                    ? await entities.Paginate(query.Paginator())
                        .CountAsync()
                    : 0
            };
            
        }
       
        public async Task<BomValueModel> GetBomValueById(int id)
        {
           
            
            var entity = await _repositoryBomValue.Find(id);
            return _mapper.Map<BomValueModel>(entity);
        }

        public async Task<PagedList<BomValueModel>> GetBomValuesByBomValueHeaderId(int bomValueHeaderId, PaginatedQueryModel query)
        {
            var entities = _repositoryBomValue.GetAll()
                           .Where(c => c.BomValueHeaderId == bomValueHeaderId)
                           .ProjectTo<BomValueModel>(_mapper.ConfigurationProvider)
                           .FilterQuery(query.Conditions)
                           .OrderByMultipleColumns(query.OrderByProperty);
            return new PagedList<BomValueModel>()
            {
                Data =await entities.Paginate(query.Paginator()).ToListAsync(),
                TotalCount = query.PageIndex <= 1
                    ? await entities
                        .CountAsync()
                    : 0
            };

        }

    }
}
