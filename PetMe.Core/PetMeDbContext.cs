using Microsoft.EntityFrameworkCore;
using PetMe.Data.Configurations;
using PetMe.Data.Entities;
using PetMe.Setting;
namespace PetMe.Data
{
    public class PetMeDbContext : DbContext
    {
        private EnviromentSetting _setting = EnviromentSetting.GetInstance();

        public PetMeDbContext() { }

        public PetMeDbContext(DbContextOptions<PetMeDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(_setting.GetConnectionString());
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AdminCfg());
            modelBuilder.ApplyConfiguration(new AdoptionCfg());
            modelBuilder.ApplyConfiguration(new AdoptionRequestCfg());
            modelBuilder.ApplyConfiguration(new AssistanceCfg());
            modelBuilder.ApplyConfiguration(new CommentCfg());
            modelBuilder.ApplyConfiguration(new HelpRequestCfg());
            modelBuilder.ApplyConfiguration(new LostPetAdCfg());
            modelBuilder.ApplyConfiguration(new PetOwnerCfg());
            modelBuilder.ApplyConfiguration(new VeterinarianCfg());
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Pet> Pets { get; set; }
        public DbSet<Adoption> Adoptions { get; set; }
        public DbSet<Assistance> Assistances { get; set; }
        public DbSet<PetOwner> PetOwners { get; set; }
        public DbSet<AdoptionRequest> AdoptionRequests { get; set; }
        public DbSet<LostPetAd> LostPetAds { get; set; }
        public DbSet<HelpRequest> HelpRequests { get; set; }
        public DbSet<Veterinarian> Veterinarians { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }
}
