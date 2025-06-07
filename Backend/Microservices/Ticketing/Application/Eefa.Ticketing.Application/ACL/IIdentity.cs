using Eefa.Ticketing.Application.Contract.Dtos.BasicInfos;
using System.Threading.Tasks;

namespace Eefa.Ticketing.Application.ACL
{
    public interface IIdentity
    {
        Task<LoginResult> LoginAsync();
    }
}
