namespace Eefa.Ticketing.Domain.Core.Dtos.BaseInfo
{
    public class GetUserDataDto
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public string Username { get; set; }
        public string UnitPositionTitle { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
    }
}
