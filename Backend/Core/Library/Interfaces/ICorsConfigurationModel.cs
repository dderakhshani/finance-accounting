namespace Library.Interfaces
{
    public interface ICorsConfigurationModel
    {
        public string PolicyName { get; set; }
        public bool AllowAnyOrigin { get; set; }
        public bool AllowAnyMethod { get; set; }
        public bool AllowAnyHeader { get; set; }
    }
}