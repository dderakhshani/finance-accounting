﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using Eefa.Purchase.Infrastructure.Context;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using System;
using Eefa.Purchase.Domain.Entities.SqlView;

#nullable disable

namespace Eefa.Purchase.Infrastructure.Context.Configurations
{
    public partial class AccountReferenceEmployeeViewConfiguration : IEntityTypeConfiguration<AccountReferenceEmployeeView>
    {
        public void Configure(EntityTypeBuilder<AccountReferenceEmployeeView> entity)
        {
            entity.ToTable("AccountReferenceEmployeeView", "inventory");

           // OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<AccountReferenceEmployeeView> entity);
    }
}
