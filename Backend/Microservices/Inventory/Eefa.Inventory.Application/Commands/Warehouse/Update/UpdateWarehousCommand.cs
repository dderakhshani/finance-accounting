using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using Eefa.Common.Data;
using Eefa.Inventory.Domain;
using MediatR;
using System.Linq;
namespace Eefa.Inventory.Application
{
    public class UpdateWarehouseCommand : CommandBase, IRequest<ServiceResult<WarehouseModel>>, IMapFrom<Domain.Warehouse>, ICommand
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        /// <description>
        /// کد سطح
        ///</description>

        //public string? LevelCode { get; set; }
        ///// <description>
        ///// عنوان
        /////</description>

        public string Title { get; set; } = default!;
        /// <description>
        /// فعال
        ///</description>

        public bool IsActive { get; set; } = default!;
        /// <description>
        /// کد گروه کالا
        ///</description>

        public int? CommodityCategoryId { get; set; } = default!;
        public int? AccountHeadId { get; set; }
        public int? Sort { get; set; }
        public List<CommodityCategoryModel> CommodityCategories { get; set; } = default!;
        public List<ReceiptALLStatusModel> ReceiptAllStatus { get; set; } = default!;
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateWarehouseCommand, Domain.Warehouse>()
                .IgnoreAllNonExisting();
        }
    }


    public class UpdateWarehouseCommandHandler : IRequestHandler<UpdateWarehouseCommand, ServiceResult<WarehouseModel>>
    {
        private readonly IWarehouseRepository _warehousRepository;
        private readonly IMapper _mapper;
        private readonly IRepository<AccountReference> _accountReferenceRepository;
        private readonly IRepository<WarehousesCodeVoucherGroups> _warehousesCodeVoucherGroupsRepository;
        private readonly IRepository<WarehousesCategories> _warehousesCategoriesRepository;

        public UpdateWarehouseCommandHandler(IWarehouseRepository warehousRepository,
            IMapper mapper,
            IRepository<AccountReference> accountReferenceRepository,
            IRepository<WarehousesCodeVoucherGroups> warehousesCodeVoucherGroupsRepository,
            IRepository<WarehousesCategories> warehousesCategoriesRepository
            )
        {
            _mapper = mapper;
            _warehousRepository = warehousRepository;
            _accountReferenceRepository = accountReferenceRepository;
            _warehousesCodeVoucherGroupsRepository = warehousesCodeVoucherGroupsRepository;
            _warehousesCategoriesRepository = warehousesCategoriesRepository;
        }

        public async Task<ServiceResult<WarehouseModel>> Handle(UpdateWarehouseCommand request, CancellationToken cancellationToken)
        {

            var entity = await _warehousRepository.Find(request.Id);

            _warehousesCodeVoucherGroupsRepository.GetAll().Where(a => a.WarehouseId == request.Id).ToList().ForEach(a =>
            {
                _warehousesCodeVoucherGroupsRepository.Delete(a);
            });


            _warehousesCategoriesRepository.GetAll().Where(a => a.WarehouseId == request.Id).ToList().ForEach(a =>
            {
                _warehousesCategoriesRepository.Delete(a);
            });



            request.ReceiptAllStatus.ForEach(a =>
            {
                _warehousesCodeVoucherGroupsRepository.Insert(new WarehousesCodeVoucherGroups() { WarehouseId = request.Id, CodeVoucherGroupId = a.Id });
            });


            request.CommodityCategories.ForEach(a =>
                {
                    _warehousesCategoriesRepository.Insert(new WarehousesCategories() { WarehouseId = request.Id, CommodityCategoryId = a.Id });
                });

            await _warehousesCodeVoucherGroupsRepository.SaveChangesAsync();
            await _warehousesCategoriesRepository.SaveChangesAsync();

            _mapper.Map<UpdateWarehouseCommand, Domain.Warehouse>(request, entity);


            _warehousRepository.Update(entity);

            await _warehousRepository.SaveChangesAsync(cancellationToken);

            var model = _mapper.Map<WarehouseModel>(entity);
            return ServiceResult<WarehouseModel>.Success(model);
        }
    }
}
