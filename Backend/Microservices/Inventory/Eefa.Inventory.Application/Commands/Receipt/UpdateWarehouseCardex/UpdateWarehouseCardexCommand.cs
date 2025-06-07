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
using Eefa.Invertory.Infrastructure.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Inventory.Application
{
    public class UpdateWarehouseCardexCommand : CommandBase, IRequest<ServiceResult<Domain.Receipt>>, IMapFrom<UpdateWarehouseCardexCommand>, ICommand
    {


        public int WarehouseId { get; set; } = default!;
        public int YearId { get; set; } = default!;
    }

    public class UpdateWarehouseCardexCommandHandler : IRequestHandler<UpdateWarehouseCardexCommand, ServiceResult<Domain.Receipt>>
    {
        private readonly IMapper _mapper;
        private readonly IInvertoryUnitOfWork _context;
        private readonly IProcedureCallService _procedureCallService;

        public UpdateWarehouseCardexCommandHandler(

              IProcedureCallService procedureCallService
            , IInvertoryUnitOfWork context
            , IMapper mapper

            )

        {
            _mapper = mapper;
            _context = context;
            _procedureCallService = procedureCallService;

        }


        public async Task<ServiceResult<Domain.Receipt>> Handle(UpdateWarehouseCardexCommand request, CancellationToken cancellationToken)
        {

            var model = new spUpdateWarehouseCardexParam() { warehouseId = request.WarehouseId, YearId = request.YearId };
            await _procedureCallService.UpdateWarehouseCardex(model);

            return ServiceResult<Domain.Receipt>.Success(new Receipt());

        }




    }
}