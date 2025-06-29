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

namespace Eefa.Sale.Application.Commands.PriceList.Update
{
    public class UpdatePriceListCommand : IRequest<ServiceResult>, IMapFrom<SalePriceList>
    {
        public int Id { get; set; }

        public int? ParentId { get; set; }

        public string Title { get; set; } = null!;
        public string LevelCode { get; set; } = null!;
        public string? Descriptions { get; set; }
        public int? AccountReferenceGroupId { get; set; }
        public double? Price { get; set; }
        public double? DollarPrice { get; set; }
    }

    public class UpdatePriceListCommandHandler : IRequestHandler<UpdatePriceListCommand, ServiceResult>
    {
        SaleDbContext _dbContext;
        IMapper _mapper;

        public UpdatePriceListCommandHandler(SaleDbContext dbContext, IMapper mapper, IPriceListQueries priceListQueries)
        {
            _dbContext = dbContext;
            _mapper = mapper;

        }


        public async Task<ServiceResult> Handle(UpdatePriceListCommand request, CancellationToken cancellationToken)
        {
            var currentPriceList = await _dbContext.SalePriceLists.FirstAsync(x => x.Id == request.Id, cancellationToken);

            if (currentPriceList.ParentId != null && request.ParentId == null)
            {
                var errors = new Dictionary<string, List<string>> { { "error:", new List<string> { "امکان تبدیل زیرمجموعه به ریشه وجود ندارد." } } };
                return ServiceResult.Failed(errors);
            }
            _mapper.Map(request, currentPriceList);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return ServiceResult.Success();

        }
    }
}
