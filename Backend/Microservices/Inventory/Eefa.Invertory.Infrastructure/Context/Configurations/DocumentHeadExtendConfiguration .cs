﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using Eefa.Invertory.Infrastructure.Context;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using System;
using Eefa.Inventory.Domain;


#nullable disable

namespace Eefa.Invertory.Infrastructure.Context.Configurations
{
    public partial class DocumentHeadExtendConfiguration : IEntityTypeConfiguration<DocumentHeadExtend>
    {
        public void Configure(EntityTypeBuilder<DocumentHeadExtend> entity)
        {
            entity.ToTable("DocumentHeadExtend", "common");

            entity.HasComment("انبارها");

            entity.Property(e => e.Id).HasComment("شناسه");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان ایجاد");

            entity.Property(e => e.CreatedById).HasComment("ایجاد کننده");

            entity.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان اصلاح");

            entity.Property(e => e.ModifiedById).HasComment("اصلاح کننده");

            entity.Property(e => e.OwnerRoleId).HasComment("نقش صاحب سند");

            //entity.HasOne(d => d.RequesterReference)
            //    .WithMany(p => p.RequesterDocumentHeadExtend)
            //    .HasForeignKey(d => d.RequesterReferenceId)
            //    .HasConstraintName("FK_DocumentHeadExtend_AccountReferences_Requster");

            //entity.HasOne(d => d.FollowUpReference)
            //    .WithMany(p => p.FollowUpDocumentHeadExtend)
            //    .HasForeignKey(d => d.FollowUpReferenceId)
            //    .HasConstraintName("FK_DocumentHeadExtend_AccountReferences_FollowUp");

            //entity.HasOne(d => d.CorroborantReference)
            //    .WithMany(p => p.CorroborantDocumentHeadExtend)
            //    .HasForeignKey(d => d.CorroborantReferenceId)

            //    .HasConstraintName("FK_DocumentHeadExtend_AccountReferencesCorroborant");

            //entity.HasOne(d => d.Receipt)
            //   .WithMany(p => p.DocumentHeadExtend)
            //   .HasForeignKey(d => d.DocumentHeadId)

            //   .HasConstraintName("FK_DocumentHeadExtend_DocumentHeads");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<DocumentHeadExtend> entity);
    }
}
