using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using MoorescnrWebsite.Database;

namespace MoorescnrWebsite.Pages
{
    public class ItemModel : PageModel
    {
        private readonly ILogger<CatalogModel> logger;
        private readonly CnrDbContext context;
        private readonly IMemoryCache memoryCache;

        private static readonly Func<CnrDbContext, int, Task<GunInfo>> itemQuery = EF.CompileAsyncQuery((CnrDbContext context, int id) => context.Guns
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
            if (memoryCache.TryGetValue<GunInfo>($"{nameof(itemQuery)}_{id}", out var item))
            {
                return item!;
            }
            else
            {
                item = await itemQuery(context, id);
                memoryCache.Set($"{nameof(itemQuery)}_{id}", item);
                return item;
            }
        }

        public ItemModel(ILogger<CatalogModel> logger, CnrDbContext context, IMemoryCache memoryCache)
        {
            this.logger = logger;
            this.context = context;
            this.memoryCache = memoryCache;
        }

        public GunInfo GunInfo { get; private set; }

        [BindProperty(SupportsGet = true)]
        public int GunId { get; private set; }

        public async Task OnGetAsync(int GunId)
        {
            this.GunId = GunId;
            GunInfo = await GetItemPageAsync(this.GunId);
        }
    }
}
