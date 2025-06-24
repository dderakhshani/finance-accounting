using AutoMapper;
using Eefa.Common;
using Eefa.Sale.Infrastructure.Data.Context;
using Eefa.Sale.Infrastructure.Data.Entities;
using MediatR;
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
                var priceListDetail = _mapper.Map<SalePriceListDetail>(request);

                if (priceListDetail != null)
                {
                    _dbContext.SalePriceListDetails.Add(priceListDetail);
                    var priceList = _dbContext.SalePriceLists.Where(x => x.Id == request.SalePriceListId && x.IsDeleted != true).FirstOrDefault();

                    if (priceList != null)
                    {
                        var priceHistory = new FixedPriceHistory
                        {
                            CommodityId = priceListDetail.CommodityId,
                            Price = priceList.Price.HasValue ? (long?)priceList.Price.Value : null,
                            DollarPrice = priceList.DollarPrice.HasValue ? (long?)priceList.DollarPrice.Value : null,
                            StartDate = priceList.StartDate
                        };
                        _dbContext.FixedPriceHistories.Add(priceHistory);
                    }
                    await _dbContext.SaveChangesAsync(cancellationToken);
                    return true;
                }
                else
                {
                    return false;
                }

            }
        }

    }
}
