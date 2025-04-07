using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using MoorescnrWebsite.Database;

namespace MoorescnrBackend.Controllers;


//dirt simple image endpoint to return the image file to the id
public class ImageController : ControllerBase
{
	private readonly ILogger<ImageController> logger;
	private readonly IDbContextFactory<CnrDbContext> dbContextFactory;

	public ImageController(ILogger<ImageController> logger, IDbContextFactory<CnrDbContext> dbContextFactory)
	{
		this.logger = logger;
		this.dbContextFactory = dbContextFactory;
	}

	public async Task<IActionResult> GetImage([FromQuery] int id)
	{
		logger.LogInformation($"Client requested HTTP GET /images/GetImage with id {id}");
		ImageEntity? image;
		using (var databaseContext = await dbContextFactory.CreateDbContextAsync())
			image = await databaseContext.FindImageAsync(id);
		if (image == null || image.Data == null)
		{
			return NotFound();
		}

		return File(image.Data, "image/png");
	}
}