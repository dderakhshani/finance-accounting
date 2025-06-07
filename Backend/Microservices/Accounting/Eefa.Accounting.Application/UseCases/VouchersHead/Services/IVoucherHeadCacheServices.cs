using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eefa.Accounting.Application.UseCases.VouchersHead.Services
{
    public interface IVoucherHeadCacheServices
    {

        Task<int> GetNewVoucherNumber();
    }
}