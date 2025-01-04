using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetMe.Core.Entities;

namespace PetMe.Core.Configurations
{
    public class CommentCfg : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder
                .HasOne(c => c.HelpRequest)
                .WithMany(h => h.Comments)
                .HasForeignKey(c => c.HelpRequestId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
               .HasOne(c => c.Veterinarian)
               .WithMany(v => v.Comments)
               .HasForeignKey(c => c.VeterinarianId)
               .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
