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
using Eefa.Inventory.Domain.Aggregates.WarehouseAggregate;
using Eefa.Inventory.Domain.Enum;
using Eefa.NotificationServices.Services.Interfaces;
using Eefa.NotificationServices.Dto;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Inventory.Application
{
    public class UpdateStateWarehousCountFormCommand : CommandBase, IRequest<ServiceResult<WarehouseCountFormHeadModel>>, IMapFrom<WarehouseCountFormHead>, ICommand
    {
        public int Id { get; set; }
        public WarehouseStateForm warehouseStateForm { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateStateWarehousCountFormCommand, WarehouseCountFormHead>()
                .IgnoreAllNonExisting();
        }
    }
    public class UpdateWarehouseCountFormCommandHandler : IRequestHandler<UpdateStateWarehousCountFormCommand, ServiceResult<WarehouseCountFormHeadModel>>
    {
        private readonly IMapper _mapper;
        private readonly IRepository<WarehouseCountFormHead> _warehouseCountFormHeadRepository;
        private readonly INotificationClient _notificationClient;
        private readonly IInvertoryUnitOfWork _invertoryUnitOfWork;
        public UpdateWarehouseCountFormCommandHandler(IRepository<WarehouseCountFormHead> warehouseCountFormHeadRepository,
            IMapper mapper,
            INotificationClient notificationClient,
            IInvertoryUnitOfWork invertorUnitOfWork)
        {
            _mapper = mapper;
            _warehouseCountFormHeadRepository = warehouseCountFormHeadRepository;
            _notificationClient = notificationClient;
            _invertoryUnitOfWork = invertorUnitOfWork;
        }
        public async Task<ServiceResult<WarehouseCountFormHeadModel>> Handle(UpdateStateWarehousCountFormCommand request, CancellationToken cancellationToken)
        {
            if (request.warehouseStateForm == WarehouseStateForm.CountCancellation)
                await CountCancellation(request.Id);
            else
            {
                var entity = await _warehouseCountFormHeadRepository.Find(request.Id);
                //_mapper.Map<UpdateStateWarehousCountFormCommand, WarehouseCountFormHead>(request, entity);

                if (entity == null)
                    return ServiceResult<WarehouseCountFormHeadModel>.Failed();

                entity.FormState = request.warehouseStateForm;
                _warehouseCountFormHeadRepository.Update(entity);

                if (entity.ParentId.HasValue && entity.FormState == WarehouseStateForm.IssuingDeductionAndExtraForm)
                {
                    await UnLockWarehouse(entity.ParentId.Value);
                }
                else if  (entity.FormState == WarehouseStateForm.ConfrimCountingWithoutConflict ||
                      entity.FormState == WarehouseStateForm.ConfirmCountingAndIssuanceCountingConflicts ||
                      entity.FormState == WarehouseStateForm.IssuingDeductionAndExtraForm)
                {
                    await UnLockWarehouse(entity.Id);
                }
            }

            await _warehouseCountFormHeadRepository.SaveChangesAsync(cancellationToken);

            //var model = _mapper.Map<WarehouseCountFormHeadModel>(entity);
         
            return ServiceResult<WarehouseCountFormHeadModel>.Success(null);

        }
        private async Task CountCancellation(int headerId)
        {
            var entities = await _invertoryUnitOfWork.WarehouseCountFormHead.Where(x => x.Id == headerId || x.ParentId.Value == headerId).ToListAsync();
            foreach (var entity in entities)
            {
                entity.FormState = WarehouseStateForm.CountCancellation;
                _warehouseCountFormHeadRepository.Update(entity);
                await UnLockWarehouse(entity.Id);
            }

        }
        private void SendNotification(WarehouseCountFormHeadModel model, string payload)
        {

            var message = new NotificationDto
            {
                Listener = "notifyInventoryReceipt",
                MessageContent = "شمارش انبار " + model.WarehouseLayoutTitle + "تمام شده و در انتظار تایید است",
                MessageTitle = "اتمام شمارش انبار ",
                MessageType = 1,
                SendForAllUser = false,
                ReceiverUserId = model.ConfirmerUserId,
                Status = 0,
                Payload = payload
            };
            _notificationClient.Send(message);
        }
        private async Task UnLockWarehouse(int headerId)
        {
            var layoutStatuses = await (from w in _invertoryUnitOfWork.WarehouseCountFormDetail
                                        join q in _invertoryUnitOfWork.WarehouseLayoutQuantities on w.WarehouseLayoutQuantitiesId equals q.Id
                                        join wl in _invertoryUnitOfWork.WarehouseLayouts on q.WarehouseLayoutId equals wl.Id
                                        where w.WarehouseCountFormHeadId == headerId
                                        select new
                                        {
                                            Id = wl.Id,
                                            Status = w.LastWarehouseLayoutStatus
                                        }).ToListAsync();


            var layoutIds = layoutStatuses.Select(l => l.Id).Distinct().ToList();


            var layouts = await _invertoryUnitOfWork.WarehouseLayouts
                                .Where(wl => layoutIds.Contains(wl.Id))
                                .ToListAsync();


            foreach (var layout in layouts)
            {
                var status = layoutStatuses.FirstOrDefault(ls => ls.Id == layout.Id)?.Status;
                if (status != null)
                {
                    layout.Status = status;
                }
                _invertoryUnitOfWork.WarehouseLayouts.Update(layout);
            }
        }
    }
}
