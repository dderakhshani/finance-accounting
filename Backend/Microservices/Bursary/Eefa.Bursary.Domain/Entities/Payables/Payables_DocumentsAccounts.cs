﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using Eefa.Bursary.Domain.Entities;
using Eefa.Bursary.Domain.Entities.Payables;
using Eefa.Common.Data;
using System;
using System.Collections.Generic;

namespace Eefa.Bursary.Domain.Entities
{
    public partial class Payables_DocumentsAccounts : BaseEntity
    {
        public int DocumentId { get; set; }
        public int ReferenceId { get; set; }
        public int? ReferenceGroupId { get; set; }
        public int? AccountHeadId { get; set; }
        public int? RexpId { get; set; }
        public string Descp { get; set; }
        public long Amount { get; set; }

        public virtual Payables_Documents Document { get; set; }
        public virtual AccountHead AccountHead { get; set; }
        public virtual AccountReferences Reference { get; set; }

    }
}