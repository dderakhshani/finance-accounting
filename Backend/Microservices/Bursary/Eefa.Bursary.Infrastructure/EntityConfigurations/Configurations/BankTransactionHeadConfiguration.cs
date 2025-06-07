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
    public partial class BankTransactionHeadConfiguration : IEntityTypeConfiguration<BankTransactionHead>
    {
        public void Configure(EntityTypeBuilder<BankTransactionHead> entity)
        {
            entity.ToTable("BankTransactionHead", "bursary");

            entity.HasOne(d => d.CreatedBy)
                .WithMany(p => p.BankTransactionHeadsCreatedBies)
                .HasForeignKey(d => d.CreatedById)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.ModifiedBy)
                .WithMany(p => p.BankTransactionHeadsModifiedBies)
                .HasForeignKey(d => d.ModifiedById);

            entity.HasOne(d => d.OwnerRole)
                .WithMany(p => p.BankTransactionHeadOwnerRoles)
                .HasForeignKey(d => d.OwnerRoleId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<BankTransactionHead> entity);


    }
}
