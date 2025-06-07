using System.Threading.Tasks;
using Eefa.Common.Domain;
using Eefa.Purchase.Domain.Entities;

namespace Eefa.Purchase.Infrastructure.Services.AdminApi
{
    public interface IAdminApiService : IService
    {
        Task<Person> CallApiSavePesron(AdminApiService.PostPerson person, string Token, string Url);
    }
}