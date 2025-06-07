using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Library.Common;
using Library.Models;

namespace Eefa.Accounting.Data.Entities
{
    [Table(name: "VouchersDetail", Schema = "accounting")]

    public partial class VouchersDetail : IAuditable
    {
        public List<AuditMapRule> Audit()
        {
            return new List<AuditMapRule>()
            {
                new AuditMapRule<Eefa.Accounting.Data.Entities.VouchersDetail, VouchersHead>(x => x.VoucherId, x => x.VoucherNo.ToString(), x => x.Id),
                new AuditMapRule<Eefa.Accounting.Data.Entities.VouchersDetail, AccountReferencesGroup>(x => x.AccountReferencesGroupId, x => x.Title, x => x.Id),
                new AuditMapRule<Eefa.Accounting.Data.Entities.VouchersDetail, AccountReference>(x => x.ReferenceId1, x => x.Title, x => x.Id),
                new AuditMapRule<Eefa.Accounting.Data.Entities.VouchersDetail, AccountReference>(x => x.ReferenceId2, x => x.Title, x => x.Id),
                new AuditMapRule<Eefa.Accounting.Data.Entities.VouchersDetail, AccountReference>(x => x.ReferenceId3, x => x.Title, x => x.Id),
                new AuditMapRule<Eefa.Accounting.Data.Entities.VouchersDetail, BaseValue>(x => x.CurrencyTypeBaseId, x => x.Title, x => x.Id),

            };
        }
    }
}
