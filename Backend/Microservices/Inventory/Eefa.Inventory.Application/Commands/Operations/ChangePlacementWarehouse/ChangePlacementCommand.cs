using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using Eefa.Common.Data;
using Eefa.Inventory.Domain;
using Eefa.Inventory.Domain.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Inventory.Application
{
    public class ChangePlacementCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<ChangePlacementCommand>, ICommand
    {
        public int CommodityId { get; set; } = default!;
        public int Quantity { get; set; } = default!;
        public int WarehouseLayoutId { get; set; }
        public int CurrentWarehouseLayoutId { get; set; }
    }

    public class ChangePlacementCommandHandler : IRequestHandler<ChangePlacementCommand, ServiceResult>
    {


        private readonly IMapper _mapper;
        private readonly IRepository<WarehouseLayoutQuantity> _WarehouseLayoutQuantity;
        private readonly IProcedureCallService _procedureCallService;
        private readonly IRepository<WarehouseHistory> _WarehouseHistory;
        private readonly IWarehouseLayoutRepository _WarehouseLayout;


        public ChangePlacementCommandHandler(
            IMapper mapper,
            IRepository<WarehouseLayoutQuantity> WarehouseLayoutQuantity,
            IRepository<WarehouseHistory> warehouseHistory,
            IWarehouseLayoutRepository WarehouseLayout,
            IProcedureCallService procedureCallService
            )
        {
            _mapper = mapper;
            _WarehouseLayoutQuantity = WarehouseLayoutQuantity;
            _WarehouseHistory = warehouseHistory;
            _WarehouseLayout = WarehouseLayout;
            _procedureCallService = procedureCallService;
        }

        public async Task<ServiceResult> Handle(ChangePlacementCommand request, CancellationToken cancellationToken)
        {

            await UpdateWarehouseHistory(request);

            WarehouseLayoutQuantity warehouseLayoutQuantity = await _WarehouseLayoutQuantity.GetAll().Where(a => a.WarehouseLayoutId == request.CurrentWarehouseLayoutId && a.CommodityId == request.CommodityId && !a.IsDeleted).FirstOrDefaultAsync();

            if( warehouseLayoutQuantity == null )
            {
                warehouseLayoutQuantity.WarehouseLayoutId = request.WarehouseLayoutId;
                warehouseLayoutQuantity.CommodityId = request.CommodityId;
                warehouseLayoutQuantity.Quantity = request.Quantity;
                _WarehouseLayoutQuantity.Insert(warehouseLayoutQuantity);
            }
            else
            {
                
                var oldWarehouseLayoutQuantity = await _WarehouseLayoutQuantity.GetAll().Where(a => a.WarehouseLayoutId == request.WarehouseLayoutId && a.CommodityId == request.CommodityId && !a.IsDeleted).FirstOrDefaultAsync();
                if( oldWarehouseLayoutQuantity != null && oldWarehouseLayoutQuantity.Id!= warehouseLayoutQuantity.Id)
                {
                    oldWarehouseLayoutQuantity.IsDeleted = true;
                    _WarehouseLayoutQuantity.Update(oldWarehouseLayoutQuantity);

                }
                warehouseLayoutQuantity.WarehouseLayoutId = request.WarehouseLayoutId;
               

                _WarehouseLayoutQuantity.Update(warehouseLayoutQuantity);
               
            }
            await _WarehouseLayoutQuantity.SaveChangesAsync();

            await _procedureCallService.UpdateWarehouseLayoutQuantities(request.CommodityId, request.WarehouseLayoutId);
           
            //محل فعلی-----------------------------------------------

            return ServiceResult.Success();

        }


        //---------------------ثبت تاریخچه----------------------------------------------
        private async Task<int> UpdateWarehouseHistory(ChangePlacementCommand request)
        {

            var History = await _WarehouseHistory.GetAll().Where(a => a.WarehouseLayoutId == request.CurrentWarehouseLayoutId && a.Commodityld == request.CommodityId).ToListAsync();

            History.ForEach(a =>
            {
                a.WarehouseLayoutId = request.WarehouseLayoutId;
                _WarehouseHistory.Update(a);
            });

            return await _WarehouseHistory.SaveChangesAsync();

        }


    }
}