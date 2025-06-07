using Library.Models;
using System.Collections.Generic;

namespace Eefa.Accounting.Application.Services.Logs
{
    public class GetLogsQuery : Pagination
    {
        public List<Condition> Conditions { get; set; }
    }
}
