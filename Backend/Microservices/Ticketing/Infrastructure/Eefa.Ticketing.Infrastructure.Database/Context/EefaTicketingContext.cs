using Eefa.Ticketing.Domain.Core.Entities.BaseInfo;
using Eefa.Ticketing.Domain.Core.Entities.Tickets;
using Eefa.Ticketing.Infrastructure.Database.Configurations.BaseInfo;
using Eefa.Ticketing.Infrastructure.Database.Configurations.Tickets;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Ticketing.Infrastructure.Database.Context
{
    public class EefaTicketingContext : DbContext
    {
        public EefaTicketingContext(DbContextOptions<EefaTicketingContext> options)
            : base(options)
        {
        }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketDetail> TicketDetails { get; set; }
        public DbSet<DetailHistory> DetailHistories { get; set; }
        public DbSet<PrivetMessage> PrivetMessages { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Person> Persons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("Ticketing");
            modelBuilder.ApplyConfiguration<Ticket>(new TicketConfiguration());
            modelBuilder.ApplyConfiguration<TicketDetail>(new TicketDetailConfiguration());
            modelBuilder.ApplyConfiguration<DetailHistory>(new DetailHistoryConfiguration());
            modelBuilder.ApplyConfiguration<PrivetMessage>(new PrivetMessageConfiguration());
            modelBuilder.ApplyConfiguration<Role>(new RoleConfiguration());
            modelBuilder.ApplyConfiguration<UserRole>(new UserRoleConfiguration());
            modelBuilder.ApplyConfiguration<User>(new UserConfiguration());
            modelBuilder.ApplyConfiguration<Person>(new PersonConfiguration());
        }
    }
}
