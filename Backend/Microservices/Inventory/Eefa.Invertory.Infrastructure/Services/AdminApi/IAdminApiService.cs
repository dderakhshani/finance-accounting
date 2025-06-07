using System.Threading.Tasks;
using Eefa.Common.Domain;
using Eefa.Inventory.Domain;
using static Eefa.Invertory.Infrastructure.Services.AdminApi.AdminApiService;

namespace Eefa.Invertory.Infrastructure.Services.AdminApi
{
    public interface IAdminApiService : IService
    {
        Task<Person> CallApiSavePerson(AdminApiService.PostPerson person, string Token, string Url);
        Task<ResultModel> CallApiAutoVoucher2(UpdateAutoVoucher documents, string Token, string Url);
       
    }
}