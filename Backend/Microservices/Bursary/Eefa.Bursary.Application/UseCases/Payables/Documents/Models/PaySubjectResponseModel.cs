using AutoMapper;
using Eefa.Bursary.Domain.Entities.Payables;
using Eefa.Common;

namespace Eefa.Bursary.Application.UseCases.Payables.Documents.Models
{
    public class PaySubjectResponseModel : IMapFrom<Payables_Subjects_View>
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Payables_Subjects_View, PaySubjectResponseModel>();
        }

    }
}
