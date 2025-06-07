using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;
using Eefa.Inventory.Application.Models.CommodityCategory;
using Eefa.Inventory.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Eefa.Inventory.Domain;
namespace Eefa.Inventory.Application
{
    public class CommodityCategoryQueries : ICommodityCategoryQueries
    {

        private readonly IInvertoryUnitOfWork _contex;
        private readonly IMapper _mapper;

        public CommodityCategoryQueries(IMapper mapper, IInvertoryUnitOfWork contex)
        {
            _mapper = mapper;

            _contex = contex;
        }
        public async Task<PagedList<CommodityCategoryModel>> GetCategores(int? ParentId, int WarehouseId, PaginatedQueryModel query)
        {
            List<CommodityCategoryModel> entites = new List<CommodityCategoryModel>();


            if (ParentId > 0) //-----------از لیست پدر  مقدارهای گروه محصول پدر خود را بگیرد------------------------------------------------
            {
                entites = await SelectCategoryByParentlayout(ParentId, entites);

                if (entites.Count() == 0)//----------در سطح پدر هیچ گروه محصولی انتخاب نشده است
                {
                    entites = await selectCategoryByWarehouseId(WarehouseId, entites);
                }

            }
            else //در سطح یک قرار داریم و باید ببینیم که خود این انبار برای چه گروه محصولاتی تعریف شده است------------------------------------
            {
                entites = await selectCategoryByWarehouseId(WarehouseId, entites);

            }
            //---------------------------------اگر فرزندی انتخاب شده باشد که پدر ندارد ، پدر آن خالی شود--------------------------------------
            int hasParent = 0;
            entites.ForEach(a =>
            {
                hasParent = 0;
                entites.ForEach(sub =>
                {
                    if (a.ParentId == sub.Id)
                    {
                        hasParent = 1;
                    }
                });
                if (hasParent == 0)
                {
                    a.ParentId = null;
                }
            });
            //------------------------------------------------------------------------------------------------------------------------------------

            return new PagedList<CommodityCategoryModel>()
            {
                Data = entites,
                TotalCount = 0

            };

        }

        private async Task<List<CommodityCategoryModel>> SelectCategoryByParentlayout(int? ParentId, List<CommodityCategoryModel> entites)
        {
            var items = await (from wc in _contex.WarehouseLayoutCategories.Where(a => !a.IsDeleted)
                               join w in _contex.WarehouseLayouts.Where(a => !a.IsDeleted) on wc.WarehouseLayoutId equals w.Id
                               join cat in _contex.CommodityCategories.Where(a => !a.IsDeleted) on wc.CategoryId equals cat.Id
                               where (w.Id == ParentId || w.ParentId == ParentId)
                               select (cat)
                 ).ToListAsync();


            entites = _contex.CommodityCategories.Where(a => !a.IsDeleted).ProjectTo<CommodityCategoryModel>(_mapper.ConfigurationProvider).ToList()
                             .Where(x =>
                                 items.Any(par => x.LevelCode.Length >= par.LevelCode.Length &&
                                                 (x.LevelCode.Substring(0, par.LevelCode.Length) == par.LevelCode)
                                           )
                                 ).ToList();
            return entites;
        }

        private async Task<List<CommodityCategoryModel>> selectCategoryByWarehouseId(int WarehouseId, List<CommodityCategoryModel> entites)
        {
            var categoryId = _contex.Warehouses.Where(a => !a.IsDeleted && a.Id == WarehouseId).Select(a => a.CommodityCategoryId).FirstOrDefault();
            var categoryLevelCode = _contex.CommodityCategories.Where(a => a.Id == categoryId).Select(a => a.LevelCode).FirstOrDefault();

            entites = await _contex.CommodityCategories.Where(a => !a.IsDeleted &&
                                                                      (categoryLevelCode == null ||
                                                                      (a.LevelCode.Length >= categoryLevelCode.Length &&
                                                                      (a.LevelCode.Substring(0, categoryLevelCode.Length) == categoryLevelCode)))
                ).ProjectTo<CommodityCategoryModel>(_mapper.ConfigurationProvider).ToListAsync();
            return entites;
        }
        public async Task<PagedList<CommodityCategoryPropertyModel>> GetPropertyByWarehouseLayoutId(int WarehouseLayoutId, PaginatedQueryModel query)
        {
            List<CommodityCategoryPropertyModel> entites = new List<CommodityCategoryPropertyModel>();

            //-----------از لیست پدر خود مقدارهای گروه محصول پدر خود را بگیرد------------------------------------------------
            var categories = await (from lp in _contex.WarehouseLayoutCategories.Where(a => !a.IsDeleted)
                                    join cp in _contex.WarehouseLayouts.Where(a => !a.IsDeleted) on lp.WarehouseLayoutId equals cp.Id
                                    where cp.Id == WarehouseLayoutId
                                    select (lp)
                 ).ToListAsync();

            entites = _contex.CommodityCategoryProperties.Where(a => !a.IsDeleted).ProjectTo<CommodityCategoryPropertyModel>(_mapper.ConfigurationProvider).ToList()
                            .Where(x => categories.Any(y => (y.CategoryId == x.CategoryId))).ToList();



            return new PagedList<CommodityCategoryPropertyModel>()
            {
                Data = entites,
                TotalCount = 0

            };
        }

        public async Task<PagedList<CommodityCategoryPropertyItemModel>> GetPropertyItemsByWarehouseLayoutId(int WarehouseLayoutId, PaginatedQueryModel query)
        {
            List<CommodityCategoryPropertyItemModel> entites = new List<CommodityCategoryPropertyItemModel>();

            //-----------از لیست پدر خود مقدارهای گروه محصول پدر خود را بگیرد-------------------------
            var categories = await (from lp in _contex.WarehouseLayoutCategories.Where(a => !a.IsDeleted)
                                    join cp in _contex.WarehouseLayouts.Where(a => !a.IsDeleted) on lp.WarehouseLayoutId equals cp.Id
                                    where cp.Id == WarehouseLayoutId
                                    select (lp)
                 ).ToListAsync();
            //-----------------------------------------------------------------------------------------
            var property = _contex.CommodityCategoryProperties.Where(a => !a.IsDeleted).ProjectTo<CommodityCategoryPropertyModel>(_mapper.ConfigurationProvider).ToList()
                                 .Where(x => categories.Any(y => (y.CategoryId == x.CategoryId))).ToList();


            entites = _contex.CommodityCategoryPropertyItems.Where(a => !a.IsDeleted).ProjectTo<CommodityCategoryPropertyItemModel>(_mapper.ConfigurationProvider).ToList()
                            .Where(x => property.Any(y => (y.Id == x.CategoryPropertyId))).ToList();



            return new PagedList<CommodityCategoryPropertyItemModel>()
            {
                Data = entites,
                TotalCount = 0

            };
        }
        public async Task<PagedList<CommodityCategoryModel>> GetCategoresCodeAssetGroup(PaginatedQueryModel query)
        {
            var list = await _contex.CommodityCategories.Where(a => a.Code.StartsWith(ConstantValues.CommodityGroups.CodeAssetGroup) && !a.IsDeleted).
                ProjectTo<CommodityCategoryModel>(_mapper.ConfigurationProvider)

                .ToListAsync();

            var result = new PagedList<CommodityCategoryModel>()
            {
                Data = list,
                TotalCount = 0
            };
            return result;
        }
        
        public async Task<PagedList<CommodityCategoryModel>> GetAll(PaginatedQueryModel paginatedQuery)
        {

            var entitis = _contex.CommodityCategories
            .ProjectTo<CommodityCategoryModel>(_mapper.ConfigurationProvider)
            .FilterQuery(paginatedQuery.Conditions)
            .Paginate(paginatedQuery.Paginator())
            .OrderByMultipleColumns(paginatedQuery.OrderByProperty);
            var result = new PagedList<CommodityCategoryModel>()
            {

                Data = await entitis.ToListAsync(),
                TotalCount = paginatedQuery.PageIndex <= 1
                    ? await entitis
                        .CountAsync()
                    : 0
            };

            return result;

        }
        public async Task<PagedList<CommodityCategoryModel>> GetTreeAll(PaginatedQueryModel query)
        {

            var entitis = _contex.CommodityCategories.Where(a => !a.IsDeleted).FilterQuery(query.Conditions).ProjectTo<CommodityCategoryModel>(_mapper.ConfigurationProvider);
            if (entitis == null)
            {
                return null;
            }
            var list = BuildTree(entitis.OrderBy(x => x.ParentId).ToList());
            return new PagedList<CommodityCategoryModel>()
            {
                Data = list,
                TotalCount = query.PageIndex <= 1
                     ? await entitis
                         .CountAsync()
                     : 0
            };

        }

        public List<CommodityCategoryModel> BuildTree(List<CommodityCategoryModel> source)
        {
            if (!source.Any())
            {
                return null;
            }
            var groups = source.GroupBy(i => i.ParentId);

            var roots = groups.FirstOrDefault(g => g.Key.HasValue == false).ToList();

            if (roots.Count > 0)
            {
                var dict = groups.Where(g => g.Key.HasValue).ToDictionary(g => g.Key.Value, g => g.ToList());
                for (int i = 0; i < roots.Count; i++)
                    AddChildren(roots[i], dict);
            }

            return roots;
        }
        private void AddChildren(CommodityCategoryModel node, IDictionary<int, List<CommodityCategoryModel>> source)
        {
            if (source.ContainsKey(node.Id))
            {
                node.Children = source[node.Id];
                for (int i = 0; i < node.Children.Count; i++)
                    AddChildren(node.Children[i], source);
            }
            else
            {
                node.Children = new List<CommodityCategoryModel>();
            }
        }
    }
}
