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
using Eefa.Inventory.Domain.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Inventory.Application
{
    public class CreateWarehouseLayoutCommand : CommandBase, IRequest<ServiceResult<PagedList<WarehouseLayoutModel>>>, IMapFrom<CreateWarehouseLayoutCommand>, ICommand
    {
        public int WarehouseId { get; set; }
        /// <description>
        /// کد والد
        ///</description>

        public int? ParentId { get; set; }
        /// <description>
        /// کد سطح
        ///</description>

        public string LevelCode { get; set; } = default!;
        /// <description>
        /// عنوان
        ///</description>

        public string Title { get; set; } = default!;
        /// <description>
        /// ظرفیت
        ///</description>

        public int ChildCount { get; set; } = default!;
        /// <description>
        /// شماره شروع
        ///</description>
        public bool LastLevel { get; set; } = default!;
        public bool IsDefault { get; set; } = default!;

        public int Capacity { get; set; } = default!;
        public int Status { get; set; }
        public int EntryMode { get; set; } = default!;
        public List<WarhousteCategoryItems> Items { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateWarehouseLayoutCommand, Domain.WarehouseLayout>().IgnoreAllNonExisting();

        }
    }
    public class WarhousteCategoryItems : WarhousteCategoryItemsModel
    {

    }

    public class CreateWarehouseLayoutCommandHandler : IRequestHandler<CreateWarehouseLayoutCommand, ServiceResult<PagedList<WarehouseLayoutModel>>>
    {
        private readonly IWarehouseLayoutRepository _Repository;
        private readonly IRepository<WarehouseLayoutCategories> _RepositoryCategories;
        private readonly IRepository<Domain.WarehouseLayoutProperty> _RepositoryProperty;

        private readonly IMapper _mapper;

        public CreateWarehouseLayoutCommandHandler(
            IWarehouseLayoutRepository warehouseLayoutRepository,
            IMapper mapper,
            IRepository<WarehouseLayoutCategories> RepositoryCategories,
            IRepository<Domain.WarehouseLayoutProperty> RepositoryProperty
            )
        {
            _mapper = mapper;
            _Repository = warehouseLayoutRepository;
            _RepositoryCategories = RepositoryCategories;
            _RepositoryProperty = RepositoryProperty;
        }


        public async Task<ServiceResult<PagedList<WarehouseLayoutModel>>> Handle(CreateWarehouseLayoutCommand request, CancellationToken cancellationToken)
        {

            List<Domain.WarehouseLayout> Layouts = new List<Domain.WarehouseLayout>();


            var childInfo = await _Repository.GetAll().Where(a => a.ParentId == request.ParentId).ToListAsync();
            var parentInfo = await _Repository.GetAll().Where(a => a.Id == request.ParentId).FirstOrDefaultAsync();
            int OrderIndex = 0;
            for (int i = 0; i < request.ChildCount; i++)
            {
                Domain.WarehouseLayout warehouseLayout = new Domain.WarehouseLayout();
                List<WarehouseLayoutCategories> Categories = new List<WarehouseLayoutCategories>();

                if (childInfo.Any())
                {
                    OrderIndex = Convert.ToInt32(childInfo.Max(o => o.OrderIndex));
                }
                warehouseLayout.WarehouseId = request.WarehouseId;
                warehouseLayout.OrderIndex = OrderIndex + (i + 1);
                warehouseLayout.ParentId = request.ParentId==0 ?null : request.ParentId;
                warehouseLayout.Title = request.Title;// + "-" + warehouseLayout.OrderIndex.ToString();
                warehouseLayout.EntryMode = request.EntryMode;
                warehouseLayout.LastLevel = request.LastLevel;
                warehouseLayout.IsDefault = request.IsDefault;
                warehouseLayout.Status = (WarehouseLayoutStatus)request.Status;
                warehouseLayout.Capacity = Convert.ToInt32(request.Capacity);
                if (parentInfo != null)
                {
                    parentInfo.LastLevel = false;
                }
                _Repository.Insert(warehouseLayout);
               
                    if (await _Repository.SaveChangesAsync() > 0)
                    {
                        foreach (var item in request.Items)
                        {
                            if (item.CommodityCategoryId != null)
                            {


                                if (Categories.Where(a => a.CategoryId == item.CommodityCategoryId).Count() == 0)
                                {
                                    WarehouseLayoutCategories Category = new WarehouseLayoutCategories();
                                    Category.WarehouseLayoutId = warehouseLayout.Id;
                                    Category.CategoryId = Convert.ToInt32(item.CommodityCategoryId);


                                    _RepositoryCategories.Insert(Category);
                                    await _RepositoryCategories.SaveChangesAsync();

                                    Categories.Add(Category);
                                }



                                if (item.CategoryPropertyId > 0)
                                {
                                    Domain.WarehouseLayoutProperty Property = new Domain.WarehouseLayoutProperty();
                                    Property.CategoryPropertyId = Convert.ToInt32(item.CategoryPropertyId);
                                    Property.CategoryPropertyItemId = item.CategoryPropertyItemId;
                                    Property.WarehouseLayoutId = warehouseLayout.Id;
                                    //به این دلیل اضافه شد که اگر قبلا گروه محصول آن در جدول گروه محصول ها خورده بود دوباره اضافه نشود
                                    Property.WarehouseLayoutCategoryId = Categories.FirstOrDefault(a => a.CategoryId == item.CommodityCategoryId).Id;
                                    _RepositoryProperty.Insert(Property);

                                }
                            }
                        }
                        await _RepositoryProperty.SaveChangesAsync();

                    }
                    //----------------------مدل خروجی----------------------
                    Layouts.Add(warehouseLayout);
                
               
            }


            IEnumerable<Domain.WarehouseLayout> _Models = Layouts.AsEnumerable<Domain.WarehouseLayout>();
            var list = _mapper.Map<IEnumerable<WarehouseLayoutModel>>(_Models);
            var result = new PagedList<WarehouseLayoutModel>()
            {
                Data = list,
                TotalCount = 0,
            };
            return ServiceResult<PagedList<WarehouseLayoutModel>>.Success(result);
        }


    }

}
