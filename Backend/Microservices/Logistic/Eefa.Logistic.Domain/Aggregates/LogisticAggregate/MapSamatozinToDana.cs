using System;
using Eefa.Common.Domain;

namespace Eefa.Logistic.Domain
{

    public partial class MapSamatozinToDana : DomainBaseEntity
    {
        public int      AccountReferenceId { get; set; } = default!;
        public string   AccountReferenceCode { get; set; } = default!;
        public string   SamaTozinCode { get; set; } = default!;
        public string   SamaTozinTitle { get; set; } = default!;

    }
}
