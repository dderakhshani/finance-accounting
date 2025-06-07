using Eefa.Bursary.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eefa.Bursary.Infrastructure.EntityConfigurations.Configurations
{
    public partial class BankTransactionsConfiguration : IEntityTypeConfiguration<BankTransactions>
    {
        public void Configure(EntityTypeBuilder<BankTransactions> entity)
        {
            entity.ToTable("BankTransactions", "bursary");

            entity.HasOne(d => d.CreatedBy)
    .WithMany(p => p.BankTransactionsCreatedBies)
    .HasForeignKey(d => d.CreatedById)
    .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.ModifiedBy)
                .WithMany(p => p.BankTransactionsModifiedBies)
                .HasForeignKey(d => d.ModifiedById);

            entity.HasOne(d => d.OwnerRole)
                .WithMany(p => p.BankTransactionsOwnerRoles)
                .HasForeignKey(d => d.OwnerRoleId)
                .OnDelete(DeleteBehavior.ClientSetNull);

        }
        partial void OnConfigurePartial(EntityTypeBuilder<BankTransactions> entity);

    }
}
