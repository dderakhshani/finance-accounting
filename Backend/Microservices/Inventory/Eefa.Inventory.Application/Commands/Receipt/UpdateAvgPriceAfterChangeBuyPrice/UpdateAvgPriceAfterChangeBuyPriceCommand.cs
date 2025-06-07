using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using Eefa.Common.Data;
using Eefa.Common.Exceptions;
using Eefa.Inventory.Domain;
using Eefa.Inventory.Domain.Common;
using Eefa.Invertory.Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Index.HPRtree;

namespace Eefa.Inventory.Application
{
    public class UpdateAvgPriceAfterChangeBuyPriceCommand : CommandBase, IRequest<ServiceResult<Domain.Receipt>>, IMapFrom<UpdateAvgPriceAfterChangeBuyPriceCommand>, ICommand
    {

        public int DocumentId { get; set; } = default!;
        public int? DocumentItemId { get; set; } = default!;

    }

    public class UpdateAvgPriceAfterChangeBuyPriceCommandHandler : IRequestHandler<UpdateAvgPriceAfterChangeBuyPriceCommand, ServiceResult<Domain.Receipt>>
    {
        private readonly IMapper _mapper;
        private readonly IInvertoryUnitOfWork _context;
        private readonly IProcedureCallService _procedureCallService;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly IReceiptCommandsService _receiptCommandsService;




        public UpdateAvgPriceAfterChangeBuyPriceCommandHandler(

             ICurrentUserAccessor currentUserAccessor
            , IReceiptCommandsService receiptCommandsService
            , IProcedureCallService procedureCallService
            , IInvertoryUnitOfWork context
            , IMapper mapper

            )

        {
            _mapper = mapper;
            _context = context;
            _procedureCallService = procedureCallService;
            _currentUserAccessor = currentUserAccessor;
            _receiptCommandsService = receiptCommandsService;

        }
        public async Task<ServiceResult<Domain.Receipt>> Handle(UpdateAvgPriceAfterChangeBuyPriceCommand request, CancellationToken cancellationToken)
        {

            try
            {

                if (request.DocumentItemId > 0)
                {
                    var items = await _context.DocumentItems.Where(a => a.Id == request.DocumentItemId).FirstOrDefaultAsync();
                    var receipt = await _context.DocumentHeads.Where(a => a.Id == items.DocumentHeadId).FirstOrDefaultAsync();
                    
                    var spComputeAvgPrice = new spComputeAvgPriceParam() { CommodityId = Convert.ToInt32(items.CommodityId), ReceiptId = receipt.Id, YearId = _currentUserAccessor.GetYearId(), CaredxRepairId = Guid.NewGuid() };
                    var CardexRepair = new spUpdateinventory_CaredxRepairParam() { CaredxRepairId = Guid.NewGuid(), YearId = _currentUserAccessor.GetYearId() };

                    await _procedureCallService.ComputeAvgPrice(spComputeAvgPrice, CardexRepair);

                }
                else
                {
                    var receipts = await _context.DocumentHeads.Where(a => a.DocumentId == request.DocumentId).ToListAsync();


                    foreach (var receipt in receipts)
                    {
                        var items = _context.DocumentItems.Where(a => a.DocumentHeadId == receipt.Id).ToList();

                        foreach (var item in items)
                        {


                            var spComputeAvgPrice = new spComputeAvgPriceParam() { CommodityId = Convert.ToInt32(item.CommodityId), ReceiptId = receipt.Id, YearId = _currentUserAccessor.GetYearId(), CaredxRepairId = Guid.NewGuid() };
                            var CardexRepair = new spUpdateinventory_CaredxRepairParam() { CaredxRepairId = Guid.NewGuid(), YearId = _currentUserAccessor.GetYearId() };
                           
                            await _procedureCallService.ComputeAvgPrice(spComputeAvgPrice, CardexRepair);
                        };
                    };
                }



                return ServiceResult<Domain.Receipt>.Success(new Receipt());
            }
            catch (Exception ex)
            {
                new LogWriter("UpdateAvgPriceAfterChangeBuyPrice Error:" + ex.Message.ToString(), "UpdateAvgPriceAfterChangeBuyPrice_Error");
                if (ex.InnerException != null)
                {
                    new LogWriter("UpdateAvgPriceAfterChangeBuyPrice InnerException:" + ex.InnerException.ToString(), "UpdateAvgPriceAfterChangeBuyPrice_Error");
                }
            }

            return ServiceResult<Domain.Receipt>.Success(new Receipt());

        }




    }
}