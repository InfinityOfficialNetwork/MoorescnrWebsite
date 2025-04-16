using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Collections;
//using static MoorescnrWebsite.Models.CatalogViewModel;

namespace MoorescnrWebsite.Database
{
	public class CnrDbContext : DbContext
	{
		private readonly IMemoryCache _memoryCache;

		private static readonly Func<CnrDbContext,int,Task<ImageEntity?>> imageQuery = EF.CompileAsyncQuery((CnrDbContext context, int id) =>
			context.Images.Where(i => i.Id == id).FirstOrDefault()
		);

		public async Task<ImageEntity?> FindImageAsync(int id)
		{
			if (_memoryCache.TryGetValue<ImageEntity?>($"{nameof(imageQuery)}_{id}", out var image))
				return image;
			else
			{
				image = await imageQuery(this,id);
				_memoryCache.Set($"{nameof(imageQuery)}_{id}", image);
				return image;
			}
		}

		private static readonly Func<CnrDbContext, Task<int>> numberOfGunsQuery = EF.CompileAsyncQuery((CnrDbContext context) =>
			context.Guns.Count()
		);

		public async Task<int> GetNumberOfGunsAsync()
		{
			if (_memoryCache.TryGetValue<int>(numberOfGunsQuery, out int count))
				return count;
			else
			{
				int num = await numberOfGunsQuery(this);
				_memoryCache.Set(numberOfGunsQuery, num);
				return num;
			}
		}

		private static readonly Func<CnrDbContext, int, IAsyncEnumerable<GunInfo>> catalogQuery = EF.CompileAsyncQuery((CnrDbContext context, int page) =>
			context.Guns
			.OrderByDescending(entity => entity.Id)
			.Skip(page * GunInfo.PageCount)
			.Take(GunInfo.PageCount)
			.GroupJoin(context.Images, entity => entity.Id, image => image.GunId, (left, right) => new { Left = left, Right = right.Select(i => i.Id) })
			.Select(gun => new GunInfo
			{
				Id = gun.Left.Id,
				Description = gun.Left.Description,
				IsAvailable = gun.Left.IsAvailable,
				ManufacturedYear = gun.Left.ManufacturedYear,
				Name = gun.Left.Name,
				Price = gun.Left.Price,
				SalePrice = gun.Left.SalePrice,

				ImageIds = gun.Right.ToArray()
			})
		);

		public async Task<IList<GunInfo>> GetCatalogPageAsync(int page)
		{
			if (_memoryCache.TryGetValue<IList<GunInfo>>($"{nameof(catalogQuery)}_{page}",out var gunInfos))
			{
				return gunInfos!;
			}
			else
			{
				var guns = catalogQuery(this, page);
				var list = new List<GunInfo>();
				await foreach (var gun in guns)
					list.Add(gun);
				_memoryCache.Set($"{nameof(catalogQuery)}_{page}", list);
				return list;
			}
		}

		private static readonly Func<CnrDbContext, int, Task<GunInfo>> itemQuery = EF.CompileAsyncQuery((CnrDbContext context, int id) =>
			context.Guns
			.Where(gun => gun.Id == id)
			.GroupJoin(context.Images, entity => entity.Id, image => image.GunId, (left, right) => new { Left = left, Right = right.Select(i => i.Id) })
			.Select(gun => new GunInfo
			{
				Id = gun.Left.Id,
				Description = gun.Left.Description,
				IsAvailable = gun.Left.IsAvailable,
				ManufacturedYear = gun.Left.ManufacturedYear,
				Name = gun.Left.Name,
				Price = gun.Left.Price,
				SalePrice = gun.Left.SalePrice,

				ImageIds = gun.Right.ToArray()
			})
			.First()
		);

		public async Task<GunInfo> GetItemPageAsync(int id)
		{
			if (_memoryCache.TryGetValue<GunInfo>($"{nameof(itemQuery)}_{id}", out var item))
			{
				return item;
			}
			else
			{
				item = await itemQuery(this, id);
				_memoryCache.Set($"{nameof(itemQuery)}_{id}", item);
				return item;
			}
		}

		public DbSet<ImageEntity> Images { get; set; }
		public DbSet<GunEntity> Guns { get; set; }

		public CnrDbContext(DbContextOptions<CnrDbContext> options, IMemoryCache cache) : base(options)
		{
			_memoryCache = cache;
		}

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
