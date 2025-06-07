namespace Eefa.Ticketing.Application.Contract.Dtos.BasicInfos
{
    public class GetRoleById
    {
        public bool Succeed { get; set; }
        public GetRoleByIdObjResult objResult { get; set; }
    }
    public class GetRoleByIdObjResult
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }
}
