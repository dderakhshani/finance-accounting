using System;

namespace Eefa.Identity.Application.Services.Login.Model
{
    public class LoginModel 
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
        public int CompanyId { get; set; }
        public int YearId { get; set; }
        public string Ip { get; set; }

        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
    }
}
