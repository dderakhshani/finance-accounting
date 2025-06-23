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
            IPriceListQueries _priceListQueries;
            public CopyPriceListCommandHandler(SaleDbContext dbContext, IMapper mapper, IPriceListQueries priceListQueries)
            {
                _dbContext = dbContext;
                _mapper = mapper;
                _priceListQueries = priceListQueries;
            }


            public async Task<bool> Handle(CopyPriceListCommand request, CancellationToken cancellationToken)
            {
                var currentDateTime = DateTime.Now;
                var allListPrice = _priceListQueries.GetAll().Result;
                var srcPriceList = allListPrice.Where(x => x.RootId == null).OrderByDescending(x => x.Id).First();

                var newPriceList = _mapper.Map<SalePriceList>(srcPriceList);
                newPriceList.StartDate = currentDateTime;
                newPriceList.Title = request.Title;
                _dbContext.SalePriceLists.Add(newPriceList);

                var allChildren = allListPrice.Where(x => x.RootId == newPriceList.Id).ToList();
                var levelOneChildrens = allChildren.Where(x => x.ParentId == newPriceList.Id).ToList();


                CopyPriceListChildren(currentDateTime, levelOneChildrens, allListPrice, newPriceList.Id, _mapper, _dbContext);


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

                //foreach (var child in children)
                //{
                //    var newPriceList = new PriceList
                //    {
                //        Date = DateTime.Now,
                //        IsDeleted = child.IsDeleted,
                //        ListName = child.ListName,
                //        Description = child.Description,
                //        DollarPrice = child.DollarPrice,
                //        Price = child.Price,
                //        RootId = rootId,
                //        ParentId = parentId
                //    };

                //    context.PriceLists.Add(newPriceList);
                //    context.SaveChanges(); // برای دسترسی به newPriceList.Id در ادامه

                //    var items = context.PriceListItems
                //        .Where(x => x.PriceListId == child.Id && !x.IsDeleted)
                //        .ToList();

                //    foreach (var item in items)
                //    {
                //        var newItem = new PriceListItem
                //        {
                //            IsDeleted = item.IsDeleted,
                //            Price = item.Price,
                //            PriceListId = newPriceList.Id,
                //            SaleProduct2Id = item.SaleProduct2Id,
                //            CustomerTypeBaseId = item.CustomerTypeBaseId
                //        };

                //        context.PriceListItems.Add(newItem);
                //    }

                //    var subChildren = allChildren
                //        .Where(x => x.ParentId == child.Id)
                //        .ToList();

                //    if (subChildren.Any())
                //    {
                //        CopyPriceListChildren(rootId, newPriceList.Id, subChildren, allChildren, context);
                //    }
                //}

            }
        }
    }
}
