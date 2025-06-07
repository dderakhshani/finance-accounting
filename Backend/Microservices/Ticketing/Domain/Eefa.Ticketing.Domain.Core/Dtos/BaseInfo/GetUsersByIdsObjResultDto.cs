namespace Eefa.Ticketing.Domain.Core.Dtos.BaseInfo
{
    public class GetUsersByIdsObjResultDto
    {
        public int UserId { get; set; }
        public string Fullname { get; set; }
        public int RoleId { get; set; }
        public string RoleTitle { get; set; }
    }
}
