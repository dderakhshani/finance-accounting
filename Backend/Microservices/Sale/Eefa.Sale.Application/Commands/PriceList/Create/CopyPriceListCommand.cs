using AutoMapper;
using Azure.Core;
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
using System.Threading;
using System.Threading.Tasks;
using static StackExchange.Redis.Role;

namespace Eefa.Sale.Application.Commands.PriceList.Copy
{
    public class CopyPriceListCommand : IRequest<bool>, IMapFrom<SalePriceList>
    {

        public string Title { get; set; } = null!;

        public class CopyPriceListCommandHandler : IRequestHandler<CopyPriceListCommand, bool>
        {
            SaleDbContext _dbContext;
            IMapper _mapper;
            public CopyPriceListCommandHandler(SaleDbContext dbContext, IMapper mapper, IPriceListQueries priceListQueries)
            {
                _dbContext = dbContext;
                _mapper = mapper;
            }


            public async Task<bool> Handle(CopyPriceListCommand request, CancellationToken cancellationToken)
            {
                var currentDateTime = DateTime.Now;
                var allListPrice = _dbContext.SalePriceLists.Where(x => x.IsDeleted != true).ToList();
                var srcPriceList = allListPrice.Where(x => x.RootId == null).OrderByDescending(x => x.Id).First();

                var newPriceList = _mapper.Map<SalePriceList>(srcPriceList);
                newPriceList.StartDate = currentDateTime;
                newPriceList.Title = request.Title;
                _dbContext.SalePriceLists.Add(newPriceList);

                var allChildren = allListPrice.Where(x => x.RootId == newPriceList.Id).ToList();
                var levelOneChildrens = allChildren.Where(x => x.ParentId == newPriceList.Id).ToList();


                CopyPriceListChildren(currentDateTime, levelOneChildrens, allListPrice, newPriceList.Id, _mapper, _dbContext);

                await _dbContext.SaveChangesAsync(cancellationToken);
                return true;
            }


            public static void CopyPriceListChildren(DateTime currentDateTime, List<SalePriceList> levelOneChildrens, List<SalePriceList> allListPrice, int parentId, IMapper mapper, SaleDbContext _dbContext)
            {
                foreach (var children in levelOneChildrens)
                {
                    var newPriceListChildren = mapper.Map<SalePriceList>(children);
                    newPriceListChildren.StartDate = currentDateTime;
                    newPriceListChildren.ParentId = parentId;
                    _dbContext.SalePriceLists.Add(newPriceListChildren);
                    var subchildren = allListPrice.Where(x => x.ParentId == children.Id).ToList();
                    if (subchildren.Any())
                    {
                        CopyPriceListChildren(currentDateTime, subchildren, allListPrice, newPriceListChildren.Id, mapper, _dbContext);
                    }

                }
            }
        }
    }
}
