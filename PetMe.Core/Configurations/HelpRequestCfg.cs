using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetMe.Data.Entities;

namespace PetMe.Data.Configurations
{
    public class HelpRequestCfg : IEntityTypeConfiguration<HelpRequest>
    {
        public void Configure(EntityTypeBuilder<HelpRequest> builder)
        {
            builder.ToTable("HelpRequests");

            builder
                .HasOne(h => h.User)
                .WithMany(u => u.HelpRequests)
                .HasForeignKey(h => h.UserId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
