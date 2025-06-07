using Eefa.Bursary.Domain.Entities;
using Eefa.Bursary.Domain.Entities.Payables;
using Eefa.Common.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eefa.Bursary.Infrastructure.EntityConfigurations.Configurations.Payables
{
    public partial class Payables_DocumentOperations_ChainConfiguration : IEntityTypeConfiguration<Payables_DocumentOperations_Chain>
    {
        public void Configure(EntityTypeBuilder<Payables_DocumentOperations_Chain> entity)
        {
            entity.ToTable("Payables_DocumentOperations_Chain", "bursary");
            entity.HasKey(x => new { x.SourceCode, x.DestCode });
        }
        partial void OnConfigurePartial(EntityTypeBuilder<Payables_DocumentOperations_Chain> entity);

    }
}
