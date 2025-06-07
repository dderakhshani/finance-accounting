using System.Collections.Generic;

namespace Eefa.Bursary.Application.Models
{
    public class SendDocument
    {
        public bool saveChanges { get; set; } = true;
        public int menueId { get; set; } = 0;
 
        public List<DataListModel> dataList { get; set; }
    }
}
