//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Eefa.Admin.Data.Databases.Entities.Configurations
//{
//    public class TableConfiguration : IEntityTypeConfiguration<Table>
//    {
//        public void Configure(EntityTypeBuilder<Table> builder)
//        {
//            builder.ToTable("Tables", "admin");
//            builder.HasComment("جداول");
//            builder.Property(a=>a.NameFa).HasMaxLength(150).IsRequired().HasComment("نام فارسی جدول");
//            builder.Property(a => a.NameEn).HasMaxLength(150).IsRequired().HasComment("نام انگلیسی جدول");
//            builder.Property(e => e.OwnerRoleId).HasComment("نقش صاحب سند");
//            //builder.HasOne(d => d.ModifiedBy)
//            //    .WithMany(p => p.PersonModifiedBies)
//            //    .HasForeignKey(d => d.ModifiedById)
//            //    .HasConstraintName("FK_Persons_Users1");

//            //builder.HasOne(d => d.OwnerRole)
//            //    .WithMany(p => p.Persons)
//            //    .HasForeignKey(d => d.OwnerRoleId)
//            //    .OnDelete(DeleteBehavior.ClientSetNull)
//            //    .HasConstraintName("FK_Persons_Roles");
//        }
//    }
//}
