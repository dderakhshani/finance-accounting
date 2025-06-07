namespace TaxCollectData.Library.Abstraction;

public interface ITaxIdGenerator
{
    string GenerateTaxId(string memoryId, long serial, DateTime createDate);
}