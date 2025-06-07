using System;
using System.Collections.Generic;

namespace Eefa.Inventory.Application
{
    public class ListIdModel
    {
        public int state { get; set; }
        public string requestId { get; set; }
        public int menueId { get; set; }
        public List<int> ListIds { get; set; }
    }
}
