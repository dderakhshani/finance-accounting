using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using Eefa.Common.Data;
using Eefa.Inventory.Domain;
using MediatR;

namespace Eefa.Inventory.Application
{
    public class UpdateWarehouseLayoutCommand : CommandBase, IRequest<ServiceResult<WarehouseLayoutGetIdModel>>, IMapFrom<Domain.WarehouseLayout>, ICommand
    {
        public int Id { get; set; }


        public int? ParentId { get; set; }

        /// <description>
        /// عنوان
        ///</description>

        public string Title { get; set; } = default!;
        /// <description>
        /// ظرفیت
        ///</description>

        public int Capacity { get; set; } = default!;
        /// <description>
        /// شماره شروع
        ///</description>
        public int? Status { get; set; }
        public int? EntryMode { get; set; }
        public bool LastLevel { get; set; } = default!;
        public bool IsDefault { get; set; } = default!;
        public int OrderIndex { get; set; } = default!;


        public List<WarhousteCategoryUpdateItems> Items { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateWarehouseLayoutCommand, Domain.WarehouseLayout>()
                .IgnoreAllNonExisting();
        }
    }
    public class WarhousteCategoryUpdateItems : WarhousteCategoryItemsModel
    {

    }

    public class UpdateWarehouseLayoutCommandHandler : IRequestHandler<UpdateWarehouseLayoutCommand, ServiceResult<WarehouseLayoutGetIdModel>>
    {

        private readonly IWarehouseLayoutRepository _Repository;
        private readonly IRepository<WarehouseLayoutCategories> _RepositoryCategorys;
        private readonly IRepository<Domain.WarehouseLayoutProperty> _RepositoryProperty;
        private readonly IWarehouseLayoutQueries _warehouseQueries;
        private readonly IMapper _mapper;

        public UpdateWarehouseLayoutCommandHandler(
            IWarehouseLayoutRepository warehouseLayoutRepository,
            IMapper mapper,
            IRepository<WarehouseLayoutCategories> RepositoryCategorys,
            IRepository<Domain.WarehouseLayoutProperty> RepositoryProperty,
            IWarehouseLayoutQueries warehouseQueries
            )
        {
            _mapper = mapper;
            _Repository = warehouseLayoutRepository;
            _RepositoryCategorys = RepositoryCategorys;
            _RepositoryProperty = RepositoryProperty;
            _warehouseQueries = warehouseQueries;
        }

        public async Task<ServiceResult<WarehouseLayoutGetIdModel>> Handle(UpdateWarehouseLayoutCommand request, CancellationToken cancellationToken)
        {
            var entity = await _Repository.Find(request.Id);

            _mapper.Map<UpdateWarehouseLayoutCommand, Domain.WarehouseLayout>(request, entity);

            _Repository.Update(entity);
            if (await _Repository.SaveChangesAsync() > 0)
            {


                //-------------------ویرایش قبلی ها-----------------------------------------------
                //یافتن فرزندانی که از این گروه محصولات استفاده می کردند--------------------------
                var warehouseLayout = _Repository.GetAll().Where(a => !a.IsDeleted &&
                                                                          (entity.LevelCode == null ||
                                                                          (a.LevelCode.Length >= entity.LevelCode.Length &&
                                                                          (a.LevelCode.Substring(0, entity.LevelCode.Length) == entity.LevelCode)))).ToList();


                var _Categories = _RepositoryCategorys.GetAll().ToList().Where(x => warehouseLayout.Any(par => x.WarehouseLayoutId == par.Id)).ToList();
                var _Property = _RepositoryProperty.GetAll().ToList().Where(x => _Categories.Any(par => x.WarehouseLayoutCategoryId == par.Id)).ToList();

                foreach (var item in request.Items.Where(a => a.WarehouseLayoutCategoriesId > 0))
                {
                    var Category = _Categories.Where(a => a.Id == item.WarehouseLayoutCategoriesId).FirstOrDefault();
                    Category.CategoryId = Convert.ToInt32(item.CommodityCategoryId);
                }
                foreach (var item in request.Items.Where(a => a.WarehouseLayoutCategoriesId > 0 && a.WarehouseLayoutPropertiesId > 0))
                {
                    var Property = _Property.Where(a => a.Id == item.WarehouseLayoutPropertiesId).FirstOrDefault();
                    Property.CategoryPropertyItemId = item.CategoryPropertyItemId;
                    Property.Value = item.ValueItem;
                }
                //--------------------------افزودن جدید------------------------------------------
                foreach (var item in request.Items.Where(a => a.WarehouseLayoutCategoriesId == null || a.WarehouseLayoutCategoriesId == 0))
                {
                    if (item.CommodityCategoryId != null)
                    {
                        WarehouseLayoutCategories Category = new WarehouseLayoutCategories();
                        Category.WarehouseLayoutId = request.Id;
                        Category.CategoryId = Convert.ToInt32(item.CommodityCategoryId);
                        _RepositoryCategorys.Insert(Category);
                        await _RepositoryCategorys.SaveChangesAsync();
                        if (item.CategoryPropertyId > 0)
                        {
                            Domain.WarehouseLayoutProperty Property = new Domain.WarehouseLayoutProperty();
                            Property.CategoryPropertyId = Convert.ToInt32(item.CategoryPropertyId);
                            Property.CategoryPropertyItemId = item.CategoryPropertyItemId;
                            Property.WarehouseLayoutId = request.Id;
                            Property.WarehouseLayoutCategoryId = Category.Id;

                            _RepositoryProperty.Insert(Property);

                        }
                    }
                }
                foreach (var item in request.Items.Where(a => a.WarehouseLayoutPropertiesId == null || a.WarehouseLayoutPropertiesId == 0))
                {

                    if (item.CategoryPropertyId > 0)
                    {
                        Domain.WarehouseLayoutProperty Property = new Domain.WarehouseLayoutProperty();
                        Property.CategoryPropertyId = Convert.ToInt32(item.CategoryPropertyId);
                        Property.CategoryPropertyItemId = item.CategoryPropertyItemId;
                        Property.WarehouseLayoutId = request.Id;
                        Property.WarehouseLayoutCategoryId = Convert.ToInt32(item.WarehouseLayoutCategoriesId);

                        _RepositoryProperty.Insert(Property);

                    }

                }
                //-----------------------------------------------------------------------------

                await _RepositoryProperty.SaveChangesAsync();



            }

            var model = await _warehouseQueries.GetById(request.Id);
            return ServiceResult<WarehouseLayoutGetIdModel>.Success(model);
        }
    }
}
