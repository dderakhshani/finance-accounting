using AutoMapper;
using Library.Mappings;

namespace Eefa.Accounting.Application.UseCases.CodeVoucherExtendType.Model
{
    public class CodeVoucherExtendTypeModel : IMapFrom<Data.Entities.CodeVoucherExtendType>
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Data.Entities.CodeVoucherExtendType, CodeVoucherExtendTypeModel>().IgnoreAllNonExisting();
        }
    }

}
