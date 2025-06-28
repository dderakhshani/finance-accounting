
using AutoMapper;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using Eefa.Common.Data;
using Eefa.Sale.Infrastructure.Data.Context;
using Eefa.Sale.Infrastructure.Data.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Sale.Application.Commands
{
    public class CreatePriceListCommand : IRequest<bool>, IMapFrom<SalePriceList>
    {
        public int? RootId { get; set; }

        public int? ParentId { get; set; }

        public string LevelCode { get; set; } = null!;

        public string Title { get; set; } = null!;

        public string? Descriptions { get; set; }

        public int? AccountReferenceGroupId { get; set; }
        public double? Price { get; set; }
        public double? DollarPrice { get; set; }

        public DateTime StartDate { get; set; }
    }

    public class CreatePriceListCommandHandler : IRequestHandler<CreatePriceListCommand, bool>
    {
        SaleDbContext _dbContext;
        IMapper _mapper;
        public CreatePriceListCommandHandler(SaleDbContext dbContext, IMapper mapper) 
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }


        public async Task<bool> Handle(CreatePriceListCommand request, CancellationToken cancellationToken)
        {
            var priceList = _mapper.Map<SalePriceList>(request);
            _dbContext.SalePriceLists.Add(priceList);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
