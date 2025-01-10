using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetMe.Data.Entities;

namespace PetMe.Data.Configurations
{
    public class AdminCfg : IEntityTypeConfiguration<Admin>
    {
        public void Configure(EntityTypeBuilder<Admin> builder)
        {
            builder
                    .HasOne(a => a.User)
                    .WithOne()
                    .HasForeignKey<Admin>(a => a.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
        }


    }
}
