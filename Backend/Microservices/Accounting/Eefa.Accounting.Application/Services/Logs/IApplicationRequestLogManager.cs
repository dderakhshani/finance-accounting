using Library.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Accounting.Application.Services.Logs
{
    public interface IApplicationRequestLogManager
    {
        Task CommitLog(object request, object? error = null, int status = 0, CancellationToken cancellationToken = default);

        Task<ServiceResult> GetAll(GetLogsQuery query);
    }
}