using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eefa.Admin.Application.UseCases.Tables.Model
{
    public class TableModel
    {
        public string schema { get; set; }
        public string Table { get; set; }
        public string Column { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
    }
}
