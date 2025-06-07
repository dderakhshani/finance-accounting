using Library.Models;
using System.Threading.Tasks;

namespace Eefa.Accounting.Application.Common.Interfaces
{
    public interface IAccountingManager
    {
        Task<ServiceResult> SetVoucherHeadAsBeingModified(int voucherHeadId);
    }
}