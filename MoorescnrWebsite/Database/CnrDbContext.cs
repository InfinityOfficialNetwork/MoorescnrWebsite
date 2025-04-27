using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Collections;
//using static MoorescnrWebsite.Models.CatalogViewModel;

namespace MoorescnrWebsite.Database
{
	public class CnrDbContext : DbContext
	{
		public DbSet<ImageEntity> Images { get; set; }
		public DbSet<GunEntity> Guns { get; set; }

		public CnrDbContext(DbContextOptions<CnrDbContext> options) : base(options)
		{}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<GunEntity>()
				.HasIndex(g => g.Id)
				.HasDatabaseName("idx_guns_id");

			modelBuilder.Entity<GunEntity>()
				.HasIndex(g => g.Name)
				.HasDatabaseName("idx_guns_name");

			modelBuilder.Entity<GunEntity>()
				.HasIndex(g => g.Description)
				.HasDatabaseName("idx_guns_description");

			modelBuilder.Entity<GunEntity>()
				.HasIndex(g => g.Price)
				.HasDatabaseName("idx_guns_price");

			modelBuilder.Entity<GunEntity>()
				.HasIndex(g => g.SalePrice)
				.HasDatabaseName("idx_guns_saleprice");

			modelBuilder.Entity<ImageEntity>()
				.HasIndex(i => i.Id)
				.HasDatabaseName("idx_images_id");

			modelBuilder.Entity<ImageEntity>()
				.HasIndex(i => i.GunId)
				.HasDatabaseName("idx_images_gunid");
		}

		//protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		//{
		//    optionsBuilder.UseSqlite("Data Source=moorescnr.db");
		//}
	}
}
