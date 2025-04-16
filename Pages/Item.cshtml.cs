using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MoorescnrWebsite.Database;

namespace MoorescnrWebsite.Pages
{
	public class ItemModel : PageModel
	{
		private readonly ILogger<CatalogModel> logger;
		private readonly CnrDbContext context;

		public ItemModel(ILogger<CatalogModel> logger, CnrDbContext context)
		{
			this.logger = logger;
			this.context = context;
		}

		public GunInfo GunInfo { get; private set; }

		[BindProperty(SupportsGet = true)]
		public int GunId { get; private set; }

		public async Task OnGetAsync(int GunId)
		{
			this.GunId = GunId;
			GunInfo = await context.GetItemPageAsync(this.GunId);
		}
	}
}
