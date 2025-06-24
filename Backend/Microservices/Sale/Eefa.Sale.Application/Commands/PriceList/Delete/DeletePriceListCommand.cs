using AutoMapper;
using Eefa.Common;
using Eefa.Sale.Application.Commands.PriceList.Update;
using Eefa.Sale.Application.Queries.PriceList;
using Eefa.Sale.Infrastructure.Data.Context;
using Eefa.Sale.Infrastructure.Data.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eefa.Sale.Application.Commands.PriceList.Delete
{
    public class DeletePriceListCommand : IRequest<bool>, IMapFrom<SalePriceList>
    {
        public int Id { get; set; }
        public class DeletePriceListCommandHandler : IRequestHandler<DeletePriceListCommand, bool>
        {
            SaleDbContext _dbContext;
            IMapper _mapper;

            public DeletePriceListCommandHandler(SaleDbContext dbContext, IMapper mapper, IPriceListQueries priceListQueries)
            {
                _dbContext = dbContext;
                _mapper = mapper;

            }


            public async Task<bool> Handle(DeletePriceListCommand request, CancellationToken cancellationToken)
            {
                var currentPriceList = _dbContext.SalePriceLists.Where(x => x.Id == request.Id).FirstOrDefault();
                currentPriceList.IsDeleted = true;
                _mapper.Map(request, currentPriceList);
                await _dbContext.SaveChangesAsync(cancellationToken);
                return true;
            }
        }
    }
}
