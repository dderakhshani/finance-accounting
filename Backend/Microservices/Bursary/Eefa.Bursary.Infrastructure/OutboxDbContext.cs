using Eefa.Bursary.Application.Interfaces;
using Eefa.Bursary.Domain.OutboxEntities;
using Eefa.Bursary.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eefa.Bursary.Infrastructure
{
    public class OutboxDbContext : DbContext 
    {
        public DbSet<OutboxMessages> OutboxMessages { get; set; }

        public OutboxDbContext(DbContextOptions<OutboxDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OutboxMessages>(entity =>
            {
                entity.HasKey(e => e.Id); 
                entity.Property(e => e.Id).ValueGeneratedOnAdd(); 
            });
            modelBuilder.Entity<OutboxMessages>().Property(o => o.VoucherHeadId).IsRequired(false);
            modelBuilder.Entity<OutboxMessages>().Property(o => o.IsBursaryUpdated).HasDefaultValue(false);
        }
    }

}
