using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Library.Common;
using Library.Models;

// ReSharper disable once CheckNamespace
namespace Eefa.Accounting.Data.Entities
{
    [Table(name: "VouchersHeads", Schema = "accounting")]
    public partial class VouchersHead : IAuditable
    {
        public List<AuditMapRule> Audit()
        {
            return new List<AuditMapRule>()
            {
                new AuditMapRule<Eefa.Accounting.Data.Entities.VouchersHead, Year>(x => x.YearId, x => x.YearName.ToString(), x => x.Id),
                new AuditMapRule<Eefa.Accounting.Data.Entities.VouchersHead, CompanyInformation>(x => x.CompanyId, x => x.Title, x => x.Id),
                new AuditMapRule<Eefa.Accounting.Data.Entities.VouchersHead, CodeVoucherGroup>(x => x.CodeVoucherGroupId, x => x.Title, x => x.Id),
            };
        }
    }
}