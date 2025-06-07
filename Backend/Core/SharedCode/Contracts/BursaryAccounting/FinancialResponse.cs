using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedCode.Contracts.BursaryAccounting
{
    public class FinancialResponse
    {
        public object ObjResult { get; set; }

        public string Message { get; set; }

        public List<ApplicationErrorModel> Errors { get; set; }

        public bool Succeed { get; set; } = false;
    }

    public class ApplicationErrorModel
    {

        public string Source { get; set; }

        public string PropertyName { get; set; }

        public string Message { get; set; }
    }

}
