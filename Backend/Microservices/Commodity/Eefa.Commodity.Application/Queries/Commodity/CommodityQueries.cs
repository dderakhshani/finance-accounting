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
using System.Linq.Dynamic.Core;

namespace Eefa.Commodity.Application.Queries.Commodity
{
    public class CommodityQueries : ICommodityQueries
    {

        private readonly CommodityUnitOfWork _context;
        private readonly IMapper _mapper;

        public CommodityQueries(
            CommodityUnitOfWork context,
            IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<ServiceResult<CommodityModel>> GetById(int id)
        {
            return ServiceResult<CommodityModel>.Success(
                  await _context.Commodities
                 .Where(x => x.Id == id)
                 .ProjectTo<CommodityModel>(_mapper.ConfigurationProvider)
                 .FirstOrDefaultAsync()
             );
        }
        public async Task<ServiceResult<PagedList<CommodityModel>>> GetAll(int CommodityCategoryId,PaginatedQueryModel query)
        {
            var commodityCategory = _context.CommodityCategories.Where(a => a.Id == CommodityCategoryId).FirstOrDefault();

            var commodityCategoryListId = _context.CommodityCategories.Where(a => CommodityCategoryId == 0 || a.LevelCode.StartsWith(commodityCategory.LevelCode)).Select(a => a.Id).ToList();

            var commodities = (from com in _context.CommoditeisView.Where(c => commodityCategoryListId.Contains((int)c.CommodityCategoryId) || CommodityCategoryId == 0)

                                   //left join----------------------
                               join item in _context.DocumentItems.Where(a => a.IsWrongMeasure == true).Select(a => a.CommodityId).Distinct() on com.Id equals item into item_com

                               from item in item_com.DefaultIfEmpty()

                               select new CommodityModel
                               {
                                   Id = com.Id,
                                   Title = com.Title,
                                   CommodityCategoryId = com.CommodityCategoryId,
                                   Code = com.Code,
                                   CommodityCategoryTitle = com.CategoryTitle,
                                   CommodityNationalId = com.CommodityNationalId,
                                   //ThirdCode = com.CompactCode,
                                   CommodityNationalTitle = com.CommodityNationalTitle,
                                   //InventoryType = com.InventoryType,
                                   IsActive=com.IsActive,
                                   MeasureTitle = com.MeasureTitle,
                                   BomsCount = com.BomsCount,

                               })
                                  .Distinct()
                                  .FilterQuery(query.Conditions)
                                  .OrderByMultipleColumns(query.OrderByProperty);
                                      
                                      
                                      
                

                return ServiceResult<PagedList<CommodityModel>>.Success(
                     new PagedList<CommodityModel>()
                     {
                         Data =await commodities.Paginate(query.Paginator()).ToListAsync(),

                         TotalCount = query.PageIndex <= 1 ?await commodities.CountAsync() : 0
                     }
                    );
           

        }

        public async Task<ServiceResult<CommodityCategoryModel>> GetCategoryById(int id)
        {
            return ServiceResult<CommodityCategoryModel>.Success(
                await _context.CommodityCategories
                .Where(x => x.Id == id)
                .ProjectTo<CommodityCategoryModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync()
                );
        }
        public async Task<ServiceResult<PagedList<CommodityCategoryModel>>> GetAllCategories(PaginatedQueryModel query)
        {
            return ServiceResult<PagedList<CommodityCategoryModel>>.Success(
                 new PagedList<CommodityCategoryModel>()
                 {
                     Data = await _context.CommodityCategories
                                            .ProjectTo<CommodityCategoryModel>(_mapper.ConfigurationProvider)
                                            .FilterQuery(query.Conditions)
                                            .OrderByMultipleColumns(query.OrderByProperty)
                                            .Paginate(query.Paginator())
                                            .ToListAsync(),

                     TotalCount = query.PageIndex <= 1 ? await _context.CommodityCategories.CountAsync() : 0
                 }
                );
        }

        public async Task<ServiceResult<List<CommodityCategoryModel>> > GetCategoryParentTree(string levelCode)
        {
            var levelCodes = new List<string>();
            while (levelCode.Length >= 4)
            {
                levelCode = levelCode.Length > 4 ? levelCode.Substring(0, levelCode.Length - 4) : "" ;
                levelCodes.Add(levelCode);
            }

            List<QueryCondition> conditions = new List<QueryCondition>();
            conditions.Add(new QueryCondition {
                PropertyName = "levelCode",
                Comparison = "in",
                Values = levelCodes.ToArray()
            }) ;
            return ServiceResult<List<CommodityCategoryModel>>.Success(
                              await _context.CommodityCategories
                                        .ProjectTo<CommodityCategoryModel>(_mapper.ConfigurationProvider)
                                        .FilterQuery(conditions)
                                        .ToListAsync()
            );
        }

        public async Task<ServiceResult<CommodityCategoryPropertyModel>> GetCategoryPropertyById(int id)
        {
            return ServiceResult<CommodityCategoryPropertyModel>.Success(
                await _context.CommodityCategoryProperties
               .Where(x => x.Id == id)
               .ProjectTo<CommodityCategoryPropertyModel>(_mapper.ConfigurationProvider)
               .FirstOrDefaultAsync()
                );
        }
        public async Task<ServiceResult<PagedList<CommodityCategoryPropertyModel>>> GetAllCategoryProperties(PaginatedQueryModel query)
        {
            var q = _context.CommodityCategoryProperties
                                          .ProjectTo<CommodityCategoryPropertyModel>(_mapper.ConfigurationProvider)
                                          .FilterQuery(query.Conditions)
                                         
                                          .OrderByMultipleColumns(query.OrderByProperty);

            var result = await q.Paginate(query.Paginator()).OrderBy(a => a.OrderIndex).ToListAsync();
            foreach (var item in result)
            {
                item.Items = item.Items.Where(a => !a.IsDeleted).OrderBy(a=>a.OrderIndex).ToList();
            }
            return ServiceResult<PagedList<CommodityCategoryPropertyModel>>.Success(
                new PagedList<CommodityCategoryPropertyModel>()
                {
                    Data = result,

                    TotalCount = query.PageIndex <= 1 ? await q.CountAsync() : 0
                }
            );
        }

        public async Task<ServiceResult<CommodityCategoryPropertyItemModel>> GetCategoryPropertyItemById(int id)
        {
            return ServiceResult<CommodityCategoryPropertyItemModel>.Success(
                 await _context.CommodityCategoryPropertyItems
               .Where(x => x.Id == id)
               .ProjectTo<CommodityCategoryPropertyItemModel>(_mapper.ConfigurationProvider)
               .FirstOrDefaultAsync()
                );

        }
        public async Task<ServiceResult<PagedList<CommodityCategoryPropertyItemModel>>> GetAllCategoryPropertyItems(PaginatedQueryModel query)
        {
            return ServiceResult<PagedList<CommodityCategoryPropertyItemModel>>.Success(
                 new PagedList<CommodityCategoryPropertyItemModel>()
                 {
                     Data = await _context.CommodityCategoryPropertyItems.Where(a=> !a.IsDeleted)
                                       .ProjectTo<CommodityCategoryPropertyItemModel>(_mapper.ConfigurationProvider)
                                       .FilterQuery(query.Conditions)
                                       .OrderByMultipleColumns(query.OrderByProperty)
                                       .Paginate(query.Paginator())
                                       .ToListAsync(),

                     TotalCount = query.PageIndex <= 1 ? await _context.CommodityCategoryPropertyItems.CountAsync() : 0
                 }
                );
        }

        public async Task<ServiceResult<CommodityPropertyValueModel>> GetPropertyValueById(int id)
        {
            return ServiceResult<CommodityPropertyValueModel>.Success(
                  await _context.CommodityPropertyValues
                 .Where(x => x.Id == id)
                 .ProjectTo<CommodityPropertyValueModel>(_mapper.ConfigurationProvider)
                 .FirstOrDefaultAsync()
                );
        }
        public async Task<ServiceResult<PagedList<CommodityPropertyValueModel>>> GetAllPropertyValues(PaginatedQueryModel query)
        {
            return ServiceResult<PagedList<CommodityPropertyValueModel>>.Success(
                 new PagedList<CommodityPropertyValueModel>()
                 {
                     Data = await _context.CommodityPropertyValues
                                       .ProjectTo<CommodityPropertyValueModel>(_mapper.ConfigurationProvider)
                                       .FilterQuery(query.Conditions)
                                       .OrderByMultipleColumns(query.OrderByProperty)
                                       .Paginate(query.Paginator())
                                       .ToListAsync(),

                     TotalCount = query.PageIndex <= 1 ? await _context.CommodityPropertyValues.CountAsync() : 0
                 }
                );

        }
    }
}
