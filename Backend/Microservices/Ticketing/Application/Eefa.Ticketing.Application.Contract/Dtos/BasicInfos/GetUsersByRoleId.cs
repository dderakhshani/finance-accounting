using System.Collections.Generic;

namespace Eefa.Ticketing.Application.Contract.Dtos.BasicInfos
{
    public class GetUsersByRoleIdInput
    {
        public GetUsersByRoleIdInput(int pageIndex, int pageSize, int roleId)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            RoleId = roleId;
        }
        public int RoleId { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
    }
    public class GetUsersByRoleIdResult
    {
        public bool succeed { get; set; }
        public GetUserObjResult objResult { get; set; }

    }
    public class GetUserObjResult
    {
        public int totalCount { get; set; }
        public List<GetUserData> data { get; set; }
    }
    public class GetUserData
    {

        public int Id { get; set; }
        public int PersonId { get; set; } = default!;
        public string Username { get; set; } = default!;
        public string UnitPositionTitle { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
    }
}
