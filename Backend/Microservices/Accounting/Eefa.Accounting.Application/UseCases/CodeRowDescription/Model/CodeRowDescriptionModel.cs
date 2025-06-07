using AutoMapper;
using Library.Mappings;

namespace Eefa.Accounting.Application.UseCases.CodeRowDescription.Model
{
    public class CodeRowDescriptionModel : IMapFrom<Data.Entities.CodeRowDescription>
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string Title { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Data.Entities.CodeRowDescription, CodeRowDescriptionModel>();
        }
    }

}
