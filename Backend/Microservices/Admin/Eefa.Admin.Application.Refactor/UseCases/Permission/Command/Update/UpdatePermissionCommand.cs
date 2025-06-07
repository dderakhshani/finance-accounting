using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using System;
using System.Linq;

public class UpdatePermissionCommand : IRequest<ServiceResult<PermissionModel>>, IMapFrom<Permission>
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
        profile.CreateMap<UpdatePermissionCommand, Permission>()
            .IgnoreAllNonExisting();
    }
}


public class UpdatePermissionCommandHandler : IRequestHandler<UpdatePermissionCommand, ServiceResult<PermissionModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    //private readonly IAdminUnitOfWork _context;

    public UpdatePermissionCommandHandler(IUnitOfWork unitOfWork, IMapper mapper/*, IAdminUnitOfWork context*/)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        //_context = context;
    }

    public async Task<ServiceResult<PermissionModel>> Handle(UpdatePermissionCommand request, CancellationToken cancellationToken)
    {
        Permission entity = await _unitOfWork.Permissions.GetByIdAsync(request.Id);
        
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
        _unitOfWork.Permissions.Update(entity);
        return ServiceResult.Success(_mapper.Map<PermissionModel>(entity));

        //_context.Permissions.Update(entity);
        //await _context.SaveAsync(cancellationToken);

        //return ServiceResult.Set(_mapper.Map<PermissionModel>(entity));
        //await request.Save<Permission, PermissionModel>(_repository, _mapper, updateEntity.Entity, cancellationToken);

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
            @where += $"t.{condition.Field} = N'{condition.Value}' {condition.Composition} ";
        }
        if (condition.Oparation == "notEqual")
        {
            @where += $"t.{condition.Field} <> N'{condition.Value}' {condition.Composition} ";
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