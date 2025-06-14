﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using Eefa.Commodity.Data.Context;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using System;
using Eefa.Commodity.Data.Entities;

#nullable disable

namespace Eefa.Commodity.Data.Context.Configurations
{
    public partial class BomsViewConfiguration : IEntityTypeConfiguration<BomsView>
    {
        public void Configure(EntityTypeBuilder<BomsView> entity)
        {
            entity.ToTable("BomsView", "common");

            entity.HasComment("فرمول های ساخت");

            entity.Property(e => e.Id).HasComment("شناسه");

            

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<BomsView> entity);
    }
}
