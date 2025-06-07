using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eefa.Identity.Services.User.Models
{
    public class UserProfileModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public List<UserRoleModel> Roles { get; set; }
        public List<UserYearModel> Years { get; set; }
        public List<UserMenuModel> Menus { get; set; }
        public List<UserPermissionModel> Permissions { get; set; }
        public List<UserCompanyModel> Companies { get; set; }
    }
}
