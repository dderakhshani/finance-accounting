
using Eefa.Bursary.Domain.Entities.Definitions;
using Eefa.Common;


namespace Eefa.Bursary.Application.Queries.Bank
{
    public record BankModel : IMapFrom<Banks>
    {
        public int Id { get; set; }
        public string Code { get; set; } = default!;
 
        public string Title { get; set; } = default!;
 
        public string? GlobalCode { get; set; }
 
        public int TypeBaseId { get; set; } = default!;
 
        public string? SwiftCode { get; set; }
 
        public string? ManagerFullName { get; set; }
 
        public string? Descriptions { get; set; }
 
        public string? TelephoneJson { get; set; }



    }



}
