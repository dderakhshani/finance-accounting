using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;

public class TermsOfAccess
{
    public string Field { get; set; }
    public string Value { get; set; }
    public string Composition { get; set; }
    public string Oparation { get; set; }
}
public class CreatePermissionCommand : IRequest<ServiceResult<PermissionModel>>, IMapFrom<CreatePermissionCommand>
{
    public string LevelCode { get; set; } = default!;
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
        profile.CreateMap<CreatePermissionCommand, Permission>()
            .IgnoreAllNonExisting();
    }
}

public class CreatePermissionCommandHandler : IRequestHandler<CreatePermissionCommand, ServiceResult<PermissionModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreatePermissionCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<PermissionModel>> Handle(CreatePermissionCommand request, CancellationToken cancellationToken)
    {
        try
        {

            if (request.IsDataRowLimiter == false)
            {
                Permission entity = _mapper.Map<Permission>(request);

                _unitOfWork.Permissions.Add(entity);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return ServiceResult.Success(_mapper.Map<PermissionModel>(entity));
            }
            string condition = "1=1";
            string jsoncondition = "";
            if (request.AccessToAll == false)
            {
                condition = CreateCondition(request.TermsOfAccesses);
                jsoncondition = Newtonsoft.Json.JsonConvert.SerializeObject(request.TermsOfAccesses);
            }

            PermissionCondition permissionCondition = new PermissionCondition(request.TableName, condition, jsoncondition);
            Permission permission = new();
            permission = _mapper.Map<Permission>(request);
            permission.PermissionConditions = new();
            permission.PermissionConditions.Add(permissionCondition);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return ServiceResult.Success(_mapper.Map<PermissionModel>(permission));
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