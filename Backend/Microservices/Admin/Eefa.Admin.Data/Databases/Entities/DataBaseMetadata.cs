using Library.Common;

#nullable disable

namespace Eefa.Admin.Data.Databases.Entities
{

    public partial class DataBaseMetadata : BaseEntity
    {
        public string TableName { get; set; }
        public string SchemaName { get; set; }
        public string Command { get; set; }
        public string CommandProperty { get; set; }
        public string Property { get; set; }
        public int? LanguageId { get; set; }
        public string Translated { get; set; }
    }
}
