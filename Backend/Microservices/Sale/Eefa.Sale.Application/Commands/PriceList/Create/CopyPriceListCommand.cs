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

namespace Eefa.Sale.Application.Commands.PriceList.Copy
{
    public class CopyPriceListCommand : IRequest<bool>, IMapFrom<SalePriceList>
    {

        public string Title { get; set; } = null!;

        public int Id { get; set; }

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
                var srcPriceList = await _priceListQueries.GetById(request.Id);
                var newPriceList = new SalePriceList
                {
                    StartDate = DateTime.Now,
                    IsDeleted = false,
                    Title = request.Title
                };
                _dbContext.Add(newPriceList);
                var children = _priceListQueries.GetAll().Result.Where(x => x.RootId == request.Id).ToList();
                var newchildren = children.Where(x => x.ParentId == request.Id).ToList();

                return true;
            }


            public static void CopyPriceListChildren(SalePriceList root, SalePriceList parent, List<SalePriceList> children, List<SalePriceList> allChildren, SaleDbContext context)
            {
                foreach (var child in children)
                {
                    var newPriceList = new SalePriceList
                    {
                        StartDate = DateTime.Now,
                        IsDeleted = child.IsDeleted,
                        Title = child.Title,
                        Descriptions = child.Descriptions,
                        DollarPrice = child.DollarPrice,
                        Price = child.Price,
                        
                    };
                    context.Add(newPriceList);


                    //foreach (var child in children)
                    //{
                    //    var newPriceList = new PriceList
                    //    {
                    //        Date = DateTime.Now,
                    //        IsDeleted = child.IsDeleted,
                    //        ListName = child.ListName,
                    //        Description = child.Description,
                    //        DollarPrice = child.DollarPrice,
                    //        //PriceList1= root,
                    //        PriceList2 = parent,
                    //        Price = child.Price,
                    //    };
                    //    context.PriceLists.Add(newPriceList);

                    //    var items = context.PriceListItems.Where(x => x.PriceListId == child.Id && x.IsDeleted != true).ToList();
                    //    foreach (var item in items)
                    //    {
                    //        var newPriceListItem = new PriceListItem
                    //        {
                    //            IsDeleted = item.IsDeleted,
                    //            Price = item.Price,
                    //            PriceList = newPriceList,
                    //            SaleProduct2Id = item.SaleProduct2Id,
                    //            CustomerTypeBaseId = item.CustomerTypeBaseId,

                    //        };
                    //        context.PriceListItems.Add(newPriceListItem);
                    //    }
                    //    var newChildren = allChildren.Where(x => x.ParentId == child.Id).ToList();

                    //    if (newChildren.Count > 0)
                    //        CopyPriceListChildren(root, newPriceList, newChildren, allChildren, context);
                    //}
                }
            }
        }
    }
}
