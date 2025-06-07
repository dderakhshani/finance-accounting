using Library.Interfaces;

namespace Library.ConfigurationAccessor.Models
{
    public class ConnectionStringModel: IConnectionStringModel
    {
        public string DefaultString { get; set; }
    }
}