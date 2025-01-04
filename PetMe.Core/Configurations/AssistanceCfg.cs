using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetMe.Core.Entities;

namespace PetMe.Core.Configurations
{
    public class AssistanceCfg : IEntityTypeConfiguration<Assistance>
    {
        public void Configure(EntityTypeBuilder<Assistance> builder)
        {
            builder.ToTable("Assistances");
        }
    }
}
