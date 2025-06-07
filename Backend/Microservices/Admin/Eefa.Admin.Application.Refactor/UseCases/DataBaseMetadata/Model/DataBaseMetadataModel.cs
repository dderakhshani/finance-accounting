using AutoMapper;

public class DataBaseMetadataModel : IMapFrom<DataBaseMetadata>
{
    public int Id { get; set; }
    public string TableName { get; set; }
    public string SchemaName { get; set; }
    public string Command { get; set; }
    public string CommandProperty { get; set; }
    public string Property { get; set; }
    public int? LanguageId { get; set; }
    public string Translated { get; set; }


    public void Mapping(Profile profile)
    {
        profile.CreateMap<DataBaseMetadata, DataBaseMetadataModel>();
    }
}