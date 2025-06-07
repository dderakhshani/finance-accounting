using AutoMapper;
using Eefa.Common;
using Eefa.Purchase.Domain.Entities;

namespace Eefa.Purchase.Application.Models
{
    public class InvoiceALLStatusModel : IMapFrom<CodeVoucherGroup>
    {
        public int Id { get; set; } = default!;
        public string Code { get; set; } = default!;
        public string Title { get; set; } = default!;
        public string UniqueName { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<CodeVoucherGroup, InvoiceALLStatusModel>();
        }
    }
}
