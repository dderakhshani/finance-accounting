using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace Eefa.Common.Data
{



    public interface IADOConnection<TEntity> where TEntity : class
    {
        Task<List<TEntity>> ExecutionCommand(string StoredProcedureName, SqlParameter[] sqlParameter, TEntity TDestination);



    }
}
