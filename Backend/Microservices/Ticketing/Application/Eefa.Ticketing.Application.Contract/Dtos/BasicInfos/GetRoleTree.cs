using System.Collections.Generic;

namespace Eefa.Ticketing.Application.Contract.Dtos.BasicInfos
{
    public class GetRoleTree
    {
        public bool Succeed { get; set; }
        public List<ObjResultRoleTree> objResult { get; set; }
    }
    public class ObjResultRoleTree
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }
}
