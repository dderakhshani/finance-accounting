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
    public class CopyPriceListCommand : IRequest<ServiceResult>, IMapFrom<SalePriceList>
    {

        public int SourcePriceListId { get; set; }
        public string NewPriceListName { get; set; } = null!;
    }
    public class CopyPriceListCommandHandler : IRequestHandler<CopyPriceListCommand, ServiceResult>
    {
        SaleDbContext _dbContext;
        IMapper _mapper;
        public CopyPriceListCommandHandler(SaleDbContext dbContext, IMapper mapper, IPriceListQueries priceListQueries)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<ServiceResult> Handle(CopyPriceListCommand request, CancellationToken cancellationToken)
        {
            var srcRoot = _dbContext.SalePriceLists.First(x => x.Id == request.SourcePriceListId); //پیدا کردن ریشه لیست قیمت
            var currentDateTime = DateTime.Now;


            SalePriceList newRoot = new SalePriceList
            {
                Title = request.NewPriceListName,
                StartDate = currentDateTime,
                ParentId = null,
                RootId = null,
                LevelCode = "004",
            };


            _dbContext.SalePriceLists.Add(newRoot);
            await _dbContext.SaveChangesAsync(cancellationToken); // برای ثبت id newRoot
            var allChildren = _dbContext.SalePriceLists.Where(x => x.RootId == request.SourcePriceListId).ToList();//پیدا کردن تمام فرزند های این ریشه

            await CopyPriceListChildrenAsync(srcRoot.Id, newRoot, allChildren, currentDateTime, newRoot.Id, cancellationToken); //کپی فرزندان این ریشه

            if (await _dbContext.SaveChangesAsync(cancellationToken) > 0)
            {
                return ServiceResult.Success();
            }

            return ServiceResult.Failed();
        }

        private async Task CopyPriceListChildrenAsync(int srcParentId, SalePriceList newParent, List<SalePriceList> allChildren, DateTime currentDateTime, int rootId, CancellationToken cancellationToken)
        {
            var children = allChildren
                .Where(x => x.ParentId == srcParentId)
                .ToList(); //پیدا کردن  فرزندان این والد

            foreach (var child in children)
            {

                SalePriceList newChild = new SalePriceList
                {
                    Title = child.Title,
                    StartDate = currentDateTime,
                    ParentId = newParent.Id,
                    RootId = rootId,
                    LevelCode = "004",
                    DollarPrice = child.Price,
                    Price = child.Price,
                };

                _dbContext.SalePriceLists.Add(newChild);
                await _dbContext.SaveChangesAsync(cancellationToken);

                await CopyPriceListChildrenAsync(child.Id, newChild, allChildren, currentDateTime, rootId, cancellationToken); // درصورت فرزند داشتن والد خود تابع دوباره صدا مزنم
            }
        }
    }
}
