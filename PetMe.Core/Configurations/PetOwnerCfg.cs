using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetMe.Data.Entities;

namespace PetMe.Data.Configurations
{
    public class PetOwnerCfg : IEntityTypeConfiguration<PetOwner>
    {
        public void Configure(EntityTypeBuilder<PetOwner> builder)
        {
            builder.ToTable("PetOwners");
            builder.HasKey(po => new { po.PetId, po.UserId });
            builder.HasOne(po => po.Pet).WithMany(p => p.PetOwners).HasForeignKey(po => po.PetId);
            builder.HasOne(po => po.User).WithMany(u => u.PetOwners).HasForeignKey(po => po.UserId);
        }
    }
}
