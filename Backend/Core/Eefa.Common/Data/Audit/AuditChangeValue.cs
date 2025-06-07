namespace Eefa.Common.Data
{
    public class AuditChangeValue
    {
        public AuditChangeValue(string title, string? old = null, string? @new = null, string? @description = null)
        {
            Title = title;
            Old = old;
            New = @new;
            Description = @description;
        }

        public string Title { get; set; }
        public string Description { get; set; }
        public string? Old { get; set; }
        public string? New { get; set; }
    }
    public class SpGetColumnsDescription
    {
        public string Table { get; set; }
        public string Column { get; set; }
        public string Description { get; set; }
    }
    public class SpGetColumnsDescriptionParam
    {
        public string TableName { get; set; }
        public string ColumnName { get; set; }
       
    }
}