namespace Library.Models
{
    public class ChangedProperty
    {
        public ChangedProperty(string title, string? old = null, string? @new = null)
        {
            Title = title;
            Old = old;
            New = @new;
        }

        public string Title { get; set; }
        public string? Old { get; set; }
        public string? New { get; set; }
    }
}