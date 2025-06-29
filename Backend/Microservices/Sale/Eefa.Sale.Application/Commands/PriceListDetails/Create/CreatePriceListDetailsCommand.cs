using AutoMapper;
using Eefa.Common;
using Eefa.Sale.Infrastructure.Data.Context;
using Eefa.Sale.Infrastructure.Data.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eefa.Sale.Application.Commands.PriceListDetails.Create
{
    public class CreatePriceListDetailsCommand : IRequest<ServiceResult>, IMapFrom<SalePriceListDetail>
    {
        public int SalePriceListId { get; set; }

        public List<int> CommodityIds { get; set; }
    }
    public class CreatePriceListDetailsCommandHandler : IRequestHandler<CreatePriceListDetailsCommand, ServiceResult>
    {
        SaleDbContext _dbContext;
        IMapper _mapper;
        public CreatePriceListDetailsCommandHandler(SaleDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<ServiceResult> Handle(CreatePriceListDetailsCommand request, CancellationToken cancellationToken)
        {

            var existingDetails = _dbContext.SalePriceListDetails.Where(x => x.SalePriceListId == request.SalePriceListId).Select(x => x.CommodityId).ToList();

            var newDetails = request.CommodityIds.Where(id => !existingDetails.Contains(id))
                .Select(id => new SalePriceListDetail
                {
                    SalePriceListId = request.SalePriceListId,
                    CommodityId = id
                }).ToList();

            if (newDetails.Count == 0)
            {
                var errors = new Dictionary<string, List<string>> { { "error:", new List<string> { "هیچ کالای جدیدی برای افزودن وجود ندارد." } } };
                return ServiceResult.Failed(errors);
            }
            _dbContext.SalePriceListDetails.AddRange(newDetails);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return ServiceResult.Success();

        }
    }

}
