using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetMe.Data.Entities;

namespace PetMe.Data.Configurations
{
    public class AssistanceCfg : IEntityTypeConfiguration<Assistance>
    {
        public void Configure(EntityTypeBuilder<Assistance> builder)
        {
            builder.ToTable("Assistances");
        }
    }
}
