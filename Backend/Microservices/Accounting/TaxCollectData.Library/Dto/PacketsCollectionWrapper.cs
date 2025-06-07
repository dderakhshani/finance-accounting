namespace TaxCollectData.Library.Dto;

public record PacketsCollectionWrapper
{
    public IEnumerable<object> Packets { get; }

    public PacketsCollectionWrapper(IEnumerable<object> packets)
    {
        Packets = packets;
    }
};