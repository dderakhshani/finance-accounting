using Eefa.Common.CommandQuery;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;
using Eefa.Logistic.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eefa.Logistic.Application
{
    
        public interface IPrehensionQueries : IQuery
        {

            Task<PagedList<Prehension>> GetAll(PaginatedQueryModel query);
            Task<PagedList<string>> GetGroupByCode(PaginatedQueryModel query);
            Task<Prehension> GetById(int id);
        }
    
}
