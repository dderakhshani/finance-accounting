
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
    public class CreatePriceListCommand : IRequest<ServiceResult>, IMapFrom<SalePriceList>
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

    public class CreatePriceListCommandHandler : IRequestHandler<CreatePriceListCommand, ServiceResult>
    {
        SaleDbContext _dbContext;
        IMapper _mapper;
        public CreatePriceListCommandHandler(SaleDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }


        public async Task<ServiceResult> Handle(CreatePriceListCommand request, CancellationToken cancellationToken)
        {
            bool isParentAndRootValid = (request.ParentId == null && request.RootId == null) || (request.ParentId != null && request.RootId != null);

            if (!isParentAndRootValid)
            {
                var errors = new Dictionary<string, List<string>> { { "error:", new List<string> { "اگر این لیست قیمت زیرمجموعه است، مقدار 'ریشه' هم باید وارد شود. اگر لیست قیمت اصلی است، مقدار 'والد' و 'ریشه' نباید وارد شوند." } } };
                return ServiceResult.Failed(errors);
            }
            var priceList = _mapper.Map<SalePriceList>(request);
            _dbContext.SalePriceLists.Add(priceList);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return ServiceResult.Success();


        }
    }
}
