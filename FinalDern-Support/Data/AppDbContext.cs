using FinalDern_Support.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FinalDern_Support.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        { }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<JobSpareParts> JobSpareParts { get; set; }
        public DbSet<KnowledgeBase> KnowledgeBases { get; set; }
        public DbSet<Quote> Quotes { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<SparePart> SpareParts { get; set; }
        public DbSet<Technician> Technicians { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<JobSpareParts>()
              .HasKey(sp => new { sp.JobID, sp.SparePartID });

            modelBuilder.Entity<Admin>()
         .HasOne(a => a.User)
         .WithOne(u => u.Admin)
         .HasForeignKey<Admin>(a => a.UserID)
         .OnDelete(DeleteBehavior.Restrict);  // Prevent cascading deletes

            // Customer -> ApplicationUser (One-to-One)
            modelBuilder.Entity<Customer>()
                .HasOne(c => c.User)
                .WithOne(u => u.Customer)
                .HasForeignKey<Customer>(c => c.UserID)
                .OnDelete(DeleteBehavior.Restrict);  // Prevent cascading deletes

            // Technician -> ApplicationUser (One-to-One)
            modelBuilder.Entity<Technician>()
                .HasOne(t => t.User)
                .WithOne(u => u.Technician)
                .HasForeignKey<Technician>(t => t.UserID)
                .OnDelete(DeleteBehavior.Restrict);  // Prevent cascading deletes

            // Feedback -> Customer (Many-to-One)
            modelBuilder.Entity<Feedback>()
                .HasOne(f => f.Customer)
                .WithMany(c => c.Feedbacks)
                .HasForeignKey(f => f.CustomerID)
                .OnDelete(DeleteBehavior.Restrict);  // Prevent cascading deletes

            // Feedback -> Job (One-to-One)
            modelBuilder.Entity<Feedback>()
                .HasOne(f => f.Job)
                .WithOne()
                .HasForeignKey<Feedback>(f => f.JobID)
                .OnDelete(DeleteBehavior.Restrict);  // Prevent cascading deletes

            // Job -> Quote (One-to-One)
            modelBuilder.Entity<Job>()
                .HasOne(j => j.Quote)
                .WithOne(q => q.Job)
                .HasForeignKey<Job>(j => j.QuoteID)
                .OnDelete(DeleteBehavior.Restrict);  // Prevent cascading deletes

            // Job -> Technician (Many-to-One)
            modelBuilder.Entity<Job>()
                .HasOne(j => j.Technician)
                .WithMany(t => t.Jobs)
                .HasForeignKey(j => j.TechID)
                .OnDelete(DeleteBehavior.Restrict);  // Prevent cascading deletes

            // Job -> Report (One-to-One, optional)
                 modelBuilder.Entity<Report>()
                .HasOne(r => r.Job)
                .WithOne()  // No navigation property on Job side
                .HasForeignKey<Report>(r => r.JobID)
                .OnDelete(DeleteBehavior.Restrict);

            // Job -> Feedback (One-to-One, optional)
            //modelBuilder.Entity<Job>()
            //    .HasOne(j => j.Feedback)
            //    .WithOne(f => f.Job)
            //    .HasForeignKey<Job>(j => j.FeedbackID)
            //    .OnDelete(DeleteBehavior.Restrict);  // Prevent cascading deletes

            // JobSpareParts -> Job (Many-to-One)
            modelBuilder.Entity<JobSpareParts>()
                .HasOne(jsp => jsp.Job)
                .WithMany(j => j.JobSpareParts)
                .HasForeignKey(jsp => jsp.JobID)
                .OnDelete(DeleteBehavior.Restrict);  // Prevent cascading deletes

            // JobSpareParts -> SparePart (Many-to-One)
            modelBuilder.Entity<JobSpareParts>()
                .HasOne(jsp => jsp.SparePart)
                .WithMany(sp => sp.JobSpareParts)
                .HasForeignKey(jsp => jsp.SparePartID)
                .OnDelete(DeleteBehavior.Restrict);  // Prevent cascading deletes

            // KnowledgeBase -> Admin (Many-to-One)
            modelBuilder.Entity<KnowledgeBase>()
                .HasOne(kb => kb.Admin)
                .WithMany(a => a.KnowledgeBases)
                .HasForeignKey(kb => kb.AdminID)
                .OnDelete(DeleteBehavior.Restrict);  // Prevent cascading deletes

            // Quote -> Request (One-to-One)
            modelBuilder.Entity<Quote>()
                .HasOne(q => q.Request)
                .WithOne(r => r.Quote)
                .HasForeignKey<Quote>(q => q.RequestID)
                .OnDelete(DeleteBehavior.Restrict);  // Prevent cascading deletes

            // Report -> Technician (Many-to-One)
            modelBuilder.Entity<Report>()
                .HasOne(r => r.Technician)
                .WithMany(t => t.Reports)
                .HasForeignKey(r => r.TechnicianID)
                .OnDelete(DeleteBehavior.Restrict);  // Prevent cascading deletes

            //// Report -> Job (One-to-One)
            //modelBuilder.Entity<Report>()
            //    .HasOne(r => r.Job)
            //    .WithOne(j => j.Report)
            //    .HasForeignKey<Report>(r => r.JobID)
            //    .OnDelete(DeleteBehavior.Restrict);  // Prevent cascading deletes

            // Request -> Customer (Many-to-One)
            modelBuilder.Entity<Request>()
                .HasOne(r => r.Customer)
                .WithMany(c => c.Requests)
                .HasForeignKey(r => r.CustomerID)
                .OnDelete(DeleteBehavior.Restrict);  // Prevent cascading deletes
                                                     // Add this configuration if it's not already present
         //   modelBuilder.Entity<Request>()
         //.HasOne(r => r.Job)
         //.WithOne(j => j.Request)
         //.HasForeignKey<Request>(r => r.JobID)
         //.OnDelete(DeleteBehavior.Restrict);

            // Seed roles
            SeedRole(modelBuilder, "Admin");
            SeedRole(modelBuilder, "Customer");
            SeedRole(modelBuilder, "Technician");
        }


        private void SeedRole(ModelBuilder modelBuilder, string roleName, params string[] permissions)
        {
            // Seed the role
            var role = new IdentityRole
            {
                Id = roleName.ToLower(),
                Name = roleName,
                NormalizedName = roleName.ToUpper(),
                ConcurrencyStamp = Guid.NewGuid().ToString()
            };

            modelBuilder.Entity<IdentityRole>().HasData(role);

            // Add claims for the role if any permissions are provided
            if (permissions != null && permissions.Length > 0)
            {
                var roleClaims = permissions.Select(permission => new IdentityRoleClaim<string>
                {
                    RoleId = role.Id,
                    ClaimType = "Permission",
                    ClaimValue = permission
                }).ToArray();

                modelBuilder.Entity<IdentityRoleClaim<string>>().HasData(roleClaims);
            }
        }

    }

}