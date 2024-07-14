using AssetManagement.Domain.Entites;
using AssetManagement.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AssetManagement.Infrastructure.Contexts
{
    public class ApplicationDbContext : DbContext
    {
        private readonly PasswordHasher<User> _passwordHasher = new PasswordHasher<User>();

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Asset> Assets { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<ReturnRequest> ReturnRequests { get; set; }
        public DbSet<Token> Token { get; set; }
        public DbSet<BlackListToken> BlackListTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .Property(p => p.StaffCode)
                .HasComputedColumnSql("CONCAT('SD', RIGHT('0000' + CAST(StaffCodeId AS VARCHAR(4)), 4))");

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            //1-1 token
            modelBuilder.Entity<Token>()
                .HasOne(t => t.User)
                .WithOne(u => u.Token)
                .HasForeignKey<Token>(t => t.UserId);

            // 1-n category-asset
            modelBuilder.Entity<Assignment>()
                .HasOne(a => a.AssignedTo)
                .WithMany(u => u.AssignmentsReceived)
                .HasForeignKey(a => a.AssignedIdTo)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Assignment>()
                .HasOne(a => a.AssignedBy)
                .WithMany(u => u.AssignmentsCreated)
                .HasForeignKey(a => a.AssignedIdBy)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Assignment>()
                .HasOne(a => a.Asset)
                .WithMany(a => a.Assignments)
                .HasForeignKey(a => a.AssetId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ReturnRequest>()
               .HasOne(rr => rr.Assignment)
               .WithOne(a => a.ReturnRequest)
               .HasForeignKey<ReturnRequest>(rr => rr.AssignmentId)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ReturnRequest>()
                .HasOne(rr => rr.RequestedUser)
                .WithMany(u => u.ReturnRequestsRequested)
                .HasForeignKey(rr => rr.RequestedBy)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ReturnRequest>()
                .HasOne(rr => rr.AcceptedUser)
                .WithMany(u => u.ReturnRequestsAccepted)
                .HasForeignKey(rr => rr.AcceptedBy)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Category>().HasIndex(c => c.CategoryName).IsUnique();
            modelBuilder.Entity<Category>().HasIndex(c => c.Prefix).IsUnique();
            modelBuilder.Entity<Category>().Property(c => c.Id).ValueGeneratedOnAdd();

            // Seed data and other configurations
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            var adminHN = new User
            {
                FirstName = "Admin",
                LastName = "Ha Noi",
                Role = RoleType.Admin,
                Location = EnumLocation.HaNoi,
                IsFirstTimeLogin = false,
                Username = "adminHN"
            };
            adminHN.PasswordHash = _passwordHasher.HashPassword(adminHN, "adminpassword");
            adminHN.CreatedOn = DateTime.Now;
            adminHN.CreatedBy = "System";

            var adminHCM = new User
            {
                FirstName = "Admin",
                LastName = "Ho Chi Minh",
                Role = RoleType.Admin,
                Location = EnumLocation.HoChiMinh,
                IsFirstTimeLogin = false,
                Username = "adminHCM"
            };
            adminHCM.PasswordHash = _passwordHasher.HashPassword(adminHCM, "adminpassword");
            adminHCM.CreatedOn = DateTime.Now;
            adminHCM.CreatedBy = "System";

            var adminDN = new User
            {
                FirstName = "Admin",
                LastName = "Da Nang",
                Role = RoleType.Admin,
                Location = EnumLocation.DaNang,
                IsFirstTimeLogin = false,
                Username = "adminDN"
            };
            adminDN.PasswordHash = _passwordHasher.HashPassword(adminDN, "adminpassword");
            adminDN.CreatedOn = DateTime.Now;
            adminDN.CreatedBy = "System";

            //modelBuilder.Entity<User>().HasData(adminHN, adminHCM, adminDN);
            modelBuilder.Entity<User>().Property(u => u.StaffCodeId).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

            //modelBuilder.Entity<Category>().HasData(
            //    new Category { CategoryName = "Laptop", Prefix = "LA" },
            //    new Category { CategoryName = "Monitor", Prefix = "MO" },
            //    new Category { CategoryName = "Desk", Prefix = "DE" }
            //);
        }
    }
}