using Eefa.NotificationServices.Common.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eefa.NotificationServices.Data
{
    public class MessageDbContext : DbContext
    {
        public DbSet<Message> Messages { get; set; }

        public IConfiguration _configuration;        
        public MessageDbContext(DbContextOptions options, IConfiguration configuration = default)  
        {
            _configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("MessageConnectionString"));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {           
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");
            modelBuilder.ApplyConfiguration(new MessageConfiguration());           
        }

    }

}
