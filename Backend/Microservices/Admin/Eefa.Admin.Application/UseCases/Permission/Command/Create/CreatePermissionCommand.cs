using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Admin.Data.Databases.Entities;
using Library.Common;
using Library.Interfaces;
using Library.Mappings;
using Library.Models;
using MediatR;


namespace Eefa.Admin.Application.CommandQueries.Permission.Command.Create
{
    public class TermsOfAccess
    {
        public string Field { get; set; }
        public string Value { get; set; }
        public string Composition { get; set; }
        public string Oparation { get; set; }
    }
    public class CreatePermissionCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<CreatePermissionCommand>, ICommand
    {

        /// <summary>
        /// کد سطح
        /// </summary>
        public string LevelCode { get; set; } = default!;

        /// <summary>
        /// کد والد
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        /// عنوان
        /// </summary>
        public string Title { get; set; } = default!;

        /// <summary>
        /// نام اختصاصی
        /// </summary>
        public string UniqueName { get; set; } = default!;
        public bool IsDataRowLimiter { get; set; } = default!;
        public string SubSystem { get; set; }
        public string TableName { get; set; }
        public bool AccessToAll { get; set; }
        public List<TermsOfAccess> TermsOfAccesses { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreatePermissionCommand, Data.Databases.Entities.Permission>()
                .IgnoreAllNonExisting();
        }
    }

    public class CreatePermissionCommandHandler : IRequestHandler<CreatePermissionCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public CreatePermissionCommandHandler(IRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }


        public async Task<ServiceResult> Handle(CreatePermissionCommand request, CancellationToken cancellationToken)
        {
            try
            {

                if (request.IsDataRowLimiter == false)
                {
                    var entity = _repository.Insert(_mapper.Map<Data.Databases.Entities.Permission>(request));
                    return await request.Save(_repository, entity.Entity, cancellationToken);
                }
                string condition = "1=1";
                string jsoncondition = "";
                if (request.AccessToAll == false)
                {
                    condition = CreateCondition(request.TermsOfAccesses);
                    jsoncondition = Newtonsoft.Json.JsonConvert.SerializeObject(request.TermsOfAccesses);
                }

                PermissionCondition permissionCondition = new PermissionCondition(request.TableName, condition, jsoncondition);
                Data.Databases.Entities.Permission permission = new();
                permission = _mapper.Map<Data.Databases.Entities.Permission>(request);
                permission.PermissionConditions = new();
                permission.PermissionConditions.Add(permissionCondition);
                return await request.Save(_repository, _repository.Insert(permission).Entity, cancellationToken);

            }
            catch (System.Exception ex)
            {

                throw;
            }
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
