using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using Eefa.Common.Data;
using Eefa.Common.Exceptions;
using Eefa.Inventory.Application.Commands.Receipt.Update;
using Eefa.Inventory.Domain;
using Eefa.Inventory.Domain.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Eefa.Inventory.Application
{

    public class UpdateExteraCostCommand : CommandBase, IRequest<ServiceResult<ReceiptQueryModel>>, IMapFrom<Domain.Receipt>, ICommand
    {
        public int Id { get; set; }
        public bool? IsFreightChargePaid { get; set; } = default!;
        public long? ExtraCost { get; set; } = default!;
        public ICollection<ReceiptDocumentItemUpdate> ReceiptDocumentItems { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateExteraCostCommand, Domain.Receipt>()
                .IgnoreAllNonExisting();
        }

        public class UpdateExteraCostCommandHandler : IRequestHandler<UpdateExteraCostCommand, ServiceResult<ReceiptQueryModel>>
        {

            private readonly IMapper _mapper;
            private readonly IInvertoryUnitOfWork _context;
            private readonly IAssetsRepository _assetsRepository;
            private readonly IReceiptRepository _receiptRepository;
            private readonly ITemporaryReceipQueries _receiptQueries;
            private readonly IRepository<BaseValue> _baseValueRepository;
            private readonly IProcedureCallService _procedureCallService;
            private readonly IRepository<DocumentItem> _documentItemRepository;
            private readonly IReceiptCommandsService _receiptCommandsService;
            private readonly IRepository<DocumentHeadExtend> _documentHeadExtendRepository;
            public UpdateExteraCostCommandHandler(IReceiptRepository receiptRepository
              , IMapper mapper
              , IInvertoryUnitOfWork context
              , IAssetsRepository assetsRepository
              , IRepository<DocumentItem> DocumentItem
              , ITemporaryReceipQueries receiptQueries
              , IProcedureCallService procedureCallService
              , IRepository<BaseValue> baseValueRepository
              , IReceiptCommandsService receiptCommandsService
              , IRepository<DocumentHeadExtend> documentHeadExtendRepository


         )
            {
                _mapper = mapper;
                _context = context;
                _receiptQueries = receiptQueries;
                _assetsRepository = assetsRepository;
                _receiptRepository = receiptRepository;
                _documentItemRepository = DocumentItem;
                _baseValueRepository = baseValueRepository;
                _procedureCallService = procedureCallService;
                _receiptCommandsService = receiptCommandsService;
                _documentHeadExtendRepository = documentHeadExtendRepository;
            }

            public async Task<ServiceResult<ReceiptQueryModel>> Handle(UpdateExteraCostCommand request, CancellationToken cancellationToken)
            {


                var entity = await _receiptRepository.Find(request.Id);

                _mapper.Map<UpdateExteraCostCommand, Domain.Receipt>(request, entity);
                entity.IsFreightChargePaid = request.IsFreightChargePaid == null ? false : true;

                _receiptRepository.Update(entity);
                await _receiptRepository.SaveChangesAsync();

                var model = await _receiptQueries.GetById(request.Id);
                return ServiceResult<ReceiptQueryModel>.Success(model);

            }



        }
    }
}
