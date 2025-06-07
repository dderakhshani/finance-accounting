using System.Collections.Generic;

namespace Eefa.Common.Data.Query { 
    public class PaginatedQueryModel : Pagination, SearchableQuery
    {
        public List<QueryCondition> Conditions { get; set; }
       
    }

    
}
