using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using MoorescnrWebsite.Database;

namespace MoorescnrWebsite.Pages
{
	[ResponseCache(Duration = 3600, Location = ResponseCacheLocation.Any, NoStore = false)]
	public class CatalogModel : PageModel
	{
		private readonly ILogger<CatalogModel> logger;
		private readonly CnrDbContext context;
        private readonly IMemoryCache memoryCache;

        private static readonly Func<CnrDbContext, int, Task<ImageEntity?>> imageQuery = EF.CompileAsyncQuery((CnrDbContext context, int id) =>
            context.Images.Where(i => i.Id == id).FirstOrDefault()
        );

        public async Task<ImageEntity?> FindImageAsync(int id)
        {
            if (memoryCache.TryGetValue<ImageEntity?>($"{nameof(imageQuery)}_{id}", out var image))
                return image;
            else
            {
                image = await imageQuery(context, id);
                memoryCache.Set($"{nameof(imageQuery)}_{id}", image);
                return image;
            }
        }

        private static readonly Func<CnrDbContext, Task<int>> numberOfGunsQuery = EF.CompileAsyncQuery((CnrDbContext context) =>
            context.Guns.Count()
        );

        public async Task<int> GetNumberOfGunsAsync()
        {
            if (memoryCache.TryGetValue<int>(numberOfGunsQuery, out int count))
                return count;
            else
            {
                int num = await numberOfGunsQuery(context);
                memoryCache.Set(numberOfGunsQuery, num);
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
            if (memoryCache.TryGetValue<IList<GunInfo>>($"{nameof(catalogQuery)}_{page}", out var gunInfos))
            {
                return gunInfos!;
            }
            else
            {
                var guns = catalogQuery(context, page);
                var list = new List<GunInfo>();
                await foreach (var gun in guns)
                    list.Add(gun);
                memoryCache.Set($"{nameof(catalogQuery)}_{page}", list);
                return list;
            }
        }

        public CatalogModel(ILogger<CatalogModel> logger, CnrDbContext context, IMemoryCache memoryCache)
		{
			this.logger = logger;
			this.context = context;
            this.memoryCache = memoryCache;
		}

		public IEnumerable<GunInfo> GunInfos { get; private set; }

		[BindProperty(SupportsGet = true)]
		public int PageNumber { get; private set; } = 0;

		public int NumberOfGuns { get; private set; } = 0;

		public double TimeTook { get; private set; } = 0;

		public async Task OnGetAsync(int PageNumber = 0)
		{
			this.PageNumber = PageNumber;
			DateTime begin = DateTime.Now;
			NumberOfGuns = await GetNumberOfGunsAsync();
			GunInfos = await GetCatalogPageAsync(PageNumber);
			DateTime end = DateTime.Now;
			TimeTook = (end - begin).TotalMilliseconds;
		}
	}
}
