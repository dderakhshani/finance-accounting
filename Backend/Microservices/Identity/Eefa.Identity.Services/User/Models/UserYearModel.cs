using System;

namespace Eefa.Identity.Services.User.Models
{
    public class UserYearModel
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int YearName { get; set; }
        public DateTime FirstDate { get; set; }
        public DateTime LastDate  { get; set; }
        public DateTime? LastEditableDate { get; set; }
        public bool IsCurrentYear { get; set; }
        public bool? IsEditable { get; set; }
    }
}