using System;
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
using Microsoft.EntityFrameworkCore;
using Eefa.Common.Exceptions;
using System.Collections.Generic;

namespace Eefa.Inventory.Application
{


    public class UpdateNewPersonsDebitedCommand : CommandBase, IRequest<ServiceResult<PersonsDebitedCommoditiesModel>>, IMapFrom<Domain.PersonsDebitedCommodities>, ICommand
    {
        public int Id { get; set; } = default!;
        public int debitAccountReferenceId { get; set; } = default!;
        public int debitAccountReferenceGroupId { get; set; } = default!;
        public string Description { get; set; } = default!;
        public DateTime CreateDate { get; set; } = default!;
       
        public List<AttachmentAssetsRequest> attachmentAssets { get; set; } = default!;


        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateNewPersonsDebitedCommand, Domain.PersonsDebitedCommodities>()
                .IgnoreAllNonExisting();
        }
    }


    public class UpdateNewPersonsDebitedCommandHandler : IRequestHandler<UpdateNewPersonsDebitedCommand, ServiceResult<PersonsDebitedCommoditiesModel>>
    {

        private readonly IMapper _mapper;
        private readonly IInvertoryUnitOfWork _context;
        private readonly IReceiptCommandsService _receiptCommandsService;
        private readonly IRepository<Domain.AssetAttachments> _repositoryAssetAttachments;
        private readonly IPersonsDebitedCommoditiesRepository _PersonsDebitedCommoditiesRepository;

        public UpdateNewPersonsDebitedCommandHandler(
            IMapper mapper,
            IInvertoryUnitOfWork context,
            IReceiptCommandsService receiptCommandsService,
            IRepository<Domain.AssetAttachments> repositoryAssetAttachments,
            IPersonsDebitedCommoditiesRepository PersonsDebitedCommoditiesRepository

            )
        {
            _mapper = mapper;
            _context = context;
            _receiptCommandsService = receiptCommandsService;
            _repositoryAssetAttachments = repositoryAssetAttachments;
            _PersonsDebitedCommoditiesRepository = PersonsDebitedCommoditiesRepository;

        }

        public async Task<ServiceResult<PersonsDebitedCommoditiesModel>> Handle(UpdateNewPersonsDebitedCommand request, CancellationToken cancellationToken)
        {
            var year = await _context.Years.Where(a => a.IsCurrentYear).FirstOrDefaultAsync();
            if (year == null)
            {
                throw new ValidationError("سال مالی تنظیم نشده است");
            }

            var entity = await _PersonsDebitedCommoditiesRepository.Find(request.Id);

            var newEntity = await InsertCopyRow(request, year, entity);

            entity.ExpierDate = request.CreateDate;
            entity.IsActive = false;
            _PersonsDebitedCommoditiesRepository.Update(entity);
            await _PersonsDebitedCommoditiesRepository.SaveChangesAsync(cancellationToken);
            await _receiptCommandsService.ModifyAttachmentAssets(request.attachmentAssets, newEntity.Id);

            var model = _mapper.Map<PersonsDebitedCommoditiesModel>(entity);
            return ServiceResult<PersonsDebitedCommoditiesModel>.Success(model);
        }
        //انتقال به کاربر دیگری
        private async Task<PersonsDebitedCommodities> InsertCopyRow(UpdateNewPersonsDebitedCommand request, Year year, PersonsDebitedCommodities entity)
        {
            Domain.PersonsDebitedCommodities newEntity = new PersonsDebitedCommodities()
            {
                DebitAccountReferenceId = request.debitAccountReferenceId,
                DebitAccountReferenceGroupId = request.debitAccountReferenceGroupId,
                PersonId= entity.PersonId,
                
                IsActive = true,
                DocumentDate = request.CreateDate,
                WarehouseId = entity.WarehouseId,
                CommodityId = entity.CommodityId,
                AssetId = entity.AssetId,
                UnitId = entity.UnitId,
                CommoditySerial = entity.CommoditySerial,
                DebitTypeId = entity.DebitTypeId,
                Quantity = entity.Quantity,
                DocumentItemId = entity.DocumentItemId,
                MeasureId = entity.MeasureId,
                ExpierDate = year.LastDate,
                Description = request.Description,
                
            };


            _PersonsDebitedCommoditiesRepository.Insert(newEntity);
            return newEntity;



        }
    }
}
