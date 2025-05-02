using Microsoft.EntityFrameworkCore;
using backend.Models;

namespace backend.Data
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options)
			: base(options) { }

		public DbSet<User> Users { get; set; }
		public DbSet<Plan> Plans { get; set; }
		public DbSet<PlanUser> PlanUsers { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<User>()
				.HasDiscriminator<string>("UserType")
				.HasValue<RegisteredUser>("RegisteredUser")
				.HasValue<GuestUser>("GuestUser");

			modelBuilder.Entity<PlanUser>()
				.Property(p => p.Role)
				.HasConversion<string>();


			modelBuilder.Entity<RegisteredUser>()
				.Property(u => u.IsVerified)
				.HasDefaultValue(false);
		}
	}
}