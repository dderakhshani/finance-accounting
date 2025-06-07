using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using Eefa.Inventory.Domain.Aggregates.WarehouseAggregate;
using Eefa.Inventory.Domain.Enum;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Eefa.Common.Data;
using Eefa.Inventory.Domain;
using System.Linq;
using NetTopologySuite.Noding;
using Eefa.Common.Data.Query;
using Microsoft.EntityFrameworkCore;
using Eefa.Common.Validation.Resources;
using Microsoft.AspNetCore.Server.IIS.Core;
using Eefa.Common.Exceptions;
using EFCore.BulkExtensions;
using System.Collections.Immutable;

namespace Eefa.Inventory.Application
{
    public class CreateWarehouseCountFormCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<CreateWarehouseCountFormCommand>, ICommand
    {
        public int? ParentId { get; set; }
        public int FormNo { get; set; }
        public DateTime FormDate { get; set; }
        public int CounterUserId { get; set; }
        public int ConfirmerUserId { get; set; }
        public string Description { get; set; }
        public int WarehouseLayoutId { get; set; }
        public int WarehouseId { get; set; }
        public bool BasedOnLocation { get; set; }
        public WarehouseStateForm WarehouseStateForm { get; set; }
        public List<Details> CountFormSelectedCommodity { get; set; }
        public class Details
        {
            public int WarehouseLayoutId { get; set; }
            public int WarehouseLayoutQuantityId { get; set; }
        }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateWarehouseCountFormCommand, WarehouseCountFormHead>()
                .IgnoreAllNonExisting();
        }
        public class CreateWarehouseCountFormCommandHandler : IRequestHandler<CreateWarehouseCountFormCommand, ServiceResult>
        {
            private readonly IMapper _mapper;
            private readonly IRepository<WarehouseCountFormHead> _repository;
            private readonly IWarehouseLayoutQueries _warehouseLayoutQueries;
            private readonly IInvertoryUnitOfWork _invertoryUnitOfWork;
            private readonly IProcedureCallService _procedureCallService;

            public CreateWarehouseCountFormCommandHandler(
                IMapper mapper,
                IRepository<WarehouseCountFormHead> repository,
                IWarehouseLayoutQueries warehouseLayoutQueries,
                IInvertoryUnitOfWork invertorUnitOfWork,
                IProcedureCallService procedureCallService
                )
            {
                _mapper = mapper;
                _repository = repository;
                _warehouseLayoutQueries = warehouseLayoutQueries;
                _invertoryUnitOfWork = invertorUnitOfWork;
                _procedureCallService = procedureCallService;
            }

            public async Task<ServiceResult> Handle(CreateWarehouseCountFormCommand request, CancellationToken cancellationToken)
            {
                using (var transaction = await _invertoryUnitOfWork.BeginTransactionAsync(cancellationToken))
                {
                    try
                    {
                        var warehouseCountForm = new List<WarehouseCountFormHead>();
                        if (request.BasedOnLocation)
                            warehouseCountForm = _invertoryUnitOfWork.WarehouseCountFormHead
                               .Where(x => x.WarehouseLayoutId == request.WarehouseLayoutId && x.IsDeleted == false && x.FormState != WarehouseStateForm.CountCancellation).ToList();
                        else
                            warehouseCountForm = _invertoryUnitOfWork.WarehouseCountFormHead
                               .Where(x => x.WarehouseId == request.WarehouseId && x.IsDeleted == false && x.FormState != WarehouseStateForm.CountCancellation).ToList();


                        var LastWarehouseCountForm = new WarehouseCountFormHead();

                        if (warehouseCountForm.Count() != 0)
                            LastWarehouseCountForm = warehouseCountForm.OrderBy(x => x.Id).Last();
                        else
                            LastWarehouseCountForm = null;

                        if (!IsAllowedCreateNewForm(LastWarehouseCountForm))
                            throw new ValidationError("امکان ثبت فرم جدید وجود ندارد. لطفا وضعیت فرم قبلی را مشخص کنید.");

                        var entity = _mapper.Map<WarehouseCountFormHead>(request);                        
                        var details = new List<WarehouseCountFormDetail>();
                        entity.FormNo = CreateFormId();

                        if (LastWarehouseCountForm == null ||
                            LastWarehouseCountForm.FormState == WarehouseStateForm.IssuingDeductionAndExtraForm ||
                            LastWarehouseCountForm.FormState == WarehouseStateForm.CountCancellation ||
                            LastWarehouseCountForm.FormState == WarehouseStateForm.ConfrimCountingWithoutConflict )
                        {
                            entity.FormState = WarehouseStateForm.IssuanceOfCountingForm;
                            entity.ParentId = null;

                            if (request.WarehouseStateForm == WarehouseStateForm.IssuanceOfCountingForm && request.CountFormSelectedCommodity?.Count > 0)
                                details = await GetSelectedDetail(request.CountFormSelectedCommodity);
                            else
                                details = GetAllDetail(request.WarehouseLayoutId);                                                                                                

                        }
                        else if(LastWarehouseCountForm.FormState == WarehouseStateForm.ConfirmCountingAndIssuanceCountingConflicts ||
                                LastWarehouseCountForm.FormState == WarehouseStateForm.CompleteCounting)
                        {                                    
                            entity.ParentId = LastWarehouseCountForm?.ParentId == null ? LastWarehouseCountForm.Id : LastWarehouseCountForm.ParentId;
                            details = await GetSelectedDetail(request.CountFormSelectedCommodity);
                            var parentForm = warehouseCountForm.FirstOrDefault(x => !x.ParentId.HasValue);
                            parentForm.FormState = WarehouseStateForm.ConfirmCountingAndIssuanceCountingConflicts;
                            _invertoryUnitOfWork.WarehouseCountFormHead.Update(parentForm);
                        }
                      
                        entity.WarehouseCountFormDetails = null;
                        await _invertoryUnitOfWork.WarehouseCountFormHead.AddAsync(entity);
                        await _invertoryUnitOfWork.SaveChangesAsync(cancellationToken);

                        foreach (var d in details)
                        {
                            d.WarehouseCountFormHeadId = entity.Id;
                        }
                       
                        await _invertoryUnitOfWork.DbContex().BulkInsertAsync(details, new BulkConfig
                        {
                            PreserveInsertOrder = true,
                            SetOutputIdentity = false,
                            BatchSize = 5000
                        });
                        await _invertoryUnitOfWork.SaveChangesAsync(cancellationToken);

                        await transaction.CommitAsync();
                        return ServiceResult.Success();
                    }

                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        throw new ValidationError(ex.Message);
                    }
                }

            }
            private int CreateFormId()
            {
                var formCount = _invertoryUnitOfWork.WarehouseCountFormHead.Count() + 1;
                var dt = DateTime.Now;
                System.Globalization.PersianCalendar date = new System.Globalization.PersianCalendar();
                string dayOfYear = string.Empty;
                if (date.GetDayOfYear(dt).ToString().Length == 1)
                    dayOfYear = "0" + (date.GetDayOfYear(dt)).ToString();
                else if (date.GetDayOfYear(dt).ToString().Length == 2)
                    dayOfYear = "00" + (date.GetDayOfYear(dt)).ToString();
                else
                    dayOfYear = date.GetDayOfYear(dt).ToString();

                var formId = date.GetYear(dt).ToString() + dayOfYear + formCount.ToString();
                return Convert.ToInt32(formId);
            }
            private void LockWarehouse(List<int> warehouseLayoutIds)
            {
                var warehouseLayouts = _invertoryUnitOfWork.WarehouseLayouts.Where(x => warehouseLayoutIds.Contains(x.Id) && x.Title != "Defualt");
                foreach (var layout in warehouseLayouts)
                {
                    layout.Status = WarehouseLayoutStatus.TemporaryLock;

                }
                _invertoryUnitOfWork.WarehouseLayouts.BatchUpdateAsync(warehouseLayouts);
            }
            private bool IsAllowedCreateNewForm(WarehouseCountFormHead warehouseCountFormHead)
            {
                if (warehouseCountFormHead == null || warehouseCountFormHead.FormState!= WarehouseStateForm.IssuanceOfCountingForm &&
                    warehouseCountFormHead.FormState != WarehouseStateForm.CompleteCounting)
                    return true;
                //if (warehouseCountFormHead.FormState == WarehouseStateForm.CompleteCounting ||
                //    warehouseCountFormHead.FormState == WarehouseStateForm.ConfrimCountingWithoutConflict ||
                //    warehouseCountFormHead.FormState == WarehouseStateForm.ConfirmCountingAndIssuanceCountingConflicts ||
                //    warehouseCountFormHead.FormState == WarehouseStateForm.CountCancellation  
                //    )
                //    return true;
                else
                return false;
            }
            private List<WarehouseCountFormDetail> GetAllDetail(int WarehouselayoutId)
            {
                var warehouseLayouts = _invertoryUnitOfWork.GetWarehouseLayoutRecursive(WarehouselayoutId);
                var warehouseLayoutIds = warehouseLayouts.Select(x => x.WarehouseLayoutId).ToList();
                warehouseLayoutIds.Add(WarehouselayoutId);
                LockWarehouse(warehouseLayoutIds);

                var details = (from wl in warehouseLayouts
                               join lq in _invertoryUnitOfWork.WarehouseLayoutQuantities on wl.WarehouseLayoutId equals lq.WarehouseLayoutId
                               select new WarehouseCountFormDetail
                               {
                                   CountedQuantity = null,
                                   WarehouseLayoutQuantitiesId = lq.Id,
                                   LastWarehouseLayoutStatus = wl.Status,
                                   SystemQuantity = lq.Quantity
                               }).ToList();
                return details;
            }
            private async Task<List<WarehouseCountFormDetail>> GetSelectedDetail(List<Details> selectedCommodities)
            {
                var warehouseLayoutIds = selectedCommodities.Select(x => x.WarehouseLayoutId).ToList();
                var warehouseLauoutQuantityIds = selectedCommodities.Select(x => x.WarehouseLayoutQuantityId).ToList();
                var warehousLayouts = await _invertoryUnitOfWork.WarehouseLayouts.Where(x => warehouseLayoutIds.Contains(x.Id)).ToListAsync();
                var warehousLayoutQuantities = await _invertoryUnitOfWork.WarehouseLayoutQuantities.Where(x => warehouseLauoutQuantityIds.Contains(x.Id)).AsNoTracking().ToListAsync();
                LockWarehouse(warehouseLayoutIds);
                try
                {
                    var details = (from wl in warehousLayouts
                                   join lq in warehousLayoutQuantities on wl.Id equals lq.WarehouseLayoutId
                                   select new WarehouseCountFormDetail
                                   {
                                       CountedQuantity = null,
                                       WarehouseLayoutQuantitiesId = lq.Id,
                                       LastWarehouseLayoutStatus = (WarehouseLayoutStatus)wl.Status,
                                       SystemQuantity = lq.Quantity
                                   }).ToList();

                    return details;
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
            private async Task<List<WarehouseCountFormDetail>> GetAllDiscrepancies(int WarehouselayoutId, int formId)
            {
                var warehouseLayouts = _invertoryUnitOfWork.GetWarehouseLayoutRecursive(WarehouselayoutId);
                var warehouseLayoutIds = warehouseLayouts.Select(x => x.WarehouseLayoutId).ToList();
                warehouseLayoutIds.Add(WarehouselayoutId);
                LockWarehouse(warehouseLayoutIds);
                var details = await (from wl in warehouseLayouts
                                     join lq in _invertoryUnitOfWork.WarehouseLayoutQuantities on wl.WarehouseLayoutId equals lq.WarehouseLayoutId
                                     join wcd in _invertoryUnitOfWork.WarehouseCountFormDetail on lq.Id equals wcd.WarehouseLayoutQuantitiesId into dlq
                                     from wlcd in dlq.DefaultIfEmpty()
                                     where ((!wlcd.CountedQuantity.HasValue) || (wlcd.CountedQuantity.Value != lq.Quantity))
                                     && wlcd.WarehouseCountFormHeadId == formId
                                     select new WarehouseCountFormDetail
                                     {
                                         CountedQuantity = null,
                                         WarehouseLayoutQuantitiesId = lq.Id,
                                         LastWarehouseLayoutStatus = wl.Status,
                                         SystemQuantity = lq.Quantity
                                     }).ToListAsync();
                return details;
            }
        }
    }
}
