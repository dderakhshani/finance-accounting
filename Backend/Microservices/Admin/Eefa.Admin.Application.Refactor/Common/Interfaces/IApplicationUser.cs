
namespace Eefa.Admin.ApplicationRefector
{
    public interface IApplicationUser
    {
        public int Id { get; }
        public int RoleId { get; }
        public int CompanyId { get; }
        public int YearId { get; }
        public int LanguageId { get; }
        public string Username { get; }
        public string FullName { get; }
        public string Token { get; }
        public IoPath GetIoPaths();
    }
}

