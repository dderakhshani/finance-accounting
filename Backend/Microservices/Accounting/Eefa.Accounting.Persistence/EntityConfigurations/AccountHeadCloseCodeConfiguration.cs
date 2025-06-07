using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eefa.Accounting.Domain.Entities;

namespace Eefa.Accounting.Persistence.EntityConfigurations
{
    partial class AccountHeadCloseCodeConfiguration : IEntityTypeConfiguration<AccountHeadCloseCode>
    {

        public void Configure(EntityTypeBuilder<AccountHeadCloseCode> entity)
        {
            entity.ToTable("AccountHeadCloseCode", "accounting");
 
            entity.HasOne(d => d.CreatedBy)
                .WithMany(p => p.AccountHeadCloseCodeCreatedBies)
                .HasForeignKey(d => d.CreatedById)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.ModifiedBy)
                .WithMany(p => p.AccountHeadCloseCodeModifiedBies)
                .HasForeignKey(d => d.ModifiedById);

            entity.HasOne(d => d.OwnerRole)
                .WithMany(p => p.AccountHeadCloseCode)
                .HasForeignKey(d => d.OwnerRoleId)
                .OnDelete(DeleteBehavior.ClientSetNull);
             
            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<AccountHeadCloseCode> entity);
    }
 
}
