using Microsoft.AspNetCore.Http;
using System.Linq;

public class ApplicationUserService :  IApplicationUser
{
    private readonly IHttpContextAccessor httpContextAccessor;

    public ApplicationUserService(IHttpContextAccessor httpContextAccessor)
    {
        this.httpContextAccessor = httpContextAccessor;
    }
    public int Id => int.Parse(httpContextAccessor?.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == "id")?.Value);
    public int RoleId => int.Parse(httpContextAccessor?.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == "roleId")?.Value);
    public int CompanyId => int.Parse(httpContextAccessor?.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == "companyId")?.Value);
    public int YearId => int.Parse(httpContextAccessor?.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == "yearId")?.Value);
    public int LanguageId => int.Parse(httpContextAccessor?.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == "languageId")?.Value);
    public string Username => httpContextAccessor?.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == "username")?.Value;
    public string FullName => httpContextAccessor?.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == "fullName")?.Value;
    public string Token => httpContextAccessor?.HttpContext?.Request.Headers["Authorization"];
}

