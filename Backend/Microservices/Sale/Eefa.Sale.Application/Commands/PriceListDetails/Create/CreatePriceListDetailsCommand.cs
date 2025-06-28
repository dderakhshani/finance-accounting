using AutoMapper;
using Eefa.Common;
using Eefa.Sale.Infrastructure.Data.Context;
using Eefa.Sale.Infrastructure.Data.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eefa.Sale.Application.Commands.PriceListDetails.Create
{
    public class CreatePriceListDetailsCommand : IRequest<bool>, IMapFrom<SalePriceListDetail>
    {
        public int SalePriceListId { get; set; }

        public int CommodityId { get; set; }
    }
    public class CreatePriceListDetailsCommandHandler : IRequestHandler<CreatePriceListDetailsCommand, bool>
    {
        SaleDbContext _dbContext;
        IMapper _mapper;
        public CreatePriceListDetailsCommandHandler(SaleDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<bool> Handle(CreatePriceListDetailsCommand request, CancellationToken cancellationToken)
        {
            var existingDetail = _dbContext.SalePriceListDetails.Where(x => x.SalePriceListId == request.SalePriceListId && x.CommodityId == request.CommodityId).FirstOrDefault();
            if (existingDetail != null)
            {
                return false;
            }
            var priceListDetail = _mapper.Map<SalePriceListDetail>(request);
            _dbContext.SalePriceListDetails.Add(priceListDetail);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
    }

}
