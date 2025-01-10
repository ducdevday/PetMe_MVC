using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetMe.Data.Entities;
using System.Reflection.Emit;

namespace PetMe.Data.Configurations
{
    public class AdoptionCfg : IEntityTypeConfiguration<Adoption>
    {
        public void Configure(EntityTypeBuilder<Adoption> builder)
        {
            builder.ToTable("Adoptions");
            builder.HasOne(a => a.Pet).WithMany().HasForeignKey(a => a.PetId);
            builder.HasOne(a => a.User).WithMany().HasForeignKey(a => a.UserId);
        }
    }
}
