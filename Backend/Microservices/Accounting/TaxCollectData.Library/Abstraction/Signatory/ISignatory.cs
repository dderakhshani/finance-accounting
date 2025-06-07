namespace TaxCollectData.Library.Abstraction.Signatory;

public interface ISignatory
{
    string Sign(string data);

    string GetKeyId();
}