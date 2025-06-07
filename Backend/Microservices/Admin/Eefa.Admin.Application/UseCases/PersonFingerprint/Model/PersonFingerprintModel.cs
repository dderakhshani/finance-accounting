using AutoMapper;
using Library.Mappings;

namespace Eefa.Admin.Application.CommandQueries.PersonFingerprint.Model
{
    public class PersonFingerprintModel : IMapFrom<Data.Databases.Entities.PersonFingerprint>
    {
        public int Id { get; set; }
        /// <summary>
        /// کد پرسنلی
        /// </summary>
        public int PersonId { get; set; } = default!;

        /// <summary>
        /// شماره انگشت
        /// </summary>
        public int FingerBaseId { get; set; } = default!;
        public string FingerPrintPhotoURL { get; set; }


        /// <summary>
        /// الگوی اثر انگشت
        /// </summary>
        public string FingerprintTemplate { get; set; } = default!;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Data.Databases.Entities.PersonFingerprint, PersonFingerprintModel>().IgnoreAllNonExisting();
        }
    }

}
