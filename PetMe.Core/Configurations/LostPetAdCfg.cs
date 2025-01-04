using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetMe.Core.Entities;

namespace PetMe.Core.Configurations
{
    public class LostPetAdCfg : IEntityTypeConfiguration<LostPetAd>
    {
        public void Configure(EntityTypeBuilder<LostPetAd> builder)
        {
            builder.ToTable("LostPetAds");

            builder
                .HasOne(l => l.User)
                .WithMany()
                .HasForeignKey(l => l.UserId);

            builder
                .HasOne(l => l.User)
                .WithMany()
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .Property(l => l.LastSeenCity)
                .IsRequired(false);

            builder
                .Property(l => l.LastSeenDistrict)
                .IsRequired(false);
        }
    }
}
