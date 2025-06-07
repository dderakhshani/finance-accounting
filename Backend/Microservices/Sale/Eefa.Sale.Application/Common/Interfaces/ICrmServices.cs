using System.Threading.Tasks;

namespace Eefa.Sale.Application.Common.Interfaces
{
    public interface ICrmServices
    {
        Task<bool> SendCustomer(int customerId);
    }
}
