using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;

namespace Eefa.Common.Domain
{
    public interface ICrudService<T> : IService where T : class, IBaseEntity
    {
        Task<List<T>> GetAll(Pagination pagination, CancellationToken cancellationToken);

        Task<T> FindById(int id, CancellationToken cancellationToken);

        void Add(T inpute);

        void Update(T inpute, CancellationToken cancellationToken);

        void SoftDelete(int id, CancellationToken cancellationToken);

        Task<List<T>> Search(Pagination pagination, List<QueryCondition> queries, CancellationToken cancellationToken);

    }
}