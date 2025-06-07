using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Library.Common;
using Library.Models;

namespace Eefa.Admin.Data.Databases.Entities
{
    [Table(name: "Persons", Schema = "admin")]
    public partial class Person : IAuditable
    {
        public List<AuditMapRule> Audit()
        {
            return new List<AuditMapRule>()
            {
                new AuditMapRule<Entities.Person, AccountReference>(x => x.AccountReferenceId, x => x.Title, x => x.Id),
                new AuditMapRule<Entities.Person, BaseValue>(x => x.GovernmentalBaseId, x => x.Title, x => x.Id),
                new AuditMapRule<Entities.Person, BaseValue>(x => x.GenderBaseId, x => x.Title, x => x.Id),
                new AuditMapRule<Entities.Person, CountryDivision>(x => x.BirthPlaceCountryDivisionId, x => x.OstanTitle + "-" + x.ShahrestanTitle, x => x.Id),
                new AuditMapRule<Entities.Person, BaseValue>(x => x.LegalBaseId, x => x.Title, x => x.Id),
            };
        }
    }
}