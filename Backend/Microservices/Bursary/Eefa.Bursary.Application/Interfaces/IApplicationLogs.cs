using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application
{
    public interface IApplicationLogs
    {
        Task CommitLog(object request, object? error = null, int status = 0, CancellationToken cancellationToken = default);
       
    }
}
