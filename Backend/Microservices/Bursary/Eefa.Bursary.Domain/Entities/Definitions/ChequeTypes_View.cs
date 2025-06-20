﻿using Eefa.Common.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eefa.Bursary.Domain.Entities.Definitions
{
    [Table("ChequeTypes_View", Schema ="bursary")]
    public class ChequeTypes_View : BaseEntity
    {
        public string Code { get; set; }
        public string Title { get; set; }
    }
}
