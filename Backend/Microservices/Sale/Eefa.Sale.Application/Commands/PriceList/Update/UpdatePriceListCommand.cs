using AutoMapper;
using Eefa.Common;
using Eefa.Sale.Application.Queries.PriceList;
using Eefa.Sale.Infrastructure.Data.Context;
using Eefa.Sale.Infrastructure.Data.Entities;
using MediatR;
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
        IPriceListQueries _priceListQueries;

        public UpdatePriceListCommandHandler(SaleDbContext dbContext, IMapper mapper, IPriceListQueries priceListQueries)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _priceListQueries = priceListQueries;

        }


        public async Task<bool> Handle(UpdatePriceListCommand request, CancellationToken cancellationToken)
        {
            SalePriceList currentPriceList = _priceListQueries.GetById(request.Id).Result;
            _mapper.Map(request, currentPriceList);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
