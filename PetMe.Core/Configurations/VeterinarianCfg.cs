using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetMe.Data.Entities;

namespace PetMe.Data.Configurations
{
    public class VeterinarianCfg : IEntityTypeConfiguration<Veterinarian>
    {
        public void Configure(EntityTypeBuilder<Veterinarian> builder)
        {
            builder
                        .HasOne(v => v.User)
                        .WithMany()
                        .HasForeignKey(v => v.UserId)
                        .OnDelete(DeleteBehavior.Cascade);

            builder
               .Property(v => v.Status)
               .HasConversion<string>();
        }
    }
}
