﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Library.Common;
using Library.Models;

namespace Eefa.Admin.Data.Databases.Entities
{
    [Table(name: "AccountReferencesRelReferencesGroups", Schema = "accounting")]

    public partial class AccountReferencesRelReferencesGroup : IAuditable
    {
        public List<AuditMapRule> Audit()
        {
            return new List<AuditMapRule>();
        }
    }
}
