using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Admin.Application.CommandQueries.Permission.Command.Create;
using Eefa.Admin.Application.CommandQueries.Permission.Model;
using Eefa.Admin.Data.Databases.Entities;
using Eefa.Admin.Data.Databases.SqlServer.Context;
using Library.Common;
using Library.Interfaces;
using Library.Mappings;
using Library.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;


namespace Eefa.Admin.Application.CommandQueries.Permission.Command.Update
{
    public class UpdatePermissionCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<Data.Databases.Entities.Permission>, ICommand
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Title { get; set; } = default!;
        public string UniqueName { get; set; } = default!;
        public bool IsDataRowLimiter { get; set; } = default!;
        public string SubSystem { get; set; }
        public string TableName { get; set; }
        public bool AccessToAll { get; set; }
        public List<TermsOfAccess> TermsOfAccesses { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdatePermissionCommand, Data.Databases.Entities.Permission>()
                .IgnoreAllNonExisting();
        }
    }


    public class UpdatePermissionCommandHandler : IRequestHandler<UpdatePermissionCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;
        private readonly IAdminUnitOfWork _context;

        public UpdatePermissionCommandHandler(IRepository repository, IMapper mapper, IAdminUnitOfWork context)
        {
            _mapper = mapper;
            _repository = repository;
            _context = context;
        }

        public async Task<ServiceResult> Handle(UpdatePermissionCommand request, CancellationToken cancellationToken)
        {

            var entity = await _repository
                .Find<Data.Databases.Entities.Permission>(c =>
            c.ObjectId(request.Id)).Include(a => a.PermissionConditions)
            .FirstOrDefaultAsync(cancellationToken);
            //var entity = await _context.Permissions
            //	.Include(a => a.PermissionConditions)
            //	.Where(a => a.Id == request.Id)
            //.FirstOrDefaultAsync(cancellationToken);
            string condition = "1=1";
            string jsoncondition = "";
            if (request.AccessToAll == false)
            {
                condition = CreateCondition(request.TermsOfAccesses);
                jsoncondition = Newtonsoft.Json.JsonConvert.SerializeObject(request.TermsOfAccesses);
            }
            entity.Title = request.Title;
            entity.UniqueName = request.UniqueName;
            entity.ParentId = request.ParentId;
            entity.IsDataRowLimiter = request.IsDataRowLimiter;
            entity.SubSystem = request.SubSystem;
            entity.AccessToAll = request.AccessToAll;
            if (entity.PermissionConditions == null || entity.PermissionConditions.Count() == 0)
            {
                PermissionCondition permissionCondition = new PermissionCondition(request.TableName, condition, jsoncondition);
                entity.PermissionConditions = new();
                entity.PermissionConditions.Add(permissionCondition);
            }
            else
            {
                entity.PermissionConditions.First().Condition = condition;
                entity.PermissionConditions.First().JsonCondition = jsoncondition;
            }
            var updateEntity = _repository.Update(entity);
            return await request.Save<Data.Databases.Entities.Permission, PermissionModel>(_repository, _mapper, updateEntity.Entity, cancellationToken);

            //_context.Permissions.Update(entity);
            //await _context.SaveAsync(cancellationToken);

            //return ServiceResult.Set(_mapper.Map<PermissionModel>(entity));
            //await request.Save<Data.Databases.Entities.Permission, PermissionModel>(_repository, _mapper, updateEntity.Entity, cancellationToken);

        }

        public string CreateCondition(List<TermsOfAccess> queries)
        {
            string @where = " ";

            foreach (var condition in queries)
            {
                bool isInt = false;
                try
                {
                    int value = Convert.ToInt32(condition.Value);
                    isInt = true;
                }
                catch (Exception ex)
                {
                    isInt = false;
                }
                if (isInt)
                {
                    @where = IntCondition(condition, @where);
                }
                else
                {
                    @where = StringCondition(condition, @where);
                }
            }
            @where = @where.Trim();

            if (@where.EndsWith("or") || @where.EndsWith("and"))
            {
                var charsToRemove = @where.EndsWith("or") ? 3 : 4;
                @where = @where.Remove(@where.Length - charsToRemove, charsToRemove);
            }

            return where;
        }

        private string IntCondition(TermsOfAccess condition, string @where)
        {
            if (condition.Oparation == "equal")
            {
                @where += $"t.{condition.Field} = {condition.Value} {condition.Composition} ";
            }
            if (condition.Oparation == "notEqual")
            {
                @where += $"t.{condition.Field} <> {condition.Value} {condition.Composition} ";
            }
            if (condition.Oparation == "greaterThan")
            {
                @where += $"t.{condition.Field} > {condition.Value} {condition.Composition} ";
            }
            if (condition.Oparation == "lessThan")
            {
                @where += $"t.{condition.Field} < {condition.Value} {condition.Composition} ";
            }

            if (condition.Oparation == "startsWith")
            {
                @where += $"t.{condition.Field} like N'{condition.Value}%' {condition.Composition} ";
            }
            else if (condition.Oparation == "endsWith")
            {
                @where += $"t.{condition.Field} like N'%{condition.Value}' {condition.Composition} ";
            }

            else if (condition.Oparation == "contains")
            {
                @where += $"t.{condition.Field} like N'%{condition.Value}%' {condition.Composition} ";
            }

            else if (condition.Oparation == "notContains")
            {
                @where += $"t.{condition.Field} NOT like N'%{condition.Value}%' {condition.Composition} ";
            }
            return @where;
        }
        private string StringCondition(TermsOfAccess condition, string @where)
        {

            if (condition.Oparation == "equal")
            {
                if (condition.Value.ToLower() != "null")
                    @where += $"t.{condition.Field} = N'{condition.Value}' {condition.Composition} ";
                else
                    @where += $"t.{condition.Field} is null {condition.Composition} ";
            }
            if (condition.Oparation == "notEqual")
            {
                if (condition.Value.ToLower() != "null")
                    @where += $"t.{condition.Field} <> N'{condition.Value}' {condition.Composition} ";
                else
                    @where += $"t.{condition.Field} is not null {condition.Composition} ";
            }
            if (condition.Oparation == "greaterThan")
            {
                @where += $"t.{condition.Field} > N'{condition.Value}' {condition.Composition} ";
            }
            if (condition.Oparation == "lessThan")
            {
                @where += $"t.{condition.Field} < N'{condition.Value}' {condition.Composition} ";
            }

            if (condition.Oparation == "startsWith")
            {
                @where += $"t.{condition.Field} like N'{condition.Value}%' {condition.Composition} ";
            }
            else if (condition.Oparation == "endsWith")
            {
                @where += $"t.{condition.Field} like N'%{condition.Value}' {condition.Composition} ";
            }

            else if (condition.Oparation == "contains")
            {
                @where += $"t.{condition.Field} like N'%{condition.Value}%' {condition.Composition} ";
            }

            else if (condition.Oparation == "notContains")
            {
                @where += $"t.{condition.Field} NOT like N'%{condition.Value}%' {condition.Composition} ";
            }
            return @where;
        }
    }
}
