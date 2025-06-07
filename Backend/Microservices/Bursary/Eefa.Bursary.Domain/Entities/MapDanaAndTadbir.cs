using Eefa.Common.Data;

namespace Eefa.Bursary.Domain.Entities
{
    public class MapDanaAndTadbir : BaseEntity
    {
      public int DanaID { get; set; }
      public string DanaCode { get; set; }
      public string TadbirCode { get; set; }
    }
}
