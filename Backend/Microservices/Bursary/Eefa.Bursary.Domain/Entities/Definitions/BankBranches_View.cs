using Eefa.Common.Data;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eefa.Bursary.Domain.Entities.Definitions
{
    [Table("BankBranches_View", Schema="bursary")]
    public class BankBranches_View : BaseEntity
    {
        public string? Code { get; set; }
        public string? Title { get; set; }
        public int? BankId { get; set; }
        public string? BankCode { get; set; }
        public string? BankTitle { get; set; }
        public int? CountryDivisionId { get; set; }
        public string? CountryDivisionTitle { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? ManagerFullName { get; set; }

    }
}
