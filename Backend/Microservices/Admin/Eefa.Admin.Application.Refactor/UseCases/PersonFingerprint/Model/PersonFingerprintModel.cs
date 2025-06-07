using AutoMapper;

public class PersonFingerprintModel : IMapFrom<PersonFingerprint>
{
    public int Id { get; set; }
    public int PersonId { get; set; } = default!;
    public int FingerBaseId { get; set; } = default!;
    public string FingerPrintPhotoURL { get; set; }
    public string FingerprintTemplate { get; set; } = default!;


    public void Mapping(Profile profile)
    {
        profile.CreateMap<PersonFingerprint, PersonFingerprintModel>().IgnoreAllNonExisting();
    }
}