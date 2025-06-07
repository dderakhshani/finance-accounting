using System.Collections.Generic;

namespace Eefa.Ticketing.Application.Contract.Dtos.BasicInfos
{
    public class GetUsersByIdsInput
    {
        public GetUsersByIdsInput(List<int> userIds)
        {
            UserIds = userIds;
        }
        public List<int> UserIds { get; set; }
    }
    public class GetUsersByIdsResult
    {
        public bool succeed { get; set; }
        public List<GetUsersByIdsObjResult> objResult { get; set; }
    }
    public class GetUsersByIdsObjResult
    {
        public int UserId { get; set; }
        public string Fullname { get; set; }
        public int RoleId { get; set; }
        public string RoleTitle { get; set; }
    }
}
