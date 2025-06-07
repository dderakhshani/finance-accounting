using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;
using Eefa.Inventory.Domain;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;


namespace Eefa.Inventory.Application
{
    public class AuditQueries : IAuditQueries
    {

        private readonly IMapper _mapper;
        private readonly IInvertoryUnitOfWork _context;
        private readonly IProcedureCallService _IProcedureCallService;

        public AuditQueries(
              IMapper mapper
            , IInvertoryUnitOfWork context
            , IProcedureCallService IProcedureCallService
            )
        {
            _mapper = mapper;
            _context = context;
            _IProcedureCallService = IProcedureCallService;

        }

        
        public Task<PagedList<SpGetAudit>> GetAll(string FromDate, string ToDate, PaginatedQueryModel query)
        {
           
            throw new NotImplementedException();
        }
        [HttpGet]
        //public async Task<PagedList<SpGetAudit>> GetAuditById(int PrimaryId, string TableName)
        //{
           
        //    var entity = await _IProcedureCallService.GetAuditById(PrimaryId, TableName);
        //    var result = new PagedList<SpGetAudit>()
        //    {
        //        Data = entity,
        //        TotalCount = 0
        //    };
        //    return result;
        //}

        public async Task<PagedList<spGetLogTable>> GetAuditById(int PrimaryId, string TableName)
        {
            List<spGetLogTable> entity = new List<spGetLogTable>();
            
            var table = Type.GetType("Eefa.Inventory.Application." + TableName);
            var myPropertyInfo = table.GetProperties();
            var tableName = table.Name;


            for (int i = 0; i < myPropertyInfo.Length; i++)
            {
                var Name = myPropertyInfo[i].Name.ToString();
                try
                {
                    var model = await _IProcedureCallService.spGetLogTable(PrimaryId, TableName.Replace("Log", ""), Name, "[Audit].[inventory]");
                    if (model.Count() > 0)
                        entity.AddRange(model);
                }
                catch(Exception ex)
                {

                }
                
            }

            
            var result = new PagedList<spGetLogTable>()
            {
                Data = entity,
                TotalCount = 0
            };
            return result;
        }

        
    }
}
