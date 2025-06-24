using AutoMapper;
using Eefa.Common;
using Eefa.Sale.Application.Commands.PriceListDetails.Create;
using Eefa.Sale.Infrastructure.Data.Context;
using Eefa.Sale.Infrastructure.Data.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eefa.Sale.Application.Commands.PriceListDetails.Update
{
    public class UpdatePriceListDetailsCommand : IRequest<bool>, IMapFrom<SalePriceListDetail>
    {
        public int id { get; set; }
        public int SalePriceListId { get; set; }

        public int CommodityId { get; set; }


        public class UpdatePriceListDetailsCommandHandler : IRequestHandler<UpdatePriceListDetailsCommand, bool>
        {
            SaleDbContext _dbContext;
            IMapper _mapper;

            public UpdatePriceListDetailsCommandHandler(SaleDbContext dbContext, IMapper mapper)
            {
                _dbContext = dbContext;
                _mapper = mapper;
            }
            public async Task<bool> Handle(UpdatePriceListDetailsCommand request, CancellationToken cancellationToken)
            {
                SalePriceListDetail salePriceListDetail = _dbContext.SalePriceListDetails.Where(x => x.Id == request.id && x.IsDeleted != true).First();
                _mapper.Map(request, salePriceListDetail);
                await _dbContext.SaveChangesAsync(cancellationToken);
                return true;
            }
        }
    }
}
