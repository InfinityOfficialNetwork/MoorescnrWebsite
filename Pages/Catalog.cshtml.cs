using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MoorescnrWebsite.Database;

namespace MoorescnrWebsite.Pages
{
	[ResponseCache(Duration = 3600, Location = ResponseCacheLocation.Any, NoStore = false)]
	public class CatalogModel : PageModel
	{
		private readonly ILogger<CatalogModel> logger;
		private readonly CnrDbContext context;

		public CatalogModel(ILogger<CatalogModel> logger, CnrDbContext context)
		{
			this.logger = logger;
			this.context = context;
		}

		public IEnumerable<GunInfo> GunInfos { get; private set; }
		
		[BindProperty(SupportsGet = true)]
		public int PageNumber { get; private set; }

		public int NumberOfGuns { get; private set; }

		public double TimeTook { get; private set; }

		public async Task OnGetAsync(int PageNumber = 0)
		{
			this.PageNumber = PageNumber;
			DateTime begin = DateTime.Now;
			NumberOfGuns = await context.GetNumberOfGunsAsync();
			GunInfos = await context.GetCatalogPageAsync(PageNumber);
			DateTime end = DateTime.Now;
			TimeTook = (end - begin).TotalMilliseconds;
		}
	}
}
