using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#nullable disable

namespace FileTransfer.WebApi.Persistance.Entities.Configurations
{
    public partial class inventory_StockFromTadbirConfiguration : IEntityTypeConfiguration<inventory_StockFromTadbir>
    {
        public void Configure(EntityTypeBuilder<inventory_StockFromTadbir> entity)
        {
            entity.ToTable("inventory.StockFromTadbir", "dbo");
            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<inventory_StockFromTadbir> entity);
    }
}
