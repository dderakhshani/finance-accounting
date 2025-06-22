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

        public int? _ChildAccountReferenceGroupId { get; set; }

        public class CreatePriceListDetailsCommandHandler : IRequestHandler<CreatePriceListDetailsCommand, bool>
        {
            SaleDbContext dbContext;
            IMapper mapper;
            public CreatePriceListDetailsCommandHandler(SaleDbContext dbContext, IMapper mapper)
            {
                this.dbContext = dbContext;
                this.mapper = mapper;
            }
            public async Task<bool> Handle(CreatePriceListDetailsCommand request, CancellationToken cancellationToken)
            {
                var priceListDetail = mapper.Map<SalePriceListDetail>(request);

                if (priceListDetail != null)
                {
                    dbContext.SalePriceListDetails.Add(priceListDetail);
                    await dbContext.SaveChangesAsync(cancellationToken);
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
