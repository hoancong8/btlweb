using Microsoft.EntityFrameworkCore;
namespace BTL.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<RvImage> RvImages { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Noti> Notis { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Các thiết lập bổ sung (nếu cần)
            modelBuilder.Entity<User>().Property(u => u.Role).HasDefaultValue(true);
            modelBuilder.Entity<User>().Property(u => u.IsActive).HasDefaultValue(true);
        }
    }
}