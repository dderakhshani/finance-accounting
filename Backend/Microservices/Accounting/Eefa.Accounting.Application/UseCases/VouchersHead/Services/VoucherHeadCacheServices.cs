using Library.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eefa.Accounting.Application.UseCases.VouchersHead.Services
{
    public class VoucherHeadCacheServices : IVoucherHeadCacheServices
    {
        private readonly IServiceScopeFactory scopeFactory;

        public VoucherHeadCacheServices(IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;
        }

        private List<VoucherNoModel> LastVoucherNumbers { get; set; } = new List<VoucherNoModel>();
        private readonly object lockObject = new object();


        public Task<int> GetNewVoucherNumber()
        {
            lock(lockObject)
            {
                using (var scope = scopeFactory.CreateScope())
                {
                    var repository = scope.ServiceProvider.GetService<IRepository>();
                    var currentUser = scope.ServiceProvider.GetService<ICurrentUserAccessor>();

                    var lastVoucherNumberModel = this.LastVoucherNumbers.FirstOrDefault(x => x.YearId == currentUser.GetYearId());
                    if (lastVoucherNumberModel != null)
                    {
                        if (lastVoucherNumberModel.ExpiryDate <= DateTime.UtcNow)
                        {
                            lastVoucherNumberModel.Number = repository.GetQuery<Data.Entities.VouchersHead>().Where(x => x.YearId == currentUser.GetYearId()).Select(x => x.VoucherNo).MaxAsync(x => x).GetAwaiter().GetResult();
                            lastVoucherNumberModel.ExpiryDate = DateTime.UtcNow.AddMinutes(1);
                        }
                        lastVoucherNumberModel.Number += 1;
                        return Task.FromResult(lastVoucherNumberModel.Number);
                    }
                    else
                    {
                        lastVoucherNumberModel = new VoucherNoModel
                        {
                            YearId = currentUser.GetYearId(),
                            ExpiryDate = DateTime.UtcNow.AddMinutes(1),
                        };
                        lastVoucherNumberModel.Number = (repository.GetQuery<Data.Entities.VouchersHead>().Where(x => x.YearId == currentUser.GetYearId()).Select(x => (int?)x.VoucherNo).MaxAsync(x => x).GetAwaiter().GetResult() ?? 0) + 1;
                        this.LastVoucherNumbers.Add(lastVoucherNumberModel);
                        return Task.FromResult(lastVoucherNumberModel.Number);
                    }
                }
            }
        }
    }

    public class VoucherNoModel
    {
        public int Number { get; set; }
        public int YearId { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}


