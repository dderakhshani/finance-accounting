using System.Collections.Generic;

namespace Eefa.Ticketing.Application.Contract.Dtos.BasicInfos
{
    public class RoleAllInput
    {
        public RoleAllInput(int pageIndex,int pageSize)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
        }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
    }
    public class RoleAllResult
    {
        public bool succeed { get; set; }
        public ObjResult objResult { get; set; }
    }
    public class ObjResult
    {
        public int totalCount { get; set; }
        public List<Data> data { get; set; }
    }
    public class Data
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string Title { get; set; }
        public string LevelCode { get; set; }
    }
}
