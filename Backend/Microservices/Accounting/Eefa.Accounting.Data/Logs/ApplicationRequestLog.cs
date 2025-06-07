using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eefa.Accounting.Data.Entities;

namespace Eefa.Accounting.Data.Logs
{
    public class ApplicationRequestLog
    {
        public Guid Id { get; set; }
        public string RequestJSON { get; set; }
        public string RequestType { get; set; }
        public string ResponseJSON { get; set; }
        public int Status { get; set; }
        public string ProjectTitle { get; set; }

        public DateTime CreatedAt { get; set; }
        public int CreatedById { get; set; }

        public User CreatedBy { get; set; }


    }
}
