using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Eefa.Bursary.Infrastructure
{
    public class OutboxDbContextFactory : IDesignTimeDbContextFactory<OutboxDbContext>
    {
        public OutboxDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<OutboxDbContext>();
 
            builder.UseSqlite("Data Source=outbox.db");

            return new OutboxDbContext(builder.Options);
        }
    }
}
