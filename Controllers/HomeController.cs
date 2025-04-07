using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using MoorescnrWebsite.Database;
using MoorescnrWebsite.Models;
using static MoorescnrWebsite.Models.CatalogViewModel;

namespace MoorescnrWebsite.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly IDbContextFactory<CnrDbContext> _dbContextFactory;
		private readonly IMemoryCache _cache;

		public HomeController(ILogger<HomeController> logger, IDbContextFactory<CnrDbContext> dbContextFactory, IMemoryCache memoryCache)
		{
			_logger = logger;
			_dbContextFactory = dbContextFactory;
			_cache = memoryCache;
		}

		public IActionResult Index()
		{
			if (_cache.TryGetValue<ViewResult>(nameof(Index), out var result)) 
				return result!;
			else
			{
				result = View();
				_cache.Set(nameof(Index), result);
				return result;
			}
		}
		public IActionResult ContactUs()
		{
			return View();
		}

		public async Task<IActionResult> Catalog(int page)
		{
			using (var databaseContext = await _dbContextFactory.CreateDbContextAsync())
			{
				var gunInfos = await databaseContext.GetCatalogPageAsync(page);
				var numGuns = await databaseContext.GetNumberOfGunsAsync();

				if (gunInfos.Count == 0)
					return NoContent();
				else
					return View(new CatalogViewModel { GunInfos = gunInfos, Page = page, NumberOfGuns = numGuns });
			}
		}

		public async Task<IActionResult> Item(int id)
		{
			using (var databaseContext = await _dbContextFactory.CreateDbContextAsync())
			{
				var item = await databaseContext.GetItemPageAsync(id);
				return View(new CatalogViewModel { GunInfos = [item], Page = 0, NumberOfGuns = 0 });
			}
		}

		public IActionResult FAQ()
		{
			return View();
		}

		public IActionResult Policy()
		{
			return View();
		}

		public IActionResult PurchaseRequirements()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
