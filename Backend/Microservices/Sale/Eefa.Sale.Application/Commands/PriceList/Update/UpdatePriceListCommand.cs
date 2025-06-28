using AutoMapper;
using Eefa.Common;
using Eefa.Sale.Application.Queries.PriceList;
using Eefa.Sale.Infrastructure.Data.Context;
using Eefa.Sale.Infrastructure.Data.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eefa.Sale.Application.Commands.PriceList.Update
{
    public class UpdatePriceListCommand : IRequest<bool>, IMapFrom<SalePriceList>
    {
        public int Id { get; set; }
        public int? RootId { get; set; }

        public int? ParentId { get; set; }

        public string Title { get; set; } = null!;

        public string? Descriptions { get; set; }

        public int? AccountReferenceGroupId { get; set; }
        public double? Price { get; set; }
        public double? DollarPrice { get; set; }
    }

    public class UpdatePriceListCommandHandler : IRequestHandler<UpdatePriceListCommand, bool>
    {
        SaleDbContext _dbContext;
        IMapper _mapper;

        public UpdatePriceListCommandHandler(SaleDbContext dbContext, IMapper mapper, IPriceListQueries priceListQueries)
        {
            _dbContext = dbContext;
            _mapper = mapper;

        }


        public async Task<bool> Handle(UpdatePriceListCommand request, CancellationToken cancellationToken)
        {
            var currentPriceList = _dbContext.SalePriceLists.Where(x => x.Id == request.Id && x.IsDeleted != true).FirstOrDefault();
            var subCommodities = _dbContext.SalePriceListDetails.Where(x => x.SalePriceListId == request.Id && x.IsDeleted != true).ToList();
            if (subCommodities.Count > 0)
            {
                foreach (var subCommodity in subCommodities)
                {
                    var priceHistory = new FixedPriceHistory
                    {
                        CommodityId = subCommodity.CommodityId,
                        Price = request.Price.HasValue ? (long?)request.Price.Value : null,
                        DollarPrice = request.DollarPrice.HasValue ? request.DollarPrice.Value : null,
                        StartDate = DateTime.Now,
                    };
                    _dbContext.FixedPriceHistories.Add(priceHistory);
                }
            }
            _mapper.Map(request, currentPriceList);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
