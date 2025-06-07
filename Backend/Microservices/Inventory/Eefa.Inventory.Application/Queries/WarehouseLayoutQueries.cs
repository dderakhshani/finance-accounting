using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;
using Eefa.Inventory.Domain;
using Microsoft.EntityFrameworkCore;
using Eefa.Inventory.Domain.Common;
using Eefa.Common;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Eefa.Inventory.Domain.Enum;

namespace Eefa.Inventory.Application
{
    public class WarehouseLayoutQueries : IWarehouseLayoutQueries
    {

        private readonly IInvertoryUnitOfWork _context;
        private readonly IProcedureCallService _iProcedureCallService;
        private readonly IMapper _mapper;
        private readonly ICurrentUserAccessor _currentUserAccessor;

        public WarehouseLayoutQueries(
            IMapper mapper,
            IInvertoryUnitOfWork context,
            IProcedureCallService iProcedureCallService,
            ICurrentUserAccessor currentUserAccessor

            )
        {
            _mapper = mapper;
            _context = context;
            _iProcedureCallService = iProcedureCallService;
            _currentUserAccessor = currentUserAccessor;
        }

        public async Task<WarehouseLayoutGetIdModel> GetById(int id)
        {
            var entity = await _context.WarehouseLayouts.Where(x => x.Id == id && !x.IsDeleted).ProjectTo<WarehouseLayoutGetIdModel>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();

            List<WarhousteCategoryItemsModel> models = new List<WarhousteCategoryItemsModel>();
            if (entity != null)
            {

                foreach (var item in entity.WarehouseLayoutCategories.Where(a => !a.IsDeleted).ToList())
                {
                    WarhousteCategoryItemsModel model = new WarhousteCategoryItemsModel();
                    List<WarhousteCategoryItemsModel> pro_models = new List<WarhousteCategoryItemsModel>();
                    model.CommodityCategoryId = item.CategoryId;
                    model.WarehouseLayoutCategoriesId = item.Id;

                    var Properties = entity.WarehouseLayoutProperties.Where(a => a.WarehouseLayoutCategoryId == item.Id && !a.IsDeleted).ToList();

                    var group = Properties.GroupBy(a => a.CategoryPropertyId).Select(a => a.First()).ToList();
                    foreach (var property in group)
                    {

                        WarhousteCategoryItemsModel pro = new WarhousteCategoryItemsModel();

                        pro.CategoryPropertyId = property.CategoryPropertyId;
                        pro.WarehouseLayoutPropertiesId = property.Id;
                        pro.WarehouseLayoutId = property.WarehouseLayoutId;
                        pro.Items = new List<WarhousteCategoryItemsModel>();
                        pro_models.Add(pro);

                        foreach (var ItemId in Properties)
                            if (ItemId.CategoryPropertyItemId != null)
                            {
                                WarhousteCategoryItemsModel pro_item = new WarhousteCategoryItemsModel();
                                pro_item.CategoryPropertyId = ItemId.CategoryPropertyId;
                                pro_item.WarehouseLayoutPropertiesId = ItemId.Id;
                                pro_item.WarehouseLayoutId = ItemId.WarehouseLayoutId;
                                pro_item.CategoryPropertyItemId = ItemId.CategoryPropertyItemId;
                                pro_item.ValueItem = ItemId.Value;
                                pro_item.WarehouseLayoutCategoriesId = item.Id;
                                pro.Items.Add(pro_item);

                            }
                    }

                    model.Items = pro_models;
                    models.Add(model);

                }
            }
            if (entity != null && models != null)
                entity.Items = models;

            return entity;
        }

        public async Task<PagedList<WarehouseLayoutModel>> GetAll(PaginatedQueryModel query)
        {

            var entities = _context.WarehouseLayouts.Where(a => !a.IsDeleted)

                .ProjectTo<WarehouseLayoutModel>(_mapper.ConfigurationProvider)
                .FilterQuery(query.Conditions);



            return new PagedList<WarehouseLayoutModel>()
            {
                Data = await entities.Paginate(query.Paginator()).OrderBy(x => x.ParentId).ThenBy(x => x.OrderIndex).ToListAsync(),
                TotalCount = query.PageIndex <= 1
                    ? await entities
                        .CountAsync()
                    : 0
            };


        }
        /// <summary>
        /// محل قرار گرفتن هر کالا در انبار با توجه به نوع سند
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<PagedList<WarehouseHistoriesDocumentViewModel>> GetAllHistoriesDocument(string FromDate, string ToDate, PaginatedQueryModel query)
        {

            DateTime? from = FromDate == null ? null : Convert.ToDateTime(FromDate).ToUniversalTime();
            DateTime? to = ToDate == null ? null : Convert.ToDateTime(ToDate).ToUniversalTime();
            List<int> AllowAccessToWareHouse = AccessWarehouse();
            var entities = _context.WarehouseHistoriesDocumentView.Where(a => (a.DocumentDate >= from || from == null)
                                                                         && (a.DocumentDate <= to || to == null)
                                                                         && (AllowAccessToWareHouse.Contains((int)a.WarehouseId))
                                                                         )

                  .FilterQuery(query.Conditions)
                  .ProjectTo<WarehouseHistoriesDocumentViewModel>(_mapper.ConfigurationProvider)
                  .OrderByMultipleColumns(query.OrderByProperty);


            var list = entities.Paginate(query.Paginator());

            return new PagedList<WarehouseHistoriesDocumentViewModel>()
            {
                Data = (IEnumerable<WarehouseHistoriesDocumentViewModel>)await list.ToListAsync(),
                TotalCount = query.PageIndex <= 1
                ? await entities
                    .CountAsync()
                : 0
            };

        }
        /// <summary>
        /// مکان های قابل پیشنهاد دادن برای جایگذاری کالا براساس گروه محصول
        /// </summary>
        /// <param name="query"></param>
        /// <param name="commodityId"></param>
        /// <returns></returns>
        public async Task<PagedList<WarehouseLayoutModel>> GetSuggestionWarehouseLayoutByCommodityCategories(PaginatedQueryModel query, int commodityId)
        {
            var CommodityCategory = await _context.Commodities.Where(a => a.Id == commodityId).FirstOrDefaultAsync();

            var categoryId = await _context.CommodityCategories.Where(a => a.Id == CommodityCategory.CommodityCategoryId).FirstOrDefaultAsync();
            var categoryListId = await _context.WarehouseLayoutCategories.Where(a => a.CategoryId == categoryId.Id).Select(a => a.WarehouseLayoutId).ToListAsync();


            var entities = _context.WarehouseLayouts.Where(a => !a.IsDeleted && (categoryListId.Contains(a.Id) || categoryListId.Count() == 0))
                .ProjectTo<WarehouseLayoutModel>(_mapper.ConfigurationProvider)
                .FilterQuery(query.Conditions);



            var result = await entities.Paginate(query.Paginator()).OrderBy(x => x.ParentId).ThenBy(x => x.OrderIndex).ToListAsync();


            //------------------مجموع کالاهای قرارگرفته در این مکان-------------------------------
            var quantity = _context.WarehouseLayoutQuantities.Where(a => result.Select(a => a.Id).Contains(a.WarehouseLayoutId)).ToList();

            result.ForEach(r =>
            {

                r.CapacityUsed = quantity.Where(a => a.WarehouseLayoutId == r.Id).Sum(a => a.Quantity);

            });
            //------------------------------------------------------------------------------------

            return new PagedList<WarehouseLayoutModel>()
            {
                Data = result,
                TotalCount = query.PageIndex <= 1
                    ? await entities
                        .CountAsync()
                    : 0
            };

        }
        /// <summary>
        /// فرزندهای یک پدر با اطلاعات کامل از نظر ظرفیت و محصول داخل آن
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<PagedList<WarehouseLayoutModel>> GetParentIdAllChildByCapacityAvailable(int id)
        {
            var entities = _context.WarehouseLayouts.Where(a => (a.ParentId == id || a.Id == id) && !a.IsDeleted);

            List<string[]> ParentName = ParentNameList(id, entities);

            var result = await (from layout in entities

                                    //left join----------------------
                                join q in _context.WarehouseLayoutQuantities.Where(a => !a.IsDeleted) on layout.Id equals q.WarehouseLayoutId into ps

                                from q in ps.DefaultIfEmpty()
                                    //left join----------------------
                                join c in _context.Commodities on q.CommodityId equals c.Id into cq

                                from c in cq.DefaultIfEmpty()

                                select new WarehouseLayoutModel
                                {
                                    Id = layout.Id,
                                    ParentId = layout.ParentId,
                                    LevelCode = layout.LevelCode,
                                    Title = layout.Title,
                                    CommodityId = q.CommodityId,
                                    TotlaCapacity = layout.Capacity,
                                    Capacity = layout.Capacity,
                                    CapacityUsed = q.Quantity == null ? 0 : (double)q.Quantity,
                                    EntryMode = (int)layout.EntryMode,
                                    LastLevel = layout.LastLevel,
                                    OrderIndex = layout.OrderIndex,
                                    Status = layout.Status,
                                    CommodityTitle = c.Title,
                                    Categoreis = new List<WarehouseLayoutCategoryModel>()

                                }).OrderBy(x => x.ParentId).ThenBy(x => x.Title).ToListAsync();

            //------------------------گروه محصولات------------------------------------------
            var ListId = result.Select(a => a.Id).ToList();
            var Categories = (from categoryLayout in _context.WarehouseLayoutCategories.Where(b => !b.IsDeleted && ListId.Contains(b.WarehouseLayoutId))
                              join category in _context.CommodityCategories.Where(b => !b.IsDeleted) on categoryLayout.CategoryId equals category.Id
                              select new WarehouseLayoutCategoryModel
                              {
                                  CategoryId = category.Id,
                                  WarehouseLayoutId = categoryLayout.WarehouseLayoutId,
                                  CategoryTitle = category.Title,
                                  Id = categoryLayout.Id

                              }).ToList();

            //------------------------------------------------------------------------------
            result.ForEach(a =>
            {

                a.ParentNameString = ParentName;

                //----------------------------ظرفیت قابل مصرف تقسیم می شود بین محصولات مختلفی که در این مکان جای دهی شده است----------------
                a.Capacity = a.TotlaCapacity > 0 ? a.TotlaCapacity / result.Where(b => b.Id == a.Id).Count() : 0;

                //----------------------------ظرفیت کل مصرف شده هر مکان---------------------------------------------------------------------
                a.TotlaCapacityUsed = result.Where(b => b.Id == a.Id).Sum(a => a.CapacityUsed);
                //---------------------------------------------------------------------------------------------------------------------------
                a.CapacityUsedPercent = a.CapacityUsed != null ? RoundValueAndAdd(Convert.ToDouble(a.TotlaCapacityUsed * 100 / a.TotlaCapacity)) : 0;

                a.Categoreis = Categories.Where(b => !b.IsDeleted && b.WarehouseLayoutId == a.Id).ToList();


            });

            //---------ظرفیت استفاده شده پدر برایر مجموع ظرفیت های استفاده شده فرزندان باشد---
            if (result.Where(a => a.ParentId == null).Count() > 0)
            {
                var parentCapacityUsed = result.Where(a => a.Id != id).Sum(a => a.CapacityUsed);
                var parentCapacity = result.Where(a => a.Id != id).Sum(a => a.Capacity);

                result.Where(a => a.Id == id).First().CapacityUsed = parentCapacityUsed;
                result.Where(a => a.Id == id).First().Capacity = parentCapacity;
            }
            //----------------------------------------------------------------------------------
            return new PagedList<WarehouseLayoutModel>()
            {
                Data = result,
                TotalCount = 0

            };
        }
        private static double RoundValueAndAdd(double value)
        {


            value = Math.Round(value, MidpointRounding.AwayFromZero);
            return value;
        }
        private List<string[]> ParentNameList(int id, IQueryable<WarehouseLayout> entities)
        {
            var parentLeve = entities.Where(a => a.Id == id).Select(a => a.LevelCode).FirstOrDefault();

            List<string[]> ParentName = new List<string[]>(); ;
            if (parentLeve != null)
            {
                for (int i = 4; i <= parentLeve.Length; i = i + 4)
                {
                    string[] strings = new string[2];
                    var row = _context.WarehouseLayouts.Where(a => !a.IsDeleted && (a.LevelCode == parentLeve.Substring(0, i))).FirstOrDefault();
                    strings[0] = row.Title;
                    strings[1] = row.Id.ToString();
                    ParentName.Add(strings);
                }
            }
            return ParentName;
        }

        public async Task<PagedList<WarehouseLayoutModel>> GetTreeAll(PaginatedQueryModel query)
        {

            var entities = _context.WarehouseLayouts.Where(a => !a.IsDeleted).FilterQuery(query.Conditions).ProjectTo<WarehouseLayoutModel>(_mapper.ConfigurationProvider);
            if (entities == null)
            {
                return null;
            }
            var list = BuildTree(entities.OrderBy(x => x.ParentId).ThenBy(x => x.OrderIndex).ToList());
            return new PagedList<WarehouseLayoutModel>()
            {
                Data = list,
                TotalCount = query.PageIndex <= 1
                     ? await entities
                         .CountAsync()
                     : 0
            };

        }

        public List<WarehouseLayoutModel> BuildTree(List<WarehouseLayoutModel> source)
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
        private void AddChildren(WarehouseLayoutModel node, IDictionary<int, List<WarehouseLayoutModel>> source)
        {
            if (source.ContainsKey(node.Id))
            {
                node.Children = source[node.Id];
                for (int i = 0; i < node.Children.Count; i++)
                    AddChildren(node.Children[i], source);
            }
            else
            {
                node.Children = new List<WarehouseLayoutModel>();
            }
        }

        public async Task<PagedList<WarehouseLayoutModel>> GetAllByParentId(int warehouseId, int? parentId, PaginatedQueryModel query)
        {
            var entities = _context.WarehouseLayouts.Where(a => !a.IsDeleted && a.WarehouseId == warehouseId);
            if (parentId.HasValue && parentId!=0)
            {
                entities = entities.Where(a => a.ParentId == parentId);
            }
         
            var result = entities.OrderBy(x => x.ParentId).ThenBy(x => x.OrderIndex)
                .ProjectTo<WarehouseLayoutModel>(_mapper.ConfigurationProvider);

            
            return new PagedList<WarehouseLayoutModel>()
            {
                Data = await result.Paginate(query.Paginator()).ToListAsync(),
                TotalCount = query.PageIndex <= 1
                    ? await entities
                        .CountAsync()
                    : 0
            };
        }
        /// <summary>
        /// موجودی کالا در مکان های انبار
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>

        public async Task<PagedList<WarehouseLayoutsCommoditiesQuantityModel>> GetWarehouseLayoutCommodityId(PaginatedQueryModel query)
        {
            var entities = (from layout in _context.WarehouseLayoutsCommoditiesQuantityView
                            select new WarehouseLayoutsCommoditiesQuantityModel()
                            {
                                Id = layout.Id,
                                WarehouseId = layout.WarehouseId,
                                CommodityId = layout.CommodityId,
                                WarehouseLayoutId = layout.WarehouseLayoutId,
                                Quantity = layout.Quantity,
                                CommodityCode = layout.CommodityCode,
                                CommodityTadbirCode = layout.CommodityTadbirCode,
                                CommodityTitle = layout.CommodityTitle,
                                WarehouseTitle = layout.WarehouseTitle,
                                WarehouseLayoutTitle = layout.WarehouseLayoutTitle,
                                WarehouseLayoutCapacity = layout.WarehouseLayoutCapacity,

                            })
                          .OrderBy(a => a.WarehouseLayoutTitle)
                          .FilterQuery(query.Conditions)

                          .OrderByMultipleColumns(query.OrderByProperty);


            var result = (List<WarehouseLayoutsCommoditiesQuantityModel>)await entities.Paginate(query.Paginator()).ToListAsync();

            return new PagedList<WarehouseLayoutsCommoditiesQuantityModel>()
            {
                Data = result,
                TotalCount = query.PageIndex <= 1
                    ? await entities
                        .CountAsync()
                    : 0
            };


        }

        /// <summary>
        /// سابقه حضور کالا در انبار
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>

        public async Task<PagedList<StocksCommoditiesModel>> GetWarehouseLayoutHistoryCommodityId(string FromDate, string ToDate, PaginatedQueryModel query)
        {
            DateTime? from = FromDate == null ? null : Convert.ToDateTime(FromDate).ToUniversalTime();
            DateTime? to = ToDate == null ? null : Convert.ToDateTime(ToDate).ToUniversalTime();
            List<StocksCommoditiesModel> stocksCommodities = new List<StocksCommoditiesModel>();
            StocksCommoditiesModel stocksCommoditiesModel = new StocksCommoditiesModel();

            IOrderedQueryable<WarehouseLayoutsCommoditiesModel> entities = QueryWarehouseHistory(query, from, to);

            var result = (List<WarehouseLayoutsCommoditiesModel>)await entities.Where(a => a.Quantity > 0)
                                                                        .Paginate(query.Paginator())
                                                                        .OrderBy(a => a.CommodityTitle)
                                                                        .Distinct()
                                                                        .ToListAsync();
            result.ForEach(r =>
            {
                //------------------موجودی فعلی هر سطر با توجه به رکوردهای قبلی-------------------------------
                r.TotalQuantity = result.Where(a => a.Id <= r.Id && a.CommodityId == r.CommodityId && a.WarehouseLayoutId == r.WarehouseLayoutId).Sum(a => (a.Mode * a.Quantity));

            });
            stocksCommoditiesModel.ModifyQuantity = result.Where(a => a.TrasctionType == "اصلاح موجودی").Sum(a => a.Quantity * a.Mode);
            stocksCommoditiesModel.WarehouseLayoutsCommoditiesModel = result.Where(a => a.DocumentHeadId != null).ToList();

            stocksCommoditiesModel.FirstQuantity = await CalculateFirstQuantity(from, query);
            stocksCommoditiesModel.FirstQuantity += stocksCommoditiesModel.ModifyQuantity;


            stocksCommodities.Add(stocksCommoditiesModel);

            return new PagedList<StocksCommoditiesModel>()
            {
                Data = (IEnumerable<StocksCommoditiesModel>)stocksCommodities,
                TotalCount = query.PageIndex <= 1
                    ? await entities
                        .CountAsync()
                    : 0
            };
        }
        private async Task<double> CalculateFirstQuantity(DateTime? from, PaginatedQueryModel query)
        {
            var FirstQuantity = await QueryWarehouseHistory(query, from, null).SumAsync(a => a.Quantity * a.Mode);
            return Convert.ToDouble(FirstQuantity);
        }
        private IOrderedQueryable<WarehouseLayoutsCommoditiesModel> QueryWarehouseHistory(PaginatedQueryModel query, DateTime? from, DateTime? to)
        {
            var receiptItemsView = _context.ReceiptItemsView.Where(a => (a.DocumentDate >= from || from == null)
                                                                         && (a.DocumentDate <= to || to == null)
                                                                         && (a.DocumentStauseBaseValue != (int)DocumentStateEnam.archiveReceipt));

            var entities = (from layout in _context.WarehouseLayoutsCommoditiesView
                            join history in _context.WarehouseHistories.Where(a => !a.IsDeleted)
                            on layout.WarehouseLayoutId equals history.WarehouseLayoutId
                            //left join----------------------
                            join items in receiptItemsView on history.DocumentItemId equals items.DocumentItemId into ps

                            from items in ps.DefaultIfEmpty()

                            where layout.CommodityId == history.Commodityld

                            select new WarehouseLayoutsCommoditiesModel()
                            {
                                Id = history.Id,
                                WarehouseId = layout.WarehouseId,
                                CommodityId = history.Commodityld,
                                WarehouseLayoutId = layout.WarehouseLayoutId,
                                Quantity = Convert.ToInt32(history.Quantity),
                                CommodityCode = layout.CommodityCode,
                                CommodityTitle = layout.CommodityTitle,
                                CommodityCompactCode = layout.CommodityCompactCode,
                                CommodityTadbirCode = layout.CommodityTadbirCode,
                                Mode = history.Mode,
                                WarehouseTitle = layout.WarehouseTitle,
                                WarehouseLayoutTitle = layout.WarehouseLayoutTitle,
                                ModeTitle = history.Mode == (int)WarehouseHistoryMode.Enter ? "ورودی" : "خروجی",
                                CreatedDate = history.CreatedAt,
                                CreatedTime = history.CreatedAt.ToString("HH:mm:ss"),
                                DocumentDate = items.DocumentDate,
                                DocumentNo = items.DocumentNo,
                                RequestNo = items.RequestNo != null ? items.RequestNo : "",
                                DocumentHeadId = items.Id,
                                QuantityInput = history.Mode == (int)WarehouseHistoryMode.Enter ? Convert.ToInt32(history.Quantity) : 0,
                                QuantityOutput = history.Mode == (int)WarehouseHistoryMode.Exit ? Convert.ToInt32(history.Quantity) : 0,
                                TrasctionType = TractionType(items.CodeVoucherGroupsCode, items.RequestNo),
                                CodeVoucherGroupId = items.CodeVoucherGroupId
                            }
                          )
                        .FilterQuery(query.Conditions)
                        .OrderByMultipleColumns(query.OrderByProperty);
            //.OrderByDescending(a => a.DocumentHeadId).ThenByDescending(a => a.DocumentDate);
            return entities;
        }
        private static string TractionType(string CodeVoucherGroupsCode, string DocumentNo)
        {
            string tractionType1 = "";
            string tractionType2 = "";
            if (CodeVoucherGroupsCode == null)
            {
                return "اصلاح موجودی";
            }

            switch (CodeVoucherGroupsCode.Substring(0, 2))
            {
                case "50":
                    tractionType2 = "قطعات";
                    break;
                case "51":
                    tractionType2 = "مواد اولیه";
                    break;
                case "52":
                    tractionType2 = "اموال";
                    break;
                case "53":
                    tractionType2 = "مصرفی";
                    break;
                case "54":
                    tractionType2 = "امانی";
                    break;
                case "55":
                    tractionType2 = "مرجوعی";
                    break;
                default:
                    break;
            }

            switch (CodeVoucherGroupsCode.Substring(2, 2))
            {
                case "63":
                case "44":
                case "54":
                    tractionType1 = "خروج";
                    break;
                case "93":
                    tractionType1 = "انتقال";
                    break;
                case "94":
                    tractionType1 = "رسید محصول";
                    break;
                case "55":
                case "45":
                    tractionType1 = "موجودی اول دوره";
                    break;

                default:
                    if (!String.IsNullOrEmpty(DocumentNo) && tractionType2 != "امانی" && tractionType2 != "مرجوعی")
                    {
                        tractionType1 = "خرید";
                    }

                    break;
            }
            return tractionType1 + " " + tractionType2;
        }
        public async Task<PagedList<spGetWarehouseLayoutQuantities>> GetWarehouseLayoutQuantities(int? warehouseId, int? CommodityId, PaginatedQueryModel query)
        {
            var UserId = _currentUserAccessor.GetIp();
            var result = await _iProcedureCallService.GetWarehouseLayoutQuantities(warehouseId, CommodityId, Convert.ToInt32(UserId), query);
            return result;

        }
        /// <summary>
        /// Erp API
        /// </summary>
        /// <param name="Minutes"></param>
        /// <returns></returns>
        public async Task<PagedList<WarehouseLayoutsCommoditiesViewArani>> GetLastChangeWarehouseLayoutQuantities(double? Minutes)
        {

            await _iProcedureCallService.UpdateAllStockQuantity();
            var h = Minutes.HasValue ? Minutes.Value * (-1) : -5;
            
            var datetime = (Nullable<DateTime>)DateTime.Now.AddMinutes(h);

           

            var entities = _context.WarehouseLayoutsCommoditiesViewArani.Where(x => 
                                                                        ((x.ModifiedAt >= datetime || x.CommoditiesModifiedAt >= datetime || x.MeasureUnitsModifiedAt >= datetime))
                                                                    );            

            var result = await entities.ToListAsync();

            return new PagedList<WarehouseLayoutsCommoditiesViewArani>()
            {
                Data = (IEnumerable<WarehouseLayoutsCommoditiesViewArani>)result,
                TotalCount = result.Count()

            };
        }
        private List<int> AccessWarehouse()
        {
            return _context.AccessToWarehouse.Where(a => a.TableName == ConstantValues.AccessToWarehouseEnam.Warehouses && a.UserId == _currentUserAccessor.GetId() && !a.IsDeleted).Select(a => a.WarehouseId).ToList();
        }
        
    }
}
