using Eefa.Common;
using Eefa.Logistic.Domain;

namespace Eefa.Logistic.Application
{
    public class MapSamatozinToDanaModel 
    {
        public int Id { get; set; } = default!;
       
        
        public string   AccountReferencesTitle { get; set; } = default!;
        public string   AccountReferencesGroupsCode { get; set; } = default!;
        public int      AccountReferenceGroupId { get; set; } = default!;
        public int?     AccountReferenceGoupCodeId { get; set; } = default!;
        

        public int AccountReferenceId { get; set; } = default!;
        public string AccountReferenceCode { get; set; } = default!;

        public string SamaTozinCode { get; set; } = default!;
        public string SamaTozinTitle { get; set; } = default!;

    }
}
