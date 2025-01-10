using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetMe.Data.Entities;

namespace PetMe.Data.Configurations
{
    public class AdoptionRequestCfg : IEntityTypeConfiguration<AdoptionRequest>
    {
        public void Configure(EntityTypeBuilder<AdoptionRequest> builder)
        {
            builder.ToTable("AdoptionRequests");
            builder
                .HasOne(ar => ar.Pet)
                .WithMany(p => p.AdoptionRequests)
                .HasForeignKey(ar => ar.PetId);
            builder
                .HasOne(ar => ar.User)
                .WithMany(u => u.AdoptionRequests)
                .HasForeignKey(ar => ar.UserId);
        }
    }
}
