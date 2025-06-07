using Eefa.Common.Web;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Eefa.Common.Data
{

    public  class ADOConnection<TEntity>: IADOConnection<TEntity> where TEntity : class
    {
        
       
        private static IConfigurationAccessor _configurationAccessor;
        public ADOConnection(IConfigurationAccessor configurationAccessor) => _configurationAccessor = configurationAccessor;
        
        public async Task<List<TEntity>> ExecutionCommand(string StoredProcedureName, SqlParameter[] sqlParameter, TEntity TDestination)
        {
           string ConnectionString = _configurationAccessor.GetConnectionString().DefaultString;
            List<TEntity> lstU = new List<TEntity>();
            DataReaderMapper<TEntity> dataReaderMapper = new DataReaderMapper<TEntity>();
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(StoredProcedureName, con);
                await con.OpenAsync();

                cmd.CommandType = CommandType.StoredProcedure;
                if (sqlParameter.Length > 0)
                {
                    cmd.Parameters.AddRange(sqlParameter);
                }

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds, "newtbale");


                if (ds.Tables.Count > 0)
                {

                    DataTable dataTable = ds.Tables[0];

                    lstU = dataReaderMapper.MapProp(dataTable, TDestination);

                }

                await con.CloseAsync();
            }
            return lstU;
        }
    }




}
