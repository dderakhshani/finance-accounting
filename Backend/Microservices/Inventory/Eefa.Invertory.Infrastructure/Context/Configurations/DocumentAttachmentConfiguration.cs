﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Eefa.Inventory.Domain;
#nullable disable

namespace Eefa.Invertory.Infrastructure.Context.Configurations
{
    public partial class DocumentAttachmentConfiguration : IEntityTypeConfiguration<DocumentAttachment>
    {
        public void Configure(EntityTypeBuilder<DocumentAttachment> entity)
        {
            entity.ToTable("DocumentAttachments", "common");
            entity.Property(e => e.Id).HasComment("شناسه");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<DocumentAttachment> entity);
    }
}
