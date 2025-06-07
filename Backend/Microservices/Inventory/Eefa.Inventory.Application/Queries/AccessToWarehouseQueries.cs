using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;
using Microsoft.EntityFrameworkCore;
using Eefa.Inventory.Domain;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Eefa.Inventory.Application
{
    public class AccessToWarehouseQueries : IAccessToWarehouseQueries
    {

        private readonly IMapper _mapper;
        private readonly IInvertoryUnitOfWork _context;
        private readonly IProcedureCallService _IProcedureCallService;

        public AccessToWarehouseQueries(
              IMapper mapper
            , IInvertoryUnitOfWork context
            , IProcedureCallService IProcedureCallService
            )
        {
            _mapper = mapper;
            _context = context;
            _IProcedureCallService = IProcedureCallService;

        }
        public Task<PagedList<SpGetAudit>> GetAll(PaginatedQueryModel query)
        {
           
            throw new NotImplementedException();
        }
        [HttpGet]
        public async Task<PagedList<SpGetAudit>> GetById(int PrimaryId, string TableName)
        {
           
            var entity = await _IProcedureCallService.GetAuditById(PrimaryId, TableName);
            var result = new PagedList<SpGetAudit>()
            {
                Data = entity,
                TotalCount = 0
            };
            return result;
        }

        public Task<PagedList<SpGetAudit>> GetById(int Id)
        {
            throw new NotImplementedException();
        }
        public async Task<List<int>> GetUserId(int UserId,string TableName)
        {
            var result = await _context.AccessToWarehouse.Where(a=>a.UserId== UserId && a.TableName== TableName && !a.IsDeleted).Select(a =>a.WarehouseId)
            .ToListAsync();
           
            return result;
        }
        public async Task<PagedList<UserModel>> GetUsers(PaginatedQueryModel query)
        {
            var entity = from user in _context.User
                         join person in _context.Persons on user.PersonId equals person.Id
                         select new UserModel()
                         {
                             Id=user.Id,
                             Title= person.FirstName + " " + person.LastName + " 🔅 "+ user.Username
                         };
            var result = new PagedList<UserModel>()
            {
                Data = await entity.ToListAsync(),
                TotalCount = 0
            };
            return result;
        }
    }
}
