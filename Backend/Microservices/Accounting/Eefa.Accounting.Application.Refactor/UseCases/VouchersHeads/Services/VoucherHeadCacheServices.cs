using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//TODO check this too
//public class VoucherHeadCacheServices : IVoucherHeadCacheServices
//{
//    private readonly IServiceScopeFactory scopeFactory;

//    public VoucherHeadCacheServices(IServiceScopeFactory scopeFactory)
//    {
//        this.scopeFactory = scopeFactory;
//    }

//    private List<VoucherNoModel> LastVoucherNumbers { get; set; } = new List<VoucherNoModel>();

//    public Task<int> GetNewVoucherNumber()
//    {
//        using (var scope = scopeFactory.CreateScope())
//        {
//            var repository = scope.ServiceProvider.GetService<IUnitOfWork>();
//            var currentUser = scope.ServiceProvider.GetService<IApplicationUser>();

//            var lastVoucherNumberModel = this.LastVoucherNumbers.FirstOrDefault(x => x.YearId == currentUser.YearId);
//            if (lastVoucherNumberModel != null)
//            {
//                if (lastVoucherNumberModel.ExpiryDate <= DateTime.UtcNow)
//                {
//                    lastVoucherNumberModel.Number = repository.VouchersHeads.GetAsync(x => x.YearId == currentUser.YearId , y => y.Select(x => x.VoucherNo).MaxAsync(x => x).GetAwaiter().GetResult());
//                    lastVoucherNumberModel.ExpiryDate = DateTime.UtcNow.AddMinutes(1);
//                }
//                lastVoucherNumberModel.Number += 1;
//                return Task.FromResult(lastVoucherNumberModel.Number);
//            }
//            else
//            {
//                lastVoucherNumberModel = new VoucherNoModel
//                {
//                    YearId = currentUser.YearId,
//                    ExpiryDate = DateTime.UtcNow.AddMinutes(1),
//                };
//                lastVoucherNumberModel.Number = (repository.VouchersHeads.GetAsync(x => x.YearId == currentUser.YearId, y => y.Select(x => (int?)x.VoucherNo).MaxAsync(x => x).GetAwaiter().GetResult() ?? 0) + 1);
//                this.LastVoucherNumbers.Add(lastVoucherNumberModel);
//                return Task.FromResult(lastVoucherNumberModel.Number);
//            }
//        }
//    }
//}
public class VoucherNoModel
{
    public int Number { get; set; }
    public int YearId { get; set; }
    public DateTime ExpiryDate { get; set; }
}