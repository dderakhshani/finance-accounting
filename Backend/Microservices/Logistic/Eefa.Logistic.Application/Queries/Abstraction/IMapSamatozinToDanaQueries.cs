using Eefa.Common.CommandQuery;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eefa.Logistic.Application
{
    
        public interface IMapSamatozinToDanaQueries : IQuery
        {

            Task<PagedList<MapSamatozinToDanaModel>> GetAll(PaginatedQueryModel query);

            Task<MapSamatozinToDanaModel> GetById(int id);
        }
    
}
