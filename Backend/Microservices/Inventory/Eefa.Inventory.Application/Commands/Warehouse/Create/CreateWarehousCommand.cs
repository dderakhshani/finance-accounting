using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using Eefa.Common.Data;
using Eefa.Inventory.Domain;
using Eefa.Inventory.Domain.Common;
using MediatR;

namespace Eefa.Inventory.Application
{
    public class CreateWarehouseCommand : CommandBase, IRequest<ServiceResult<WarehouseModel>>, IMapFrom<CreateWarehouseCommand>, ICommand
    {
        public int? ParentId { get; set; }
        /// <description>
        /// کد سطح
        ///</description>

        public string? LevelCode { get; set; }
        /// <description>
        /// عنوان
        ///</description>

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
            profile.CreateMap<CreateWarehouseCommand, Domain.Warehouse>()
                .IgnoreAllNonExisting();
        }
    }

    public class CreateWarehouseCommandHandler : IRequestHandler<CreateWarehouseCommand, ServiceResult<WarehouseModel>>
    {
        private readonly IMapper _mapper;
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly IRepository<WarehousesCodeVoucherGroups> _warehousesCodeVoucherGroupsRepository;
        private readonly IRepository<WarehousesCategories> _warehousesCategoriesRepository;
        public CreateWarehouseCommandHandler(
            IMapper mapper,
            IWarehouseRepository warehouseRepository,
            IRepository<WarehousesCodeVoucherGroups> warehousesCodeVoucherGroupsRepository,
            IRepository<WarehousesCategories> warehousesCategoriesRepository

            )
        {
            _mapper = mapper;
            _warehouseRepository = warehouseRepository;
            _warehousesCodeVoucherGroupsRepository = warehousesCodeVoucherGroupsRepository;
            _warehousesCategoriesRepository = warehousesCategoriesRepository;
        }

        public async Task<ServiceResult<WarehouseModel>> Handle(CreateWarehouseCommand request, CancellationToken cancellationToken)
        {
            Domain.Warehouse warehouse;
            warehouse = _mapper.Map<Domain.Warehouse>(request);


            var entity = _warehouseRepository.Insert(warehouse);

            if (await _warehouseRepository.SaveChangesAsync(cancellationToken) > 0)
            {

                request.ReceiptAllStatus.ForEach(a =>
                {
                    _warehousesCodeVoucherGroupsRepository.Insert(new WarehousesCodeVoucherGroups() { WarehouseId = entity.Id, CodeVoucherGroupId = a.Id });
                });
                request.CommodityCategories.ForEach(a =>
                {
                    _warehousesCategoriesRepository.Insert(new WarehousesCategories() { WarehouseId = entity.Id, CommodityCategoryId = a.Id });
                });

                await _warehousesCodeVoucherGroupsRepository.SaveChangesAsync();
                await _warehousesCategoriesRepository.SaveChangesAsync();




                var model = _mapper.Map<WarehouseModel>(entity);
                return ServiceResult<WarehouseModel>.Success(model);
            }

            return ServiceResult<WarehouseModel>.Failed();

        }
        //private async Task<AccountReference> AddNewAccountReference(string Name, string Code)
        //{
        //    try
        //    {
        //        AccountReference AccountReference = new AccountReference();
        //        AccountReference.Code = Code;
        //        AccountReference.Title = Name;
        //        AccountReference.IsActive = true;

        //        _accountReferenceRepository.Insert(AccountReference);
        //        if (await _accountReferenceRepository.SaveChangesAsync() > 0)
        //        {
        //            return AccountReference;
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //    catch(Exception ex)
        //    {

        //    }
        //    return null;

        //}

        //private async Task<int> AddNewAccountRelReferencesGroup(int AccountReferenceId)
        //{
        //    AccountReferencesRelReferencesGroup accountReferencesRelReferencesGroup = new AccountReferencesRelReferencesGroup();
        //    accountReferencesRelReferencesGroup.ReferenceId = AccountReferenceId;
        //    accountReferencesRelReferencesGroup.ReferenceGroupId = ConstantValues.AccountReferenceGroup.WarehouseAccountReference;
        //    _accountReferenceRelReferencesRepository.Insert(accountReferencesRelReferencesGroup);
        //    return await _accountReferenceRelReferencesRepository.SaveChangesAsync();

        //}
    }


}
