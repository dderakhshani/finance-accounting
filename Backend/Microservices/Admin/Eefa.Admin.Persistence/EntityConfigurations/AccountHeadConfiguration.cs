using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#nullable disable
public partial class AccountHeadConfiguration : IEntityTypeConfiguration<AccountHead>
{
    public void Configure(EntityTypeBuilder<AccountHead> entity)
    {
        entity.ToTable("AccountHead", "accounting");

        entity.HasComment("کدینک سرفصل حساب ها");

        entity.HasIndex(e => e.Code)
            .IsUnique();

        entity.Property(e => e.Id).HasComment("کد");

        entity.Property(e => e.BalanceBaseId).HasComment("کنترل ماهیت حساب ");

        entity.Property(e => e.BalanceId)
            .HasDefaultValueSql("((0))")
            .HasComment("نوع موازنه");

        entity.Property(e => e.BalanceName)
            .HasMaxLength(15)
            .HasComputedColumnSql("(case [balanceid] when (0) then N'بدهکار-بستانکار' when (1) then N'بدهکار' when (2) then N'بستانکار'  end)", true)
            .HasComment("شرح موازنه");

        entity.Property(e => e.Code)
            .IsRequired()
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasComment("شناسه");

        entity.Property(e => e.CodeLength)
            .HasComputedColumnSql("(len([Code]))", true)
            .HasComment("طول کد");

        entity.Property(e => e.CodeLevel).HasDefaultValueSql("((2))");

        entity.Property(e => e.CompanyId).HasComment("کد شرکت");

        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("(getdate())")
            .HasComment("تاریخ و زمان ایجاد");

        entity.Property(e => e.CreatedById).HasComment("ایجاد کننده");

        entity.Property(e => e.CurrencyBaseTypeId).HasComment("نوع ارز ");

        entity.Property(e => e.CurrencyFlag).HasComment("ویژگی ارزی دارد ؟");

        entity.Property(e => e.Description).HasMaxLength(250);

        entity.Property(e => e.ExchengeFlag).HasComment("تسعیر پذیر است ؟");

        entity.Property(e => e.GroupId).HasComment("کد گروه");

        entity.Property(e => e.IsActive)
            .IsRequired()
            .HasDefaultValueSql("((1))")
            .HasComment("فعال است؟");

        entity.Property(e => e.IsDeleted).HasComment("آیا حذف شده است؟");

        entity.Property(e => e.LastLevel).HasDefaultValueSql("((0))");

        entity.Property(e => e.LevelCode)
            .IsRequired()
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasDefaultValueSql("((0))")
            .HasComment("کد سطح");

        entity.Property(e => e.ModifiedAt)
            .HasDefaultValueSql("(getdate())")
            .HasComment("تاریخ و زمان اصلاح");

        entity.Property(e => e.ModifiedById).HasComment("اصلاح کننده");

        entity.Property(e => e.OwnerRoleId).HasComment("نقش صاحب سند");

        entity.Property(e => e.ParentId).HasComment("کد والد");

        entity.Property(e => e.QuantityFlag).HasComment("ویژگی تعداد دارد ؟");

        entity.Property(e => e.Title)
            .IsRequired()
            .HasMaxLength(100)
            .HasComment("عنوان");

        entity.Property(e => e.TraceFlag).HasComment("ویژگی پیگیری دارد ؟ ");

        entity.Property(e => e.TransferId).HasComment("وضعیت سند");

        entity.Property(e => e.TransferName)
            .HasMaxLength(4)
            .HasComputedColumnSql("(case [TransferId] when (1) then N'موقت' when (2) then N'دائم'  end)", true)
            .HasComment("شرح وضعیت سند");

        entity.HasOne(d => d.CreatedBy)
            .WithMany(p => p.AccountHeadCreatedBies)
            .HasForeignKey(d => d.CreatedById)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_AccountHead_Users");

        entity.HasOne(d => d.ModifiedBy)
            .WithMany(p => p.AccountHeadModifiedBies)
            .HasForeignKey(d => d.ModifiedById)
            .HasConstraintName("FK_AccountHead_Users1");

        entity.HasOne(d => d.OwnerRole)
            .WithMany(p => p.AccountHeads)
            .HasForeignKey(d => d.OwnerRoleId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_AccountHead_Roles");

        entity.HasOne(d => d.Parent)
            .WithMany(p => p.InverseParent)
            .HasForeignKey(d => d.ParentId)
            .HasConstraintName("FK_AccountHead_AccountHead");

        OnConfigurePartial(entity);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<AccountHead> entity);
}