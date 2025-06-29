using AutoMapper;
using Eefa.Common;
using Eefa.Sale.Application.Commands.PriceList.Delete;
using Eefa.Sale.Application.Queries.PriceList;
using Eefa.Sale.Infrastructure.Data.Context;
using Eefa.Sale.Infrastructure.Data.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eefa.Sale.Application.Commands.PriceListDetails.Delete
{
    public class DeletePriceListDetailsCommand : IRequest<ServiceResult>, IMapFrom<SalePriceListDetail>
    {
        public int Id { get; set; }
    }
    public class DeletePriceListDetailsCommandHandler : IRequestHandler<DeletePriceListDetailsCommand, ServiceResult>
    {
        SaleDbContext _dbContext;
        IMapper _mapper;

        public DeletePriceListDetailsCommandHandler(SaleDbContext dbContext, IMapper mapper, IPriceListQueries priceListQueries)
        {
            _dbContext = dbContext;
            _mapper = mapper;

        }


        public async Task<ServiceResult> Handle(DeletePriceListDetailsCommand request, CancellationToken cancellationToken)
        {
            var currentPriceListDetails = _dbContext.SalePriceListDetails.Where(x => x.Id == request.Id).FirstOrDefault();
            _dbContext.RemoveEntity(currentPriceListDetails);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return ServiceResult.Success();
        }
    }
}
