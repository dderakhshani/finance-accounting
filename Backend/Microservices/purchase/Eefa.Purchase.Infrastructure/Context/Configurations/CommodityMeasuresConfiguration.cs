using Eefa.Purchase.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eefa.Purchase.Infrastructure.Context.Configurations
{
    public partial class CommodityMeasuresConfiguration : IEntityTypeConfiguration<CommodityMeasures>
    {
        public void Configure(EntityTypeBuilder<CommodityMeasures> entity)
        {
            entity.ToTable("CommodityMeasures", "common");

            entity.Property(e => e.Id).HasComment("شناسه");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<CommodityMeasures> entity);
    }
}
